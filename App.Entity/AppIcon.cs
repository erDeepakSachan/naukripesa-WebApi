using System;
using System.Collections.Generic;

namespace App.Entity
{
    public partial class AppIcon
    {
        public AppIcon()
        {
            DemoTabs = new HashSet<DemoTab>();
            MenuCategories = new HashSet<MenuCategory>();
            Webpages = new HashSet<Webpage>();
        }

        public int AppIconId { get; set; }
        public string Name { get; set; } = null!;
        public string? CssClass { get; set; }
        public string? IconColor { get; set; }

        public virtual ICollection<DemoTab>? DemoTabs { get; set; }
        public virtual ICollection<MenuCategory>? MenuCategories { get; set; }
        public virtual ICollection<Webpage>? Webpages { get; set; }
    }
}
