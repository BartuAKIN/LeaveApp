import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { map } from 'rxjs';
import { environment } from "../../../environments/environment";
import { Personnel, CreatePersonnel, UpdatePersonnel, ApiResponse } from "./personnel.model";

@Injectable({
  providedIn: 'root'
})
export class PersonnelApi {
  private apiUrl = `${environment.apiUrl}/User`;

  constructor(private http: HttpClient) {}

  getAll() {
    return this.http.get<ApiResponse<Personnel[]>>(this.apiUrl)
      .pipe(map(response => response.data));
  }

  getById(id: number) {
    return this.http.get<ApiResponse<Personnel>>(`${this.apiUrl}/${id}`)
      .pipe(map(response => response.data));
  }

  create(personnel: CreatePersonnel) {
    return this.http.post<ApiResponse<Personnel>>(this.apiUrl, personnel)
      .pipe(map(response => response.data));
  }

  update(id: number, personnel: UpdatePersonnel) {
    return this.http.put<ApiResponse<Personnel>>(`${this.apiUrl}/${id}`, personnel)
      .pipe(map(response => response.data));
  }

  delete(id: number) {
    return this.http.delete<ApiResponse<null>>(`${this.apiUrl}/${id}`)
      .pipe(map(response => response.data));
  }
}