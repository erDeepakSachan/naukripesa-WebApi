using System;
using System.Collections.Generic;

namespace DBScoffolding;

public partial class Accessactivity
{
    public int AccessActivityId { get; set; }

    public int UserId { get; set; }

    public int UserSessionId { get; set; }

    public string ActivityType { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public virtual User User { get; set; } = null!;
}
