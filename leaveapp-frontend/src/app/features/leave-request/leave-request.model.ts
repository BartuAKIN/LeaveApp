export interface LeaveRequest {
    id: number;
    userId: number;
    leaveTypeId: number;
    startDate: string;
    endDate: string;
    description: PerformanceServerTiming;
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