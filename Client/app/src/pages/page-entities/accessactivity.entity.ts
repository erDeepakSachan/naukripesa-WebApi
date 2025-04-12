export interface AccessActivity {
    AccessActivityId: any,
    UserId: any,
    UserSessionId: any,
    ActivityType: any,
    CreatedOn: any,
    User: any,
}

export const emptyAccessActivity = (): AccessActivity => {
    return {
        AccessActivityId: '',
        UserId: '',
        UserSessionId: '',
        ActivityType: '',
        CreatedOn: '',
        User: '',
    }
}
