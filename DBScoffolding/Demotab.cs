using System;
using System.Collections.Generic;

namespace DBScoffolding;

public partial class Demotab
{
    public int DemoId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int? UserId { get; set; }

    public int? AppIconId { get; set; }

    public string? Other { get; set; }

    public bool? IsActive { get; set; }

    public virtual Appicon? AppIcon { get; set; }

    public virtual User? User { get; set; }
}
