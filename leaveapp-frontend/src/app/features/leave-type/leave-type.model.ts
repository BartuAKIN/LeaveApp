export interface LeaveType {
    id: number;
    name: string;
    description: string;
}

export interface CreateLeaveType {
    name: string;
    description: string;
}

export interface ApiResponse<T> {
    success: boolean;
    message: string | null;
    data: T;
}