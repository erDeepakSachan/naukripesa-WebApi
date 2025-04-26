using System;
using System.Collections.Generic;

namespace DBScoffolding;

public partial class Usertype
{
    public int UserTypeId { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
