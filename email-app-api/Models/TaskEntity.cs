using email_app_api.Core;
using System;

namespace email_app_api.Models
{
    public class TaskEntity
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Cron { get; set; }

        public Topic Topic { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? LastExecuted { get; set; }

        public string Option { get; set; }
    }
}
