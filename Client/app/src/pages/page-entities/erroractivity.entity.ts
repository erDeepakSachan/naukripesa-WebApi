export interface ErrorActivity {
    ErrorActivityId: any,
    Title: any,
    ErrorMessage: any,
    StackTraceMessage: any,
    ErrorDateTime: any,
    UserId: any,
}

export const emptyErrorActivity = (): ErrorActivity => {
    return {
        ErrorActivityId: '',
        Title: '',
        ErrorMessage: '',
        StackTraceMessage: '',
        ErrorDateTime: '',
        UserId: '',
    }
}
