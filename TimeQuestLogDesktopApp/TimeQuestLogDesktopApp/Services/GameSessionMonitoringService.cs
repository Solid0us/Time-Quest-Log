using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TimeQuestLogDesktopApp.Database;
using TimeQuestLogDesktopApp.Models;
using TimeQuestLogDesktopApp.Models.DTOs;
using TimeQuestLogDesktopApp.Repositories;

namespace TimeQuestLogDesktopApp.Services
{
    class GameSessionMonitoringService : IDisposable, INotifyPropertyChanged
    {
		private static GameSessionMonitoringService _instance;
		private static readonly object lockObject = new object();
		private ManagementEventWatcher _startWatch;
        private ManagementEventWatcher _stopWatch;
		private bool isMonitoring;
		private bool disposed;
		private ConcurrentDictionary<int, string> _processIdToNameMap = new();
		private ConcurrentDictionary<string, int> _gameMap = new();
		private ConcurrentDictionary<int, GameSessions> _gameSessionMap = new();
		private List<GameSessionsDTO> _gameSessions = new List<GameSessionsDTO>();
		private GameSessionsRepository _gameSessionsRepository;
		private UserGameRepository _userGameRepository;
		private readonly CredentialManagerService _credentialManagerService;
		private readonly EnvironmentVariableService _environmentVariableService;
		private readonly HttpService _httpService;

		private int _numberUnsynced = 0;
		public int NumberUnsynced
		{
			get { return _numberUnsynced; }
			set
			{
				_numberUnsynced = value;
				OnPropertyChanged(nameof(NumberUnsynced));
			}
		}

		public event EventHandler<List<GameSessionsDTO>> GameSessionsChanged;
		public event PropertyChangedEventHandler? PropertyChanged;

		private GameSessionMonitoringService()
        {
			var sqliteDataAccess = new SqliteDataAccess();
			var sqliteConnectionFactory = new SqliteConnectionFactory(sqliteDataAccess.LoadConnectionString());
			_gameSessionsRepository = new GameSessionsRepository(sqliteConnectionFactory);
			_userGameRepository = new UserGameRepository(sqliteConnectionFactory);
			_environmentVariableService = EnvironmentVariableService.Instance;
			_httpService = HttpService.GetInstance();
			_credentialManagerService = CredentialManagerService.GetInstance();
			_credentialManagerService.LoadCredentials();
			LoadGameSessions();
			LoadExeMap();
			UpdateUnsyncedCounter();
		}

        public List<GameSessionsDTO> GameSessions 
		{
			get => _gameSessions;
			private set
			{
				_gameSessions = value;
				GameSessionsChanged?.Invoke(this, _gameSessions);
			}
		}

		protected void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public static GameSessionMonitoringService Instance
		{
			get
			{
				if (_instance == null)
				{
					lock (lockObject)
					{
						if (_instance == null)
						{
							_instance = new GameSessionMonitoringService();
						}
					}
				}
				return _instance;
			}
		}

		public void StartMonitoring()
        {
			string startQuery = "SELECT * FROM Win32_ProcessStartTrace";
			_startWatch = new ManagementEventWatcher(new WqlEventQuery(startQuery));
			_startWatch.EventArrived += new EventArrivedEventHandler(ProcessStarted);
			_startWatch.Start();

			string stopQuery = "SELECT * FROM Win32_ProcessStopTrace";
			_stopWatch = new ManagementEventWatcher(new WqlEventQuery(stopQuery));
			_stopWatch.EventArrived += new EventArrivedEventHandler(ProcessStopped);
			_stopWatch.Start();

			isMonitoring = true;
		}

		public void StopMonitoring()
		{
			if (!isMonitoring)
				return;

			CleanupWatchers();
		}

		private Task<HttpResponseMessage> SendGameSessionAsync(GameSessions gameSession)
		{

			string url = _environmentVariableService.ApiBaseUrl + $"game-sessions/{gameSession.Id}";
			return _httpService.PutAsync(url, new
			{
				user = new { id = gameSession.UserId },
				game = new { id = gameSession.GameId },
				startTime = gameSession.StartTime,
				endTime = gameSession.EndTime
			});

		}

		private async void ProcessStarted(object sender, EventArrivedEventArgs e)
		{
			string processName = e.NewEvent.Properties["ProcessName"].Value.ToString();
			int processId = Convert.ToInt32(e.NewEvent.Properties["ProcessID"].Value);
			if (!_gameMap.TryGetValue(processName, out var game))
				return;

			string userId = _credentialManagerService.GetUserId(CredentialManagerService.CredentialType.REFRESH);
			GameSessions gameSession = new GameSessions(_gameMap[processName], userId);
			if (!_gameSessionMap.TryAdd(processId, gameSession))
				return;
			_processIdToNameMap.TryAdd(processId, processName);
			_gameSessionsRepository.CreateGameSession(gameSession);
			try
			{
				string url = _environmentVariableService.ApiBaseUrl + $"game-sessions/{gameSession.Id}";
				HttpResponseMessage response = await _httpService.SendAndRepeatAuthorization(() => SendGameSessionAsync(gameSession));
				_gameSessionsRepository.UpdateSync(gameSession.Id, response.IsSuccessStatusCode);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error in ProcessStarted: {ex.Message}");
			}
			finally
			{
				LoadGameSessions();
				UpdateUnsyncedCounter();
			}
		}

		private async void ProcessStopped(object sender, EventArrivedEventArgs e)
		{
			string processName = e.NewEvent.Properties["ProcessName"].Value.ToString();
			int processId = Convert.ToInt32(e.NewEvent.Properties["ProcessID"].Value);
			if (!_gameSessionMap.TryRemove(processId, out var gameSessionToEnd))
				return;

			_processIdToNameMap.TryRemove(processId, out _);
			gameSessionToEnd.EndTime = DateTime.UtcNow;
			_gameSessionsRepository.EndGameSession(gameSessionToEnd);
			try
			{
				string url = _environmentVariableService.ApiBaseUrl + $"game-sessions/{gameSessionToEnd.Id}";
				HttpResponseMessage response = await _httpService.SendAndRepeatAuthorization(() => SendGameSessionAsync(gameSessionToEnd));
				_gameSessionsRepository.UpdateSync(gameSessionToEnd.Id, response.IsSuccessStatusCode);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error in ProcessStopped: {ex.Message}");
			}
			finally
			{
				LoadGameSessions();
				UpdateUnsyncedCounter();
			}
		}

		private void CleanupWatchers()
		{
			if (_startWatch != null)
			{
				_startWatch.EventArrived -= ProcessStarted;
				_startWatch.Stop();
				_startWatch.Dispose();
				_startWatch = null;
			}

			if (_stopWatch != null)
			{
				_stopWatch.EventArrived -= ProcessStopped;
				_stopWatch.Stop();
				_stopWatch.Dispose();
				_stopWatch = null;
			}

			isMonitoring = false;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!disposed)
			{
				if (disposing)
				{
					CleanupWatchers();
				}
				disposed = true;
			}
		}

		~GameSessionMonitoringService()
		{
			Dispose(false);
		}

		public void LoadGameSessions()
		{
			
			List<GameSessionsDTO> gameSessionsDTOs = _gameSessionsRepository.GetGameSessionsByUserId(_credentialManagerService.GetUserId(CredentialManagerService.CredentialType.REFRESH));

			GameSessions = new List<GameSessionsDTO>(gameSessionsDTOs);
		}

		public void LoadExeMap()
		{
			List<UserGameDTO> userGames = _userGameRepository.GetUserGames(_credentialManagerService.GetUserId(CredentialManagerService.CredentialType.REFRESH));
			
			_gameMap.Clear();
			foreach(var userGame in userGames)
			{
				_gameMap.TryAdd(userGame.Exe, userGame.GameId);
			}
		}

		public void ClearMapsAndSessions()
		{
			_gameMap.Clear();
			_gameSessionMap.Clear();
			_gameSessions.Clear();
		}

		public void MapGame(string exe, int gameId)
		{
			_gameMap.TryAdd(exe, gameId);
		}

		public void UpdateUnsyncedCounter()
		{
			var sqliteDataAccess = new SqliteDataAccess();
			var sqliteConnectionFactory = new SqliteConnectionFactory(sqliteDataAccess.LoadConnectionString());
			string userId = _credentialManagerService.GetUserId(CredentialManagerService.CredentialType.REFRESH);
			GameSessionsRepository gameSessionRepository = new GameSessionsRepository(sqliteConnectionFactory);
			UserGameRepository userGameRepository = new UserGameRepository(sqliteConnectionFactory);
			NumberUnsynced = gameSessionRepository.GetUnsyncedGameSessionsByUserId(userId).Count()
								+ userGameRepository.GetUnsyncedUserGamesByUserId(userId).Count();
		}

	}
}
