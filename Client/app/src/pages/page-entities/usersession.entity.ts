export interface UserSession {
    UserSessionId: any,
    UserId: any,
    SessionGuid: any,
    StartTime: any,
    EndTime: any,
    IsActive: any,
    ExpirationTimeFrame: any,
    User: any,
}

export const emptyUserSession = (): UserSession => {
    return {
        UserSessionId: '',
        UserId: '',
        SessionGuid: '',
        StartTime: '',
        EndTime: '',
        IsActive: '',
        ExpirationTimeFrame: '',
        User: '',
    }
}
