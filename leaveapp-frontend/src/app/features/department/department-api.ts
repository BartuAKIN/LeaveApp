import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { map } from 'rxjs';
import { environment } from "../../../environments/environment";
import { Department, CreateDepartment, ApiResponse } from "./department.model";

@Injectable({
    providedIn: 'root'
})
export class DepartmentApi {
    private apiUrl = `${environment.apiUrl}/Department`;

    constructor(private http: HttpClient) {}

    getAll() {
        return this.http.get<ApiResponse<Department[]>>(this.apiUrl)
        .pipe(map(response => response.data));
    }

   getById(id: number) {
    return this.http.get<ApiResponse<Department>>(`${this.apiUrl}/${id}`)
      .pipe(map(response => response.data));
  }

  create(department: CreateDepartment) {
    return this.http.post<ApiResponse<Department>>(this.apiUrl, department)
      .pipe(map(response => response.data));
  }

  update(id: number, department: CreateDepartment) {
    return this.http.put<ApiResponse<Department>>(`${this.apiUrl}/${id}`, department)
      .pipe(map(response => response.data));
  }

  delete(id: number) {
    return this.http.delete<ApiResponse<null>>(`${this.apiUrl}/${id}`)
      .pipe(map(response => response.data));
  }
}
