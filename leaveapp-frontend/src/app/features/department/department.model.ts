export interface Department {
    id: number;
    createdDate: string;
    name: string;
}

export interface CreateDepartment {
    name: string;
}

export interface ApiResponse<T> {
    success: boolean;
    message: string | null;
    data: T;
}