using System;
using System.Collections.Generic;

namespace DBScoffolding;

public partial class Appicon
{
    public int AppIconId { get; set; }

    public string Name { get; set; } = null!;

    public string? CssClass { get; set; }

    public string? IconColor { get; set; }

    public virtual ICollection<Demotab> Demotabs { get; set; } = new List<Demotab>();

    public virtual ICollection<Menucategory> Menucategories { get; set; } = new List<Menucategory>();

    public virtual ICollection<Webpage> Webpages { get; set; } = new List<Webpage>();
}
