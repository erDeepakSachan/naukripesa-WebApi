export interface Currency {
    CurrencyId: any,
    Name: any,
    Description: any,
    CreatedBy: any,
    CreatedOn: any,
    ModifiedBy: any,
    ModifiedOn: any,
    Companies: any,
}

export const emptyCurrency = (): Currency => {
    return {
        CurrencyId: '',
        Name: '',
        Description: '',
        CreatedBy: '',
        CreatedOn: '',
        ModifiedBy: '',
        ModifiedOn: '',
        Companies: '',
    }
}
