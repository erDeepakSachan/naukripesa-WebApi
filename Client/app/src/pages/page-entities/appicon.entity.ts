export interface AppIcon {
    AppIconId: any,
    Name: any,
    CssClass: any,
    IconColor: any,
    DemoTabs: any,
    MenuCategories: any,
    Webpages: any,
}

export const emptyAppIcon = (): AppIcon => {
    return {
        AppIconId: '',
        Name: '',
        CssClass: '',
        IconColor: '',
        DemoTabs: '',
        MenuCategories: '',
        Webpages: '',
    }
}
