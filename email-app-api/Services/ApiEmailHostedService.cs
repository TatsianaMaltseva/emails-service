using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cronos;

namespace email_app_api.Services
{
    public class ApiEmailHostedService : IHostedService, IDisposable
    {
        private Timer _timer = null!;
        private readonly IServiceScopeFactory _scopeFactory;

        public ApiEmailHostedService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));

            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            using var scope = _scopeFactory.CreateScope();
            ApiEmailService apiEmailService = scope.ServiceProvider.GetRequiredService<ApiEmailService>();
            TaskService taskService = scope.ServiceProvider.GetRequiredService<TaskService>();
            UserService userService = scope.ServiceProvider.GetRequiredService<UserService>();
            List<Models.Task> tasks = taskService.GetTasks();

            foreach(Models.Task task in tasks)
            {
                string cron = task.Cron;
                CronExpression expression = CronExpression.Parse(cron);
                DateTimeOffset? next = expression.GetNextOccurrence(DateTimeOffset.Now, TimeZoneInfo.Local);

                DateTime localTimeNow = DateTime.Now;
                DateTime? nextLocalTime = next?.DateTime;

                if ((nextLocalTime - localTimeNow) < TimeSpan.FromMinutes(1))
                {
                    Models.UserEntity user = userService.GetUser(task.UserId);
                    await apiEmailService.SendEmailAsync(user.Email, ApiEmailService.Topics.Weather);
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer.Dispose();
        }
    }
}
