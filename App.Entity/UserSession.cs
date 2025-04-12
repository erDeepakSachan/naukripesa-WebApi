using System;
using System.Collections.Generic;

namespace App.Entity
{
    public partial class UserSession
    {
        public int UserSessionId { get; set; }
        public int UserId { get; set; }
        public Guid SessionGuid { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool? IsActive { get; set; }
        public int ExpirationTimeFrame { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
