using System;

namespace email_app_api.Models
{
    public class ExecutedTaskEntity
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int TaskId { get; set; }

        public DateTime Executed { get; set; }
    }
}
