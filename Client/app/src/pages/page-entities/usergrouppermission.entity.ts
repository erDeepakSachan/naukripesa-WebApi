import { MenuCategory } from './menucategory.entity';
import { UserGroup } from './usergroup.entity';
import { Webpage } from './webpage.entity';

export interface UserGroupPermission {
    UserGroupPermissionId: any,
    UserGroupId: any,
    WebpageId: any,
    MenuCategoryId: any,
    IsVisible: any,
    MenuCategory: MenuCategory | null,
    UserGroup: UserGroup | null,
    Webpage: Webpage | null,
}

export const emptyUserGroupPermission = (): UserGroupPermission => {
    return {
        UserGroupPermissionId: 0,
        UserGroupId: 0,
        WebpageId: 0,
        MenuCategoryId: 0,
        IsVisible: false,
        MenuCategory: null,
        UserGroup: null,
        Webpage: null,
    }
}
