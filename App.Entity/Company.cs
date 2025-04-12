using System;
using System.Collections.Generic;

namespace App.Entity
{
    public partial class Company
    {
        public Company()
        {
            Settings = new HashSet<Setting>();
            Users = new HashSet<User>();
        }

        public int CompanyId { get; set; }
        public int CurrencyId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }

        public virtual Currency Currency { get; set; } = null!;
        public virtual ICollection<Setting> Settings { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
