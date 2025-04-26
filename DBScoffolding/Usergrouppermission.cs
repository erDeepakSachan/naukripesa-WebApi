using System;
using System.Collections.Generic;

namespace DBScoffolding;

public partial class Usergrouppermission
{
    public int UserGroupPermissionId { get; set; }

    public int UserGroupId { get; set; }

    public int WebpageId { get; set; }

    public int MenuCategoryId { get; set; }

    public bool IsVisible { get; set; }

    public virtual Menucategory MenuCategory { get; set; } = null!;

    public virtual Usergroup UserGroup { get; set; } = null!;

    public virtual Webpage Webpage { get; set; } = null!;
}
