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

        public List<User> GetUsers(int currerntUserId)
        {
            UserEntity user = GetUser(currerntUserId);
            List<UserEntity> userEntities = GetUsers();
            return mapper.Map<List<UserEntity>, List<User>>(userEntities);
        }

        public UserEntity GetUser(int userId)
        {
            string sqlExpression = $"SELECT * FROM Users WHERE Id = \"{userId}\"";
            using var connection = new SqliteConnection(connectionString);
            connection.Open();
            SqliteCommand command = new SqliteCommand(sqlExpression, connection);
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    UserEntity user = GetUserFromReader(reader);

                }
            }
            return null;
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

        public User GetUser(string email, string password)
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
                    UserEntity userEntity = GetUserFromReader(reader);
                    return mapper.Map<User>(userEntity);
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
