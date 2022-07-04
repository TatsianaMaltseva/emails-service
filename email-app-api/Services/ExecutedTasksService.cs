using AutoMapper;
using email_app_api.Models;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace email_app_api.Services
{
    public class ExecutedTasksService
    {
        private readonly string connectionString;

        public ExecutedTasksService(IOptions<EmailAppDbOptions> dbOptions)
        {
            connectionString = dbOptions.Value.ConnectionString;
        }

        public ExecutedTask AddExecutedTask(int userId, Models.Task task, DateTime executed)
        {
            string sqlExpression = $"INSERT INTO ExecutedTasks " +
                $"(UserId, TaskId, Executed) " +
                $"VALUES (\"{userId}\", \"{task.Id}\", \"{executed}\"); " +
                $"SELECT MAX(Id) FROM ExecutedTasks";
            using var connection = new SqliteConnection(connectionString);
            connection.Open();
            SqliteCommand command = new SqliteCommand(sqlExpression, connection);
            int executedTaskId = Convert.ToInt32(command.ExecuteScalar());
            if (executedTaskId != 0)
            {
                return new ExecutedTask()
                {
                    Name = task.Name,
                    Executed = executed,
                    StartDate = task.StartDate,
                    LastExecuted = task.LastExecuted ?? executed,
                };
            }
            return null;
        }
        
        public List<ExecutedTask> GetExecutedTasksForUser(int userId)
        {
            string sqlExpression = $"SELECT t.Name, et.Executed, t.StartDate, t.LastExecuted " +
                $"FROM ExecutedTasks as et " +
                $"INNER JOIN Tasks as t " +
                $"ON et.TaskId = t.Id " +
                $"WHERE et.UserId = \"{userId}\"" +
                $"LIMIT 10; ";
            using var connection = new SqliteConnection(connectionString);
            List<ExecutedTask> executedTasks = new List<ExecutedTask>();
            connection.Open();
            SqliteCommand command = new SqliteCommand(sqlExpression, connection);
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while(reader.Read())
                    {
                        ExecutedTask executedTask = GetExecutedTaskFromReader(reader);
                        executedTasks.Add(executedTask);
                    }
                }
            }
            return executedTasks;
        }

        private ExecutedTask GetExecutedTaskFromReader (SqliteDataReader reader)
        {
            return new ExecutedTask()
            {
                Name = reader.GetString(0),
                Executed = reader.GetDateTime(1),
                StartDate = reader.GetDateTime(2),
                LastExecuted = reader.GetDateTime(3)
            };
        }
    }
}
