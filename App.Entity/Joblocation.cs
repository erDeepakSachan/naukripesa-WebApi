using System;
using System.Collections.Generic;

namespace App.Entity;

public partial class Joblocation
{
    public int JobLocationId { get; set; }

    public string Location { get; set; } = null!;

    public int? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public bool? IsArchived { get; set; }

    public virtual ICollection<Jobdetail> Jobdetails { get; set; } = new List<Jobdetail>();
}
