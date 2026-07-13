export interface Personnel {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  departmentId: number;
  isManager: boolean;
  isActive: boolean;
}

export interface CreatePersonnel {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
  departmentId: number;
  isManager: boolean;
}

export interface UpdatePersonnel {
  firstName: string;
  lastName: string;
  departmentId: number;
  isManager: boolean;
  isActive: boolean;
}

export interface ApiResponse<T> {
  success: boolean;
  message: string | null;
  data: T;
}