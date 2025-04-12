using System;
using System.Collections.Generic;

namespace App.Entity
{
    public partial class Setting
    {
        public int SettingId { get; set; }
        public int CompanyId { get; set; }
        public string Name { get; set; } = null!;
        public string Value { get; set; } = null!;
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public int ModifiedBy { get; set; }
        public bool IsArchived { get; set; }

        public virtual Company? Company { get; set; } = null;
    }
}
