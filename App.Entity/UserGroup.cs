using System;
using System.Collections.Generic;

namespace App.Entity
{
    public partial class UserGroup
    {
        public UserGroup()
        {
            UserGroupPermissions = new HashSet<UserGroupPermission>();
            Users = new HashSet<User>();
        }

        public int UserGroupId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        public virtual ICollection<UserGroupPermission>? UserGroupPermissions { get; set; }
        public virtual ICollection<User>? Users { get; set; }
    }
}
