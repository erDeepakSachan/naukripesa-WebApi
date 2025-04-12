using System;
using System.Collections.Generic;

namespace App.Entity
{
    public partial class Product
    {
        public int ProductId { get; set; }
        public string Pname { get; set; }
        public string? Description { get; set; }
        public int? WebPageId { get; set; }
        public int? UserTypeId { get; set; }
        public bool IsActive { get; set; }

        public virtual UserType? UserType { get; set; }
        public virtual Webpage? WebPage { get; set; }
    }
}
