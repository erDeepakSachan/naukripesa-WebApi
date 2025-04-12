using System;
using System.Collections.Generic;

namespace App.Entity
{
    public partial class User
    {
        public User()
        {
            AccessActivities = new HashSet<AccessActivity>();
            DemoTabs = new HashSet<DemoTab>();
            UserSessions = new HashSet<UserSession>();
        }

        public int UserId { get; set; }
        public int UserTypeId { get; set; }
        public int UserGroupId { get; set; }
        public int CompanyId { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string MobileNo { get; set; } = null!;
        public DateTime? RecentLogin { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public bool IsArchived { get; set; }
        public string? Otp { get; set; }

        public virtual Company? Company { get; set; } = null!;
        public virtual UserGroup? UserGroup { get; set; } = null!;
        public virtual UserType? UserType { get; set; } = null!;
        public virtual ICollection<AccessActivity>? AccessActivities { get; set; }
        public virtual ICollection<DemoTab>? DemoTabs { get; set; }
        public virtual ICollection<UserSession>? UserSessions { get; set; }
    }
}
