using App.Web.Fx;
using App.Service;
using Microsoft.AspNetCore.Mvc;

namespace App.Web.Controllers
{
    public class LeftMenuController : NeoController
    {
        private readonly CommonService commonService;
        private readonly UserGroupPermissionService userGroupPermissionService;
        private readonly UserSessionService userSessionService;
        private readonly UserService userService;

        public LeftMenuController(CommonService commonService
            , UserGroupPermissionService userGroupPermissionService
            , UserSessionService userSessionService
            , UserService userService
            , WebHelper webHelper, WebHelper.SessionHelper sessionHelper) :
            base(webHelper, sessionHelper)
        {
            this.commonService = commonService;
            this.userGroupPermissionService = userGroupPermissionService;
            this.userSessionService = userSessionService;
            this.userService = userService;
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> LeftMenu()
        {
            var auth = this.User.FindFirst(NeoAuthToken.AuthTokenKey)?.Value;
            var session = HttpContext.RequestServices.GetService<NeoAuthorization>().ParseAuthToken(auth);
            var sid = session.SID.ToString();
            var guid = new Guid(sid);
            var userGroupId = 0;
            var loadedPermissions = LoadPermissions(guid, out userGroupId);
            MenuNode menuTree = BuildMenu(loadedPermissions, userGroupId);
            return NeoData(menuTree);
        }

        private List<dynamic> LoadPermissions(Guid sid, out int userGroupId)
        {
            var userSession = userSessionService.GetByUserSessionGuid(sid);
            var user = userService.Get(userSession.UserId).Result;
            userGroupId = user.UserGroupId;
            return userGroupPermissionService.GetByUserGroup(user.UserGroupId);
        }

        private MenuNode BuildMenu(List<dynamic> UserPermissions, int userGroupId)
        {
            MenuNode globalRootNode = new MenuNode();

            if (UserPermissions.Count > 0)
            {
                var dsMenuCategory = userGroupPermissionService.GetDistinctMenuCategory(userGroupId);
                if (dsMenuCategory.Count > 0)
                {
                    for (Int32 i = 0; i <= dsMenuCategory.Count - 1; i++)
                    {
                        MenuNode tn = new MenuNode();
                        tn.Text = "" + dsMenuCategory[i].Name;
                        tn.IconCss = Convert.ToString(dsMenuCategory[i].AppIconCss);
                        tn.IconColor = Convert.ToString(dsMenuCategory[i].AppIconColor);
                        dynamic row;

                        for (Int32 j = 0; j <= UserPermissions.Count - 1; j++)
                        {
                            row = UserPermissions[j];
                            if (Convert.ToInt32(row.MenuCategoryMenuCategoryID) ==
                                Convert.ToInt32(dsMenuCategory[i].MenuCategoryID)
                                && row.WebpageParentWebpageID == null)
                            {
                                if ((bool)row.IsVisible == true)
                                {
                                    MenuNode n = new MenuNode();
                                    n.Text = commonService.BeauitfyWord(Convert.ToString(row.WebpageName));
                                    n.Icon = Convert.ToString(row.AppIconName);
                                    n.IconCss = Convert.ToString(row.AppIconCss);
                                    n.IconColor = Convert.ToString(row.AppIconColor);
                                    n.ControllerName = Convert.ToString(row.WebpageURL).Replace(".aspx", "");
                                    n.NavUrl = Convert.ToString(row.PageURL).Replace(".aspx", "");
                                    AddChildNodes(
                                        userGroupPermissionService.GetPermissionByParentWebpage(userGroupId,
                                            Convert.ToInt32(row.WebpageID)), n);
                                    tn.Nodes.Add(n);
                                }
                            }
                        }

                        globalRootNode.Nodes.Add(tn);
                    }
                }
            }

            return globalRootNode;
        }


        private void AddChildNodes(List<dynamic> UserPermissions, MenuNode Node)
        {
            dynamic CurrentRow;
            for (Int32 k = 0; k <= UserPermissions.Count - 1; k++)
            {
                if (k == 0)
                {
                    var nodeJson = Newtonsoft.Json.JsonConvert.SerializeObject(Node);
                    var nodeClone = Newtonsoft.Json.JsonConvert.DeserializeObject<MenuNode>(nodeJson);
                    Node.Nodes.Add(nodeClone);
                }

                CurrentRow = UserPermissions[k];
                MenuNode childnode = new MenuNode();
                if ((bool)CurrentRow.IsVisible == true)
                {
                    {
                        childnode.Text = commonService.BeauitfyWord(Convert.ToString(CurrentRow.WebpageName));
                        childnode.Icon = Convert.ToString(CurrentRow.AppIconName);
                        childnode.IconCss = Convert.ToString(CurrentRow.AppIconCss);
                        childnode.IconColor = Convert.ToString(CurrentRow.AppIconColor);
                        childnode.ControllerName = Convert.ToString(CurrentRow.WebpageURL).Replace(".aspx", "");
                        childnode.NavUrl = Convert.ToString(CurrentRow.PageURL).Replace(".aspx", "");
                    }
                    Node.Nodes.Add(childnode);
                }

                AddChildNodes(
                    userGroupPermissionService.GetPermissionByParentWebpage(
                        Convert.ToInt32(CurrentRow.UserGroupUserGroupID),
                        Convert.ToInt32(CurrentRow.WebpageID)), childnode);
            }
        }
    }
}