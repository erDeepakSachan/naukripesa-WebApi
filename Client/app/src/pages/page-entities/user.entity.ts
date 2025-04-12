import { Company } from './company.entity';
import { UserGroup } from './usergroup.entity';
import { UserType } from './usertype.entity';

export interface User {
    UserId: any;
    UserTypeId: any;
    UserGroupId: any;
    CompanyId: any;
    Code: any;
    Name: any;
    Email: any;
    Password: any;
    MobileNo: any;
    RecentLogin: any;
    CreatedOn: any;
    CreatedBy: any;
    ModifiedOn: any;
    ModifiedBy: any;
    IsArchived: any;
    Otp: any;
    Company: Company | null;
    UserGroup: UserGroup | null;
    UserType: UserType | null;
    AccessActivities: any;
    DemoTabs: any;
    UserSessions: any;
}

export const emptyUser = (): User => {
    return {
        UserId: 0,
        UserTypeId: 0,
        UserGroupId: 0,
        CompanyId: 0,
        Code: '',
        Name: '',
        Email: '',
        Password: '',
        MobileNo: '',
        RecentLogin: '',
        CreatedOn: '',
        CreatedBy: '',
        ModifiedOn: '',
        ModifiedBy: '',
        IsArchived: false,
        Otp: '',
        Company: null,
        UserGroup: null,
        UserType: null,
        AccessActivities: '',
        DemoTabs: '',
        UserSessions: '',
    };
};
