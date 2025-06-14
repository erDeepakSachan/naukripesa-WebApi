using System;
using System.Collections.Generic;

namespace App.Entity;

public partial class Jobdetail
{
    public int JobDetailId { get; set; }

    public int CompanyId { get; set; }

    public int JobLocationId { get; set; }

    public DateTime? InterviewDate { get; set; }

    public string? InterviewTime { get; set; }

    public string? InterviewLocation { get; set; }

    public string? Qualification { get; set; }

    public string? ContactNumber { get; set; }

    public string? Department { get; set; }

    public string? OtherDetail { get; set; }

    public DateTime? CreatedOn { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public int? ModifiedBy { get; set; }

    public virtual Company? Company { get; set; } = null!;

    public virtual Joblocation? JobLocation { get; set; } = null!;
}
