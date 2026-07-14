import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { map } from "rxjs";
import { environment } from "../../../environments/environment";
import { LeaveType, CreateLeaveType, ApiResponse } from "./leave-type.model";

@Injectable({
    providedIn: 'root'
})
export class LeaveTypeApi {
    private apiUrl = `${environment.apiUrl}/LeaveType`;

    constructor(private http: HttpClient) {}

    getAll() {
        return this.http.get<ApiResponse<LeaveType[]>>(this.apiUrl)
        .pipe(map(response => response.data));
    }

    getById(id: number) {
        return this.http.get<ApiResponse<LeaveType>>(`${this.apiUrl}/${id}`)
        .pipe(map(response => response.data));
    }

    create(leaveType: CreateLeaveType) {
        return this.http.post<ApiResponse<LeaveType>>(this.apiUrl, leaveType)
        .pipe(map(response => response.data));
    }

    update(id: number, leaveType: CreateLeaveType) {
        return this.http.put<ApiResponse<LeaveType>>(`${this.apiUrl}/${id}`, leaveType)
        .pipe(map(response => response.data));
    }

    delete(id: number) {
        return this.http.delete<ApiResponse<null>>(`${this.apiUrl}/${id}`)
        .pipe(map(response => response.data));
    }
}