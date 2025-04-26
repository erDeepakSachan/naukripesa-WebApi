using System;
using System.Collections.Generic;

namespace DBScoffolding;

public partial class Currency
{
    public int CurrencyId { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public int ModifiedBy { get; set; }

    public DateTime ModifiedOn { get; set; }

    public virtual ICollection<Company> Companies { get; set; } = new List<Company>();
}
