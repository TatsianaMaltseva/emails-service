using AutoMapper;
using email_app_api.Core;
using email_app_api.Models;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace email_app_api.Services
{
    public class TaskService
    {
        private readonly string connectionString;
        private readonly IMapper mapper;

        public TaskService(IMapper mapper, IOptions<EmailAppDbOptions> dbOptions)
        {
            this.mapper = mapper;
            connectionString = dbOptions.Value.ConnectionString;
        }

        public Task AddTask(int userId, Task task)
        {
            string sqlExpression = $"INSERT INTO Tasks " +
                $"(UserId, Name, Description, Cron, Topic, StartDate, Option) " +
                $"VALUES(\"{userId}\", " +
                    $"\"{task.Name}\", " +
                    $"\"{task.Description}\", " +
                    $"\"{task.Cron}\", " +
                    $"\"{task.Topic}\", " +
                    $"\"{task.StartDate}\", " +
                    $"\"{task.Option}\"); " +
                $"SELECT MAX(Id) FROM Tasks";
            using var connection = new SqliteConnection(connectionString);
            connection.Open();
            SqliteCommand command = new SqliteCommand(sqlExpression, connection);
            TaskEntity taskEntity = mapper.Map<TaskEntity>(task);
            taskEntity.Id = Convert.ToInt32(command.ExecuteScalar());
            if (taskEntity.Id != 0)
            {
                return mapper.Map<Task>(taskEntity);
            }
            return null;
        }

        public List<Task> GetTasks(int userId)
        {
            string sqlExpression = $"SELECT * FROM Tasks WHERE UserId = \"{userId}\"";
            using var connection = new SqliteConnection(connectionString);
            List<Task> tasks = new List<Task>();
            connection.Open();
            SqliteCommand command = new SqliteCommand(sqlExpression, connection);
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        TaskEntity task = GetTaskFromReader(reader);
                        tasks.Add(mapper.Map<Task>(task));
                    }
                }
            }
            return tasks;
        }

        public Task EditTask(int taskId, Task task)
        {
            string sqlExpression = $"UPDATE Tasks " +
                $"SET Name = \"{task.Name}\", " +
                $"Description = \"{task.Description}\", " +
                $"Cron = \"{task.Cron}\", " +
                $"Topic = \"{task.Topic}\", " +
                $"StartDate = \"{task.StartDate}\", " +
                $"Option = \"{task.Option}\" " +
                $"WHERE Id = \"{taskId}\"";
            using var connection = new SqliteConnection(connectionString);
            connection.Open();
            SqliteCommand command = new SqliteCommand(sqlExpression, connection);
            bool ifWasUpdated = command.ExecuteNonQuery() != 0;
            if (ifWasUpdated)
            {
                return task;
            }
            return null;
        }

        public void UpdateLastExecutedDate(int taskId, DateTime newDateTime)
        {
            string sqlExpression = $"UPDATE Tasks " +
                $"SET LastExecuted = \"{newDateTime}\" " +
                $"WHERE Id = \"{taskId}\"";
            using var connection = new SqliteConnection(connectionString);
            connection.Open();
            SqliteCommand command = new SqliteCommand(sqlExpression, connection);
            command.ExecuteNonQuery();
        }

        public List<Task> GetTasks()
        {
            string sqlExpression = $"SELECT * FROM Tasks";
            using var connection = new SqliteConnection(connectionString);
            List<Task> tasks = new List<Task>();
            connection.Open();
            SqliteCommand command = new SqliteCommand(sqlExpression, connection);
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        TaskEntity task = GetTaskFromReader(reader);
                        tasks.Add(mapper.Map<Task>(task));
                    }
                }
            }
            return tasks;
        }

        public bool DeleteTask(int taskId)
        {
            string sqlExpression = $"DELETE FROM Tasks WHERE Id = \"{taskId}\"";
            using var connection = new SqliteConnection(connectionString);
            connection.Open();
            SqliteCommand command = new SqliteCommand(sqlExpression, connection);
            bool ifWasDeleted = command.ExecuteNonQuery() != 0;
            return ifWasDeleted;
        }

        private TaskEntity GetTaskFromReader (SqliteDataReader reader)
        {
            return new TaskEntity()
            {
                Id = reader.GetInt32(0),
                UserId = reader.GetInt32(1),
                Name = reader.GetString(2),
                Description = reader.GetString(3),
                Cron = reader.GetString(4),
                Topic = (Topic)reader.GetInt32(5),
                StartDate = reader.GetDateTime(6),
                LastExecuted = !reader.IsDBNull(7) ? reader.GetDateTime(7) : null,
                Option = !reader.IsDBNull(8) ? reader.GetString(8) : null
            };
        }
    }
}
