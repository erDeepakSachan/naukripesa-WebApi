using System;
using System.Collections.Generic;

namespace App.Entity
{
    public partial class Currency
    {
        public Currency()
        {
            Companies = new HashSet<Company>();
        }

        public int CurrencyId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }

        public virtual ICollection<Company> Companies { get; set; }
    }
}
