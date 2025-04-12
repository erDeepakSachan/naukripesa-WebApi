import { AppIcon } from './appicon.entity';

export interface MenuCategory {
    MenuCategoryId: any,
    AppIconId: any,
    Name: any,
    Description: any,
    MenuOrder: any,
    AppIcon?: AppIcon | null,
    UserGroupPermissions: any,
}

export const emptyMenuCategory = (): MenuCategory => {
    return {
        MenuCategoryId: '',
        AppIconId: 0,
        Name: '',
        Description: '',
        MenuOrder: '',
        AppIcon: null,
        UserGroupPermissions: '',
    }
}
