import { jwtDecode } from 'jwt-decode';

export const getDecodedAccessToken = (token?: string): any => {
    try {
        return jwtDecode(token!);
    } catch (Error) {
        return null;
    }
};