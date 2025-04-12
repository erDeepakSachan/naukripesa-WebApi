using App.Entity;
using Dapper;
using Microsoft.EntityFrameworkCore;

namespace App.Repository;

public class UserGroupPermissionRepository : GenericRepository<UserGroupPermission>
{
    public UserGroupPermissionRepository(AppDbContext db) : base(db)
    {
    }

    public List<dynamic> GetByUserGroup(int userGroupId)
    {
        var sql = @"
            SELECT
            a0.*
            ,a1.WebpageID AS WebpageWebpageID
            ,a1.ParentWebpageID AS WebpageParentWebpageID
            ,a1.Name AS WebpageName
            ,a1.Description AS WebpageDescription
            ,a1.URL AS WebpageURL
            ,a1.UiURL AS PageURL
            ,a2.UserGroupID AS UserGroupUserGroupID
            ,a2.Name AS UserGroupName
            ,a2.Description AS UserGroupDescription
            ,a3.MenuCategoryID AS MenuCategoryMenuCategoryID
            ,a3.Name AS MenuCategoryName
            ,a3.MenuOrder as MenuOrder
            ,a3.Description AS MenuCategoryDescription
            ,a4.Name AS AppIconName
            ,a4.CssClass AS AppIconCss
            ,a4.IconColor AS AppIconColor
            FROM UserGroupPermission a0
            LEFT JOIN Webpage a1 on a1.WebpageID =  a0.WebpageID
            LEFT JOIN UserGroup a2 on a2.UserGroupID =  a0.UserGroupID
            LEFT JOIN MenuCategory a3 on a3.MenuCategoryID =  a0.MenuCategoryID
            LEFT JOIN AppIcon a4 on a1.AppIconID = a4.AppIconID
            WHERE a0.UserGroupID = @userGroupId
            ORDER BY MenuOrder,WebpageName
            ";
        var connection = GetUnderlineConnection();
        return connection.Query(sql, new { userGroupId }).ToList();
    }

    public List<dynamic> GetDistinctMenuCategory(int userGroupId)
    {
        var sql = @"
            SELECT 
            DISTINCT a3.Name,a3.MenuCategoryID,a4.Name AS AppIconName,a4.CssClass AS AppIconCss,a4.IconColor AS AppIconColor, a3.MenuOrder
            FROM UserGroupPermission a0
            LEFT JOIN Webpage a1 on a1.WebpageID =  a0.WebpageID
            LEFT JOIN UserGroup a2 on a2.UserGroupID =  a0.UserGroupID
            LEFT JOIN MenuCategory a3 on a3.MenuCategoryID =  a0.MenuCategoryID
            LEFT JOIN AppIcon a4 on a3.AppIconID = a4.appIconID
            WHERE a2.UserGroupID = @userGroupId
            ORDER BY a3.MenuOrder
            ";
        var connection = GetUnderlineConnection();
        return connection.Query(sql, new { userGroupId }).ToList();
    }

    public List<dynamic> GetPermissionByParentWebpage(int userGroupId, int parentWebpageId)
    {
        var sql = @"
        SELECT
        a0.*
        ,a1.WebpageID AS WebpageWebpageID
        ,a1.ParentWebpageID AS WebpageParentWebpageID
        ,a1.Name AS WebpageName
        ,a1.Description AS WebpageDescription
        ,a1.URL AS WebpageURL
        ,a1.UiURL AS PageURL
        ,a2.UserGroupID AS UserGroupUserGroupID
        ,a2.Name AS UserGroupName
        ,a2.Description AS UserGroupDescription
        ,a3.MenuCategoryID AS MenuCategoryMenuCategoryID
        ,a3.Name AS MenuCategoryName
        ,a3.MenuOrder as MenuOrder
        ,a3.Description AS MenuCategoryDescription
        ,a4.Name AS AppIconName
        ,a4.CssClass AS AppIconCss
        ,a4.IconColor AS AppIconColor
        FROM UserGroupPermission a0
        LEFT JOIN Webpage a1 on a1.WebpageID =  a0.WebpageID
        LEFT JOIN UserGroup a2 on a2.UserGroupID =  a0.UserGroupID
        LEFT JOIN MenuCategory a3 on a3.MenuCategoryID =  a0.MenuCategoryID
        LEFT JOIN AppIcon a4 on a1.AppIconID = a4.AppIconID
        WHERE a0.UserGroupID = @userGroupId
        AND a1.ParentWebpageID = @parentWebpageId
        ORDER BY MenuOrder
        ";
        var connection = GetUnderlineConnection();
        return connection.Query(sql, new { userGroupId, parentWebpageId }).ToList();
    }
}