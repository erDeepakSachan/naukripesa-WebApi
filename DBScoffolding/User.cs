using System;
using System.Collections.Generic;

namespace DBScoffolding;

public partial class User
{
    public int UserId { get; set; }

    public int UserTypeId { get; set; }

    public int UserGroupId { get; set; }

    public int CompanyId { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string MobileNo { get; set; } = null!;

    public DateTime? RecentLogin { get; set; }

    public DateTime CreatedOn { get; set; }

    public int CreatedBy { get; set; }

    public DateTime ModifiedOn { get; set; }

    public int ModifiedBy { get; set; }

    public bool? IsArchived { get; set; }

    public string? Otp { get; set; }

    public virtual ICollection<Accessactivity> Accessactivities { get; set; } = new List<Accessactivity>();

    public virtual Company Company { get; set; } = null!;

    public virtual ICollection<Demotab> Demotabs { get; set; } = new List<Demotab>();

    public virtual Usergroup UserGroup { get; set; } = null!;

    public virtual Usertype UserType { get; set; } = null!;

    public virtual ICollection<Usersession> Usersessions { get; set; } = new List<Usersession>();
}
