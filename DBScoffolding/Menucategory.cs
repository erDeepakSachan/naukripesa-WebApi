using System;
using System.Collections.Generic;

namespace DBScoffolding;

public partial class Menucategory
{
    public int MenuCategoryId { get; set; }

    public int AppIconId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int MenuOrder { get; set; }

    public virtual Appicon AppIcon { get; set; } = null!;

    public virtual ICollection<Usergrouppermission> Usergrouppermissions { get; set; } = new List<Usergrouppermission>();
}
