using System;
using System.Collections.Generic;

namespace App.Entity;

public partial class contactus
{
    public int ID { get; set; }

    public string Name { get; set; }

    public string PhoneNo { get; set; }

    public string Email{ get; set; }

    public string Message { get; set; }
}
