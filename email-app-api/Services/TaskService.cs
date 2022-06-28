using AutoMapper;
using email_app_api.Models;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;
using System;

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
            string sqlExpression = $"INSERT INTO Tasks (UserId, Name, Description, Cron) " +
                $"VALUES(\"{userId}\", \"{task.Name}\", \"{task.Description}\", \"{task.Cron}\");" +
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
    }
}
