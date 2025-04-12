export interface Product {
    ProductId: any,
    Pname: any,
    Description: any,
    WebPageId: any,
    UserTypeId: any,
    IsActive: any,
    UserType: any,
    WebPage: any,
}

export const emptyProduct = (): Product => {
    return {
        ProductId: '',
        Pname: '',
        Description: '',
        WebPageId: '',
        UserTypeId: '',
        IsActive: '',
        UserType: '',
        WebPage: '',
    }
}
