export interface LeaveRequest {
    id: number;
    createdDate: string;
    userId: number;
    leaveTypeId: number;
    startDate: string;
    endDate: string;
    description: string;
    status: number;
    approvedByUserId: number | null;
    decisionDate: string | null;
}

export interface CreateLeaveRequest {
    startDate: string;
    endDate: string;
    leaveTypeId: number;
    description: string;
}

export interface ApiResponse<T> {
    success: boolean;
    message: string | null;
    data: T;
}

export interface LeaveRequestWithUser {
    id: number;
    userId: number;
    userFullName: string;
    leaveTypeId: number
    startDate: string;
    endDate: string;
    description: string;
    status: number;
    approvedByUserId: number | null;
    decisionDate: string | null;
}

export interface LeaveTypeSummary {
    leaveTypeName: string;
    totalDays: number;
}

export interface AnnualLeaveSummary {
    usedDays: number;
    remainingDays: number;
    totalQuota: number;
}

export interface LeaveTypeBreakdown {
    leaveTypeName: string;
    totalDays: number;
}

export interface TeamLeaveBarChart {
    userFullName: string;
    leaveBreakdown: LeaveTypeBreakdown[];
}

export interface MonthlyLeaveCount {
    month: number;
    monthName: string;
    totalRequests: number;
}