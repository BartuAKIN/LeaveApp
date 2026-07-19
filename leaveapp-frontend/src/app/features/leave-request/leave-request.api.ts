import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { map } from "rxjs";
import { environment } from "../../../environments/environment";
import { LeaveRequest, CreateLeaveRequest, ApiResponse, LeaveRequestWithUser, LeaveTypeSummary, AnnualLeaveSummary, TeamLeaveBarChart, MonthlyLeaveCount } from "./leave-request.model";

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

    getTeamLeaveRequests() {
        return this.http.get<LeaveRequestWithUser[]>(`${this.apiUrl}/team`);
    }

    updateStatus(leaveRequestId: number, newStatus: number) {
        return this.http.put<LeaveRequest>(`${this.apiUrl}/${leaveRequestId}/status`, { newStatus });
    }

    getMyLeaveSummary() {
        return this.http.get<LeaveTypeSummary[]>(`${this.apiUrl}/my-summary`);
    }

    getMyAnnualSummary() {
        return this.http.get<AnnualLeaveSummary>(`${this.apiUrl}/my-annual-summary`);
    }

    getTeamLeaveBarChart() {
        return this.http.get<TeamLeaveBarChart[]>(`${this.apiUrl}/team-bar-chart`);
    }

    getMonthlyLeaveCount() {
        return this.http.get<MonthlyLeaveCount[]>(`${this.apiUrl}/monthly-leave-count`);
    }
}
