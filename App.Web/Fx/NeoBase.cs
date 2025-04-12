using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.Web.Fx
{
    public interface NeoBase
    {
        WebHelper.SessionHelper SessionHelper { get; }
        void InitializeSessionHelper(HttpContextAccessor httpContext);
    }
}