import { Component, OnInit, signal } from '@angular/core';
import { ReactiveFormsModule, FormGroup, FormControl, Validators } from '@angular/forms';
import { PersonnelApi } from './personnel-api';
import { Personnel } from './personnel.model';
import { DepartmentApi } from '../department/department-api';
import { Department } from '../department/department.model';

@Component({
  selector: 'app-personnel',
  imports: [ReactiveFormsModule],
  templateUrl: './personnel.html',
  styleUrl: './personnel.css',
})
export class PersonnelComponent implements OnInit {
  personnel = signal<Personnel[]>([]);
  departments = signal<Department[]>([]);
  isFormOpen = signal(false);
  editingId = signal<number | null>(null);

  personnelForm = new FormGroup({
    firstName: new FormControl('', [Validators.required, Validators.minLength(2)]),
    lastName: new FormControl('', [Validators.required, Validators.minLength(2)]),
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [Validators.required, Validators.minLength(6)]),
    departmentId: new FormControl<number | null>(null, [Validators.required]),
    isManager: new FormControl(false),
  });

  editForm = new FormGroup({
    firstName: new FormControl('', [Validators.required, Validators.minLength(2)]),
    lastName: new FormControl('', [Validators.required, Validators.minLength(2)]),
    departmentId: new FormControl<number | null>(null, [Validators.required]),
    isManager: new FormControl(false),
    isActive: new FormControl(true),
  });

  constructor(
    private personnelApi: PersonnelApi,
    private departmentApi: DepartmentApi
  ) {}

  ngOnInit() {
    this.loadPersonnel();
    this.loadDepartments();
  }

  loadPersonnel() {
    this.personnelApi.getAll().subscribe({
      next: (data) => this.personnel.set(data),
      error: (err) => console.error('Personel yüklenirken hata:', err),
    });
  }

  loadDepartments() {
    this.departmentApi.getAll().subscribe({
      next: (data) => this.departments.set(data),
      error: (err) => console.error('Departmanlar yüklenirken hata:', err),
    });
  }

  getDepartmentName(departmentId: number): string {
    const dept = this.departments().find(d => d.id === departmentId);
    return dept ? dept.name : '-';
  }

  toggleForm() {
    this.isFormOpen.set(!this.isFormOpen());
  }

  onSubmit() {
    if (this.personnelForm.invalid) {
      this.personnelForm.markAllAsTouched();
      return;
    }

    const newPersonnel = {
      firstName: this.personnelForm.value.firstName!,
      lastName: this.personnelForm.value.lastName!,
      email: this.personnelForm.value.email!,
      password: this.personnelForm.value.password!,
      departmentId: this.personnelForm.value.departmentId!,
      isManager: this.personnelForm.value.isManager!,
    };

    this.personnelApi.create(newPersonnel).subscribe({
      next: () => {
        this.loadPersonnel();
        this.personnelForm.reset();
        this.isFormOpen.set(false);
      },
      error: (err) => console.error('Personel eklenirken hata:', err),
    });
  }

  startEdit(person: Personnel) {
    this.editingId.set(person.id);
    this.editForm.setValue({
      firstName: person.firstName,
      lastName: person.lastName,
      departmentId: person.departmentId,
      isManager: person.isManager,
      isActive: person.isActive,
    });
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
      firstName: this.editForm.value.firstName!,
      lastName: this.editForm.value.lastName!,
      departmentId: this.editForm.value.departmentId!,
      isManager: this.editForm.value.isManager!,
      isActive: this.editForm.value.isActive!,
    };

    this.personnelApi.update(id, updated).subscribe({
      next: () => {
        this.loadPersonnel();
        this.editingId.set(null);
      },
      error: (err: any) => console.error('Personel güncellenirken hata:', err),
    });
  }

  deletePersonnel(id: number) {
    if (!confirm('Bu personeli pasif etmek istediğinize emin misiniz?')) {
      return;
    }

    this.personnelApi.delete(id).subscribe({
      next: () => this.loadPersonnel(),
      error: (err: any) => console.error('Personel silinirken hata:', err),
    });
  }
}