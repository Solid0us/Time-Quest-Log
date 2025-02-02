using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using TimeQuestLogDesktopApp.Database;
using TimeQuestLogDesktopApp.Models;
using TimeQuestLogDesktopApp.Models.DTOs;
using TimeQuestLogDesktopApp.Repositories;

namespace TimeQuestLogDesktopApp.Services
{
    class GameSessionMonitoringService : IDisposable
    {
		private static GameSessionMonitoringService _instance;
		private static readonly object lockObject = new object();
		private ManagementEventWatcher _startWatch;
        private ManagementEventWatcher _stopWatch;
		private bool isMonitoring;
		private bool disposed;
		private Dictionary<int, string> _processIdToNameMap = new Dictionary<int, string>();
		private Dictionary<string, int> _gameMap;
		private Dictionary<int, GameSessions> _gameSessionMap;
		private List<GameSessionsDTO> _gameSessions = new List<GameSessionsDTO>();
		private GameSessionsRepository _gameSessionsRepository;
		private UserGameRepository _userGameRepository;
		private readonly CredentialManagerService _credentialManagerService;

		public event EventHandler<List<GameSessionsDTO>> GameSessionsChanged;
		private GameSessionMonitoringService()
        {
			var sqliteDataAccess = new SqliteDataAccess();
			var sqliteConnectionFactory = new SqliteConnectionFactory(sqliteDataAccess.LoadConnectionString());
			_gameSessionsRepository = new GameSessionsRepository(sqliteConnectionFactory);
			_userGameRepository = new UserGameRepository(sqliteConnectionFactory);

			_gameMap = new Dictionary<string, int>();
			_gameSessionMap = new Dictionary<int, GameSessions>();
			_credentialManagerService = CredentialManagerService.GetInstance();
			_credentialManagerService.LoadCredentials();
			LoadGameSessions();
			LoadExeMap();	
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

        public static GameSessionMonitoringService Instance
		{
			get
			{
				if (_instance == null)
				{
					lock (lockObject)
					{
						_instance ??= new GameSessionMonitoringService();
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
		}

		public void StopMonitoring()
		{
			if (!isMonitoring)
				return;

			CleanupWatchers();
		}

		private void ProcessStarted(object sender, EventArrivedEventArgs e)
		{
			string processName = e.NewEvent.Properties["ProcessName"].Value.ToString();
			int processId = Convert.ToInt32(e.NewEvent.Properties["ProcessID"].Value);
			if (Debugger.IsAttached)
			{
				Console.WriteLine($"Process Started: {processName} (PID: {processId})");
			}
			if (_gameMap.ContainsKey(processName) && !_gameSessionMap.ContainsKey(processId))
			{
				_processIdToNameMap[processId] = processName;
				GameSessions gameSession = _gameSessionsRepository.CreateGameSession(_gameMap.GetValueOrDefault(processName), _credentialManagerService.GetUserId(CredentialManagerService.CredentialType.REFRESH));
				_gameSessionMap.Add(processId, gameSession);
				LoadGameSessions();
			}
		}

		private void ProcessStopped(object sender, EventArrivedEventArgs e)
		{
			
			string processName = e.NewEvent.Properties["ProcessName"].Value.ToString();
			int processId = Convert.ToInt32(e.NewEvent.Properties["ProcessID"].Value);
			if (Debugger.IsAttached)
			{
				Console.WriteLine($"Process Stopped: {processName} (PID: {processId})");
			}
			if (_gameSessionMap.ContainsKey(processId))
			{
				
				_gameSessionsRepository.EndGameSession(_gameSessionMap.GetValueOrDefault(processId).Id);
				_gameSessionMap.Remove(processId);
				_processIdToNameMap.Remove(processId);
				LoadGameSessions();
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
				_gameMap.Add(userGame.Exe, userGame.GameId);
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
			_gameMap.Add(exe, gameId);
		}

	}
}
