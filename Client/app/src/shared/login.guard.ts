import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import AppConstants from './app-constants';

export const loginGuard: CanActivateFn = () => {
    const router = inject(Router);
    const token = localStorage.getItem(AppConstants.AuthTokenKey);

    if (token) {
        router.navigate(['/dashboard']);
        return false;
    }

    return true;
};
