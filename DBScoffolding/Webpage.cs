using System;
using System.Collections.Generic;

namespace DBScoffolding;

public partial class Webpage
{
    public int WebpageId { get; set; }

    public int? ParentWebpageId { get; set; }

    public int AppIconId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string Url { get; set; } = null!;

    public string? UiUrl { get; set; }

    public virtual Appicon AppIcon { get; set; } = null!;

    public virtual ICollection<Webpage> InverseParentWebpage { get; set; } = new List<Webpage>();

    public virtual Webpage? ParentWebpage { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual ICollection<Usergrouppermission> Usergrouppermissions { get; set; } = new List<Usergrouppermission>();
}
