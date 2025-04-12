import { Company } from './company.entity';
export interface Setting {
    SettingId: number,
    CompanyId: number,
    Name: string,
    Value: string,
    CreatedOn: Date,
    CreatedBy: number,
    ModifiedOn: Date,
    ModifiedBy: number,
    IsArchived: boolean,
    Company?: Company
}

export const emptySetting = (): Setting => {
    return {
        CompanyId: 0,
        CreatedBy: 0,
        CreatedOn: new Date(),
        IsArchived: false,
        ModifiedBy: 0,
        ModifiedOn: new Date(),
        Name: '',
        SettingId: 0,
        Value: ''
    }
}