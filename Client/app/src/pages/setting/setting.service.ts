import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of, finalize, BehaviorSubject } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { Setting } from '../page-entities/setting.entity';
import { withNoGlobalLoaderGif } from './../../shared/auth.interceptor';
import { PageCommonService } from './../page-common.service';
import { ListingResponse } from './../page-entities/listing-response.entity';

@Injectable({
  providedIn: 'root'
})
export class SettingService extends PageCommonService {
  private baseUrl: string;
  private emptyListingResponse: ListingResponse<Setting> = { pageSize: 0, totalPageCount: 0, totalItemCount: 0, currentPageNo: 0, data: [] };

  constructor(override http: HttpClient) {
    super(http);
    this.baseUrl = `${this.apiBaseUrl}/setting`
  }

  list(pageNo: number = 0): Observable<ListingResponse<Setting>> {
    this.showLoader();
    return this.http.get<{ isSuccess: boolean, data: ListingResponse<Setting> }>(`${this.baseUrl}?pageNo=${pageNo}`, { context: withNoGlobalLoaderGif() }).pipe(
      map((response) => {
        if (response.isSuccess) {
          return response.data;
        } else {
          return this.emptyListingResponse;
        }
      }),
      catchError(() => {
        return of(this.emptyListingResponse);
      }))
      .pipe(finalize(() => this.hideLoader()));
  }

  search(q: string): Observable<ListingResponse<Setting>> {
    this.showLoader();
    return this.http.get<{ isSuccess: boolean, data: ListingResponse<Setting> }>(`${this.baseUrl}/search?q=${q}`, { context: withNoGlobalLoaderGif() }).pipe(
      map((response) => {
        if (response.isSuccess) {
          return response.data;
        } else {
          return this.emptyListingResponse;
        }
      }),
      catchError(() => {
        return of(this.emptyListingResponse);
      }))
      .pipe(finalize(() => this.hideLoader()));
  }

  add(setting: Setting): Observable<{ isSuccess: boolean, message: string }> {
    this.showLoader();
    return this.http.post<{ isSuccess: boolean, message: string }>(this.baseUrl, setting)
      .pipe(finalize(() => this.hideLoader()));
  }

  get(id: number): Observable<Setting> {
    this.showLoader();
    return this.http.get<Setting>(`${this.baseUrl}/${id}`)
      .pipe(finalize(() => this.hideLoader()));
  }

  edit(setting: Setting): Observable<{ isSuccess: boolean, message: string }> {
    this.showLoader();
    return this.http.put<{ isSuccess: boolean, message: string }>(this.baseUrl, setting)
      .pipe(finalize(() => this.hideLoader()));
  }

  delete(id: number): Observable<{ isSuccess: boolean, message: string }> {
    this.showLoader();
    return this.http.delete<{ isSuccess: boolean, message: string }>(`${this.baseUrl}/${id}`)
      .pipe(finalize(() => this.hideLoader()));
  }
}