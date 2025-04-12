export interface Company {
    CompanyId: number
    CurrencyId: number
    Name: string
    Description: string
    CreatedBy: number
    CreatedOn: string
    ModifiedBy: number
    ModifiedOn: string
}

export const emptyCompany = (): Company => {
    return {
        CompanyId: 0,
        CurrencyId: 0,
        Name: '',
        Description: '',
        CreatedBy: 0,
        CreatedOn: '',
        ModifiedBy: 0,
        ModifiedOn: '',
    }
}