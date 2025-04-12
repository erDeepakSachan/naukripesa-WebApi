export interface ListingResponse<T> {
    pageSize: number
    , totalPageCount: number
    , totalItemCount: number
    , currentPageNo: number
    , data: T[]
}