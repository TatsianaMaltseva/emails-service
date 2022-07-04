using System;

namespace email_app_api.Models
{
    public class ExecutedTask
    {
        public string Name { get; set; }

        public DateTime Executed { get; set; }
    
        public DateTime StartDate { get; set; }

        public DateTime LastExecuted { get; set; }
    }
}

