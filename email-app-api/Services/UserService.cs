using AutoMapper;
using email_app_api.Models;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace email_app_api.Services
{
    public class UserService
    {
        private readonly string connectionString;
        private readonly IMapper mapper;

        public UserService(IMapper mapper, IOptions<EmailAppDbOptions> dbOptions)
        {
            this.mapper = mapper;
            connectionString = dbOptions.Value.ConnectionString;
        }

        public LoginResponse Login(LoginRequest loginRequest)
        {
            UserEntity user = GetUser(loginRequest.Email, loginRequest.Password);
            if (user != null)
            {
                return mapper.Map<LoginResponse>(user);
            }
            return null;
        }

        public List<User> GetUsers(int currerntUserId)
        {
            UserEntity user = GetUser(currerntUserId);
            if (user.Role != "Admin")
            {
                return null;
            }
            List<UserEntity> userEntities = GetUsers();
            return mapper.Map<List<UserEntity>, List<User>>(userEntities);
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
                        UserEntity user = GetUserFromReader(reader);
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
                    return GetUserFromReader(reader);
                }
            }
            return null;
        }

        private UserEntity GetUser(int currerntUserId)
        {
            string sqlExpression = $"SELECT * FROM Users WHERE Id = \"{currerntUserId}\"";
            using var connection = new SqliteConnection(connectionString);
            connection.Open();
            SqliteCommand command = new SqliteCommand(sqlExpression, connection);
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return GetUserFromReader(reader);
                }
            }
            return null;
        }

        private UserEntity GetUserFromReader (SqliteDataReader reader)
        {
            return new UserEntity()
            {
                Id = reader.GetInt32(0),
                Email = reader.GetString(1),
                Password = reader.GetString(2),
                Role = reader.GetString(3)
            };
        }
    }
}
