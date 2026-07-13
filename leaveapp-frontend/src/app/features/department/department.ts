import { Component, OnInit, signal } from '@angular/core';
import { ReactiveFormsModule, FormGroup, FormControl, Validators } from '@angular/forms';
import { DepartmentApi } from './department-api';
import { Department } from './department.model';

@Component({
  selector: 'app-department',
  imports: [ReactiveFormsModule],
  templateUrl: './department.html',
  styleUrl: './department.css',
})
export class DepartmentComponent implements OnInit {
  departments = signal<Department[]>([]);
  isFormOpen = signal(false);
  editingId = signal<number | null>(null);

  departmentForm = new FormGroup({
    name: new FormControl('', [Validators.required, Validators.minLength(2)]),
  });

  editForm = new FormGroup({
    name: new FormControl('', [Validators.required, Validators.minLength(2)]),
  });

  constructor(private departmentApi: DepartmentApi) {}

  ngOnInit() {
    this.loadDepartments();
  }

  loadDepartments() {
    this.departmentApi.getAll().subscribe({
      next: (data) => this.departments.set(data),
      error: (err) => console.error('Departmanlar yüklenirken hata:', err),
    });
  }

  toggleForm() {
    this.isFormOpen.set(!this.isFormOpen());
  }

  onSubmit() {
    if (this.departmentForm.invalid) {
      this.departmentForm.markAllAsTouched();
      return;
    }

    const newDepartment = {
      name: this.departmentForm.value.name!,
    };

    this.departmentApi.create(newDepartment).subscribe({
      next: () => {
        this.loadDepartments();
        this.departmentForm.reset();
        this.isFormOpen.set(false);
      },
      error: (err) => console.error('Departman eklenirken hata:', err),
    });
  }

  startEdit(dept: Department) {
    this.editingId.set(dept.id);
    this.editForm.setValue({ name: dept.name });
  }

  cancelEdit() {
    this.editingId.set(null);
  }

  saveEdit(id: number) {
    if (this.editForm.invalid) {
      this.editForm.markAllAsTouched();
      return;
    }

    const updated = {
      name: this.editForm.value.name!,
    };

    this.departmentApi.update(id, updated).subscribe({
      next: () => {
        this.loadDepartments();
        this.editingId.set(null);
      },
      error: (err) => console.error('Departman güncellenirken hata:', err),
    });
  }

  deleteDepartment(id: number) {
    if (!confirm('Bu departmanı silmek istediğinize emin misiniz?')) {
      return;
    }

    this.departmentApi.delete(id).subscribe({
      next: () => this.loadDepartments(),
      error: (err) => console.error('Departman silinirken hata:', err),
    });
  }
}