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
		private Dictionary<string, int> _gameMap;
		private Dictionary<string, GameSessions> _gameSessionMap;
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
			_gameSessionMap = new Dictionary<string, GameSessions>();
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

        public static GameSessionMonitoringService GetInstance
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

			Console.ReadLine();
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
			if (_gameMap.ContainsKey(processName))
			{
				int processId = Convert.ToInt32(e.NewEvent.Properties["ProcessID"].Value);
				Console.WriteLine($"Process Started: {processName} (PID: {processId})");

				GameSessions gameSession = _gameSessionsRepository.CreateGameSession(_gameMap.GetValueOrDefault(processName), _credentialManagerService.GetUserId(CredentialManagerService.CredentialType.REFRESH));
				_gameSessionMap.Add(processName, gameSession);
				LoadGameSessions();
			}
		}

		private void ProcessStopped(object sender, EventArrivedEventArgs e)
		{
			string processName = e.NewEvent.Properties["ProcessName"].Value.ToString();
			if (_gameMap.ContainsKey(processName) && _gameSessionMap.ContainsKey(processName))
			{
				int processId = Convert.ToInt32(e.NewEvent.Properties["ProcessID"].Value);
				Console.WriteLine($"Process Stopped: {processName} (PID: {processId})");

				_gameSessionsRepository.EndGameSession(_gameSessionMap.GetValueOrDefault(processName).Id);
				_gameSessionMap.Remove(processName);	
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

	}
}
