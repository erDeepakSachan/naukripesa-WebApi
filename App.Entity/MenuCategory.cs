using System;
using System.Collections.Generic;

namespace App.Entity
{
    public partial class MenuCategory
    {
        public MenuCategory()
        {
            UserGroupPermissions = new HashSet<UserGroupPermission>();
        }

        public int MenuCategoryId { get; set; }
        public int AppIconId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int MenuOrder { get; set; }

        public virtual AppIcon? AppIcon { get; set; } = null!;
        public virtual ICollection<UserGroupPermission>? UserGroupPermissions { get; set; }
    }
}
