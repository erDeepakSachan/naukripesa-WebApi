using System;
using System.Collections.Generic;

namespace App.Entity
{
    public partial class ErrorActivity
    {
        public long ErrorActivityId { get; set; }
        public string Title { get; set; } = null!;
        public string ErrorMessage { get; set; } = null!;
        public string StackTraceMessage { get; set; } = null!;
        public DateTime ErrorDateTime { get; set; }
        public int UserId { get; set; }
    }
}
