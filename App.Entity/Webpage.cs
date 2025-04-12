using System;
using System.Collections.Generic;

namespace App.Entity
{
    public partial class Webpage
    {
        public Webpage()
        {
            InverseParentWebpage = new HashSet<Webpage>();
            Products = new HashSet<Product>();
            UserGroupPermissions = new HashSet<UserGroupPermission>();
        }

        public int WebpageId { get; set; }
        public int? ParentWebpageId { get; set; }
        public int AppIconId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string Url { get; set; } = null!;
        public string? UiUrl { get; set; } = null!;

        public virtual AppIcon? AppIcon { get; set; } = null!;
        public virtual Webpage? ParentWebpage { get; set; }
        public virtual ICollection<Webpage>? InverseParentWebpage { get; set; }
        public virtual ICollection<Product>? Products { get; set; }
        public virtual ICollection<UserGroupPermission>? UserGroupPermissions { get; set; }
    }
}
