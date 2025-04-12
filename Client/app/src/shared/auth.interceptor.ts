import { inject } from '@angular/core';
import { HttpEvent, HttpInterceptorFn, HttpRequest, HttpHandlerFn, HttpErrorResponse, HttpContextToken, HttpContext } from '@angular/common/http';
import { Observable, catchError, finalize } from 'rxjs';
import { Router } from '@angular/router';
import { LoaderGifService } from '../shared/services/loader-gif.service';
import AppConstants from './app-constants';

const NO_AUTH = new HttpContextToken(() => false);
const NO_GLOBAL_LOADER = new HttpContextToken(() => false);

export const AuthInterceptor: HttpInterceptorFn = (req: HttpRequest<unknown>, next: HttpHandlerFn): Observable<HttpEvent<unknown>> => {
    const router = inject(Router);
    const loaderGifSvc = inject(LoaderGifService);

    if (!req.context.get(NO_GLOBAL_LOADER)) {
        loaderGifSvc.show();
    }

    if (req.context.get(NO_AUTH)) {
        return next(req).pipe(finalize(() => loaderGifSvc.hide()));
    }

    const token = localStorage.getItem(AppConstants.AuthTokenKey);

    const clonedReq = token
        ? req.clone({ setHeaders: { Authorization: `Bearer ${token}` } })
        : req;

    return next(clonedReq).pipe(
        catchError((error) => {
            if (error instanceof HttpErrorResponse && (error.status === 401)) {
                localStorage.removeItem(AppConstants.AuthTokenKey);
                router.navigate(['/login']);
            }
            if (error instanceof HttpErrorResponse && (error.status === 403)) {
                router.navigate(['/_403']);
            }
            throw error;
        })
    ).pipe(finalize(() => {
        if (!req.context.get(NO_GLOBAL_LOADER)) {
            loaderGifSvc.hide();
        }
    }));;
};

export function withNoAuth() {
    return new HttpContext().set(NO_AUTH, true);
}

export function withNoGlobalLoaderGif() {
    return new HttpContext().set(NO_GLOBAL_LOADER, true);
}
