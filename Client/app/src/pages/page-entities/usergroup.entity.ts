export interface UserGroup {
    UserGroupId: any,
    Name: any,
    Description: any,
    UserGroupPermissions: any,
    Users: any,
}

export const emptyUserGroup = (): UserGroup => {
    return {
        UserGroupId: '',
        Name: '',
        Description: '',
        UserGroupPermissions: '',
        Users: '',
    }
}
