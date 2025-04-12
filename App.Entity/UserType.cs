using System;
using System.Collections.Generic;

namespace App.Entity
{
    public partial class UserType
    {
        public UserType()
        {
            Products = new HashSet<Product>();
            Users = new HashSet<User>();
        }

        public int UserTypeId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;

        public virtual ICollection<Product>? Products { get; set; }
        public virtual ICollection<User>? Users { get; set; }
    }
}
