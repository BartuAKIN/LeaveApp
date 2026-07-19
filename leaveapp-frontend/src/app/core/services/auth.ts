import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { environment } from "../../../environments/environment";
import { tap } from 'rxjs';

@Injectable({
    providedIn:'root'
})
export class Auth {
    private apiUrl = environment.apiUrl;
    private tokenKey = 'leaveapp_token';

    constructor(private http: HttpClient) {}

    login(email: string, password: string) {
        return this.http.post<{ token: string }> (`${this.apiUrl}/Auth/login`, { email, password })
        .pipe(
            tap((response) => {
                this.saveToken(response.token);
            })
        );
    }

        saveToken(token: string) {
            localStorage.setItem(this.tokenKey, token);
        }

        getToken(): string | null {
            return localStorage.getItem(this.tokenKey);
        }

        logout() {
            localStorage.removeItem(this.tokenKey);
        }

        isLoggedIn(): boolean {
            return !!this.getToken();
        }

        private decodeToken(): any | null {
            const token = this.getToken();
            if (!token) {
                return null;
            }
            try {
                const payload = token.split('.')[1];
                const decoded = atob(payload);
                return JSON.parse(decoded);
            } catch {
                return null;
            }
        }

        isManager(): boolean {
            const decoded = this.decodeToken();
            if (!decoded) {
                return false;
            }
            const isManagerClaim = decoded['IsManager'] ?? decoded['isManager'];
            return isManagerClaim === 'True' || isManagerClaim === true || isManagerClaim === 'true';
        }

        getUserFullName(): string | null {
            const decoded = this.decodeToken();
             if (!decoded) {
                 return null;
        }
        return decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'] ?? null;
    }
}