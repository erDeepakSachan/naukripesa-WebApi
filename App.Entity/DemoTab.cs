using System;
using System.Collections.Generic;

namespace App.Entity
{
    public partial class DemoTab
    {
        public int DemoId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int? UserId { get; set; }
        public int? AppIconId { get; set; }
        public string? Other { get; set; }
        public bool? IsActive { get; set; }

        public virtual AppIcon? AppIcon { get; set; }
        public virtual User? User { get; set; }
    }
}
