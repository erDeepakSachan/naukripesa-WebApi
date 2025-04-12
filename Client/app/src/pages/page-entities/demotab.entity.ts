export interface DemoTab {
    DemoId: any,
    Name: any,
    Description: any,
    UserId: any,
    AppIconId: any,
    Other: any,
    IsActive: any,
    AppIcon: any,
    User: any,
}

export const emptyDemoTab = (): DemoTab => {
    return {
        DemoId: '',
        Name: '',
        Description: '',
        UserId: '',
        AppIconId: '',
        Other: '',
        IsActive: '',
        AppIcon: '',
        User: '',
    }
}
