using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeQuestLogDesktopApp.Database;
using TimeQuestLogDesktopApp.Models;

namespace TimeQuestLogDesktopApp.Repositories
{
	internal class UserRepository : SqliteDataAccess
	{
		private readonly IDbConnectionFactory _connectionFactory;
        public UserRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        public List<Users> LoadUsers()
		{
			using (var cnn = _connectionFactory.CreateConnection())
			{
				var output = cnn.Query<Users>("select * from Users", new DynamicParameters());
				return output.ToList();
			}
		}

		public void SaveUsers(Users user)
		{
			string sql = "INSERT INTO Users (Id, Username) VALUES (@Id, @Username)";
			var parameters = new { user.Id, user.Username };

			using (var cnn = _connectionFactory.CreateConnection())
			{
				int rowsAffected = cnn.Execute(sql, parameters);
			}
		}

		public Users? GetUserById(string id)
		{
			string sql = @"SELECT * FROM Users WHERE Id = @Id";
			var parameters = new {Id = id};

			using (var cnn = _connectionFactory.CreateConnection())
			{
				return cnn.QuerySingleOrDefault<Users>(sql, parameters);
			}
		}
	}
}
