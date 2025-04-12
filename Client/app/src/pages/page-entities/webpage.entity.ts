import { AppIcon } from './appicon.entity';
export interface Webpage {
    WebpageId: any,
    ParentWebpageId: any,
    AppIconId: any,
    Name: any,
    Description: any,
    Url: any,
    AppIcon: AppIcon | null,
    ParentWebpage: Webpage | null,
    InverseParentWebpage: any,
    Products: any,
    UserGroupPermissions: any,
}

export const emptyWebpage = (): Webpage => {
    return {
        WebpageId: '',
        ParentWebpageId: 0,
        AppIconId: 0,
        Name: '',
        Description: '',
        Url: '',
        AppIcon: null,
        ParentWebpage: null,
        InverseParentWebpage: '',
        Products: '',
        UserGroupPermissions: '',
    }
}
