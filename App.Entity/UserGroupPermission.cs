using System;
using System.Collections.Generic;

namespace App.Entity
{
    public partial class UserGroupPermission
    {
        public int UserGroupPermissionId { get; set; }
        public int UserGroupId { get; set; }
        public int WebpageId { get; set; }
        public int MenuCategoryId { get; set; }
        public bool IsVisible { get; set; }

        public virtual MenuCategory? MenuCategory { get; set; } = null!;
        public virtual UserGroup? UserGroup { get; set; } = null!;
        public virtual Webpage? Webpage { get; set; } = null!;
    }
}
