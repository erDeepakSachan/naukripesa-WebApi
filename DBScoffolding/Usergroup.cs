using System;
using System.Collections.Generic;

namespace DBScoffolding;

public partial class Usergroup
{
    public int UserGroupId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Usergrouppermission> Usergrouppermissions { get; set; } = new List<Usergrouppermission>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
