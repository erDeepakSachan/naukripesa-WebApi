export interface UserType {
    UserTypeId: any,
    Name: any,
    Description: any,
    Products: any,
    Users: any,
}

export const emptyUserType = (): UserType => {
    return {
        UserTypeId: '',
        Name: '',
        Description: '',
        Products: '',
        Users: '',
    }
}
