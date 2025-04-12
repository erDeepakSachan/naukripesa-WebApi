import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of, finalize, BehaviorSubject } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { NeoContextService } from './../shared/services/neo-context.service';
import { withNoGlobalLoaderGif } from './../shared/auth.interceptor';
import { DdlItem } from './page-entities/ddl-item.entity';

@Injectable({
  providedIn: 'root'
})
export class PageCommonService {
  protected neoCtx = inject(NeoContextService);
  protected loading = new BehaviorSubject<boolean>(false);
  protected apiBaseUrl: string = '';
  protected baseUrlHome: string = '';
  protected requestCount = 0;

  constructor(protected http: HttpClient) {
    this.apiBaseUrl = `${this.neoCtx.context.AppSettings?.ApiBaseUrl}`;
    this.baseUrlHome = `${this.apiBaseUrl}/Home`
  }

  loading$ = this.loading.asObservable();

  loadDropDownList(typeName: string, selectedValue?: any): Observable<DdlItem[]> {
    this.showLoader();
    return this.http.post<{ isSuccess: boolean, data: DdlItem[] }>(`${this.baseUrlHome}/LoadDropDownOptions?type=${typeName}&selectedValue=${selectedValue}`, {}, { context: withNoGlobalLoaderGif() }).pipe(
      map((response) => {
        if (response.isSuccess) {
          return response.data;
        } else {
          return [];
        }
      }),
      catchError(() => {
        return of([]);
      })
    ).pipe(finalize(() => this.hideLoader()));
  }

  showLoader() {
    this.requestCount++;
    this.loading.next(true);
  }

  hideLoader() {
    this.loading.next(false);
    this.requestCount--;
    if (this.requestCount <= 0) {
      // this.loading.next(false);
      this.requestCount = 0;
    }
  }
}