using email_app_api.Models;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;

namespace email_app_api.Services
{
    public class UserService
    {
        string connectionString = "Data Source=C:/Users/Msltzeva Tatiana/AppData/Local/Temp/Rar$EXa0.224/SQLiteStudio/fileData.db";
        //options
        public LoginResponse Login(LoginRequest loginRequest)
        {
            UserEntity user = GetUser(loginRequest.Email, loginRequest.Password);
            if (user != null)
            {
                return new LoginResponse()
                {
                    Id = user.Id,
                    Role = user.Role
                };
            }
            return null;
        }

        public List<UserEntity> GetUsers(int userId)
        {
            UserEntity user = GetUser(userId);
            if (user.Role != "Admin")
            {
                return new List<UserEntity>();
            }
            return GetUsers();
        }

        private List<UserEntity> GetUsers()
        {
            string sqlExpression = $"SELECT * FROM Users";
            using var connection = new SqliteConnection(connectionString);
            List<UserEntity> userEntities = new List<UserEntity>();
            connection.Open();
            SqliteCommand command = new SqliteCommand(sqlExpression, connection);
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while(reader.Read())
                    {
                        UserEntity user = new UserEntity()
                        {
                            Id = reader.GetInt32(0),
                            Email = reader.GetString(1),
                            Password = reader.GetString(2),
                            Role = reader.GetString(3)
                        };
                        userEntities.Add(user);
                    }
                }
            }
            return userEntities;
        }

        private UserEntity GetUser(string email, string password)
        {
            string sqlExpression = $"SELECT * FROM Users WHERE Email = \"{email}\" AND Password = \"{password}\"";
            using var connection = new SqliteConnection(connectionString);
            connection.Open();
            SqliteCommand command = new SqliteCommand(sqlExpression, connection);
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    UserEntity user = new UserEntity()
                    {
                        Id = reader.GetInt32(0),
                        Email = reader.GetString(1),
                        Password = reader.GetString(2),
                        Role = reader.GetString(3)
                    };
                    return user;
                }
            }
            return null;
        }

        private UserEntity GetUser(int id)
        {
            string sqlExpression = $"SELECT * FROM Users WHERE Id = \"{id}\"";
            using var connection = new SqliteConnection(connectionString);
            connection.Open();
            SqliteCommand command = new SqliteCommand(sqlExpression, connection);
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    UserEntity user = new UserEntity()
                    {
                        Id = reader.GetInt32(0),
                        Email = reader.GetString(1),
                        Password = reader.GetString(2),
                        Role = reader.GetString(3)
                    };
                    return user;
                }
            }
            return null;
        }
    }
}
