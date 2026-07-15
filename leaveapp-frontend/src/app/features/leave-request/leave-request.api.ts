import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { map } from "rxjs";
import { environment } from "../../../environments/environment";
import { LeaveRequest, CreateLeaveRequest, ApiResponse } from "./leave-request.model";

@Injectable({
    providedIn: 'root'
})
export class LeaveRequestApi {
    private apiUrl = `${environment.apiUrl}/LeaveRequest`;

    constructor(private http: HttpClient) {}

    create(leaveRequest: CreateLeaveRequest) {
        return this.http.post<ApiResponse<LeaveRequest>>(this.apiUrl, leaveRequest)
        .pipe(map(response => response.data));
    }

    getMyLeaveRequests() {
        return this.http.get<LeaveRequest[]>(this.apiUrl)
    }
}
