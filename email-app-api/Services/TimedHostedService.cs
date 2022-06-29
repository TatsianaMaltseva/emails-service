using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace email_app_api.Services
{
    public class TimedHostedService : IHostedService, IDisposable
    {
        private Timer _timer = null!;
        private readonly IServiceScopeFactory _scopeFactory;

        public TimedHostedService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));

            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            using var scope = _scopeFactory.CreateScope();
            ApiEmailService apiEmailService = scope.ServiceProvider.GetRequiredService<ApiEmailService>();
            TaskService taskService = scope.ServiceProvider.GetRequiredService<TaskService>();
            List<Models.Task> tasks = taskService.GetTasks();
            await apiEmailService.SendEmailAsync("tanjamaltzevatanja@gmail.com", ApiEmailService.Topics.Weather);
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
