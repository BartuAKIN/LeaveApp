import { Component, OnInit, signal } from '@angular/core';
import { ReactiveFormsModule, FormGroup, FormControl, Validators } from '@angular/forms';
import { LeaveTypeApi } from './leave-type.api';
import { LeaveType } from './leave-type.model';

@Component({
  selector: 'app-leave-type',
  imports: [ReactiveFormsModule],
  templateUrl: './leave-type.html',
  styleUrl: './leave-type.css',
})
export class LeaveTypeComponent implements OnInit {
  leaveTypes = signal<LeaveType[]>([]);
  isFormOpen = signal(false);
  editingId = signal<number | null>(null);

  leaveTypeForm = new FormGroup({
    name: new FormControl('', [Validators.required, Validators.minLength(2)]),
    description: new FormControl('', [Validators.required]),
  });

  editForm = new FormGroup({
    name: new FormControl('', [Validators.required, Validators.minLength(2)]),
    description: new FormControl('', [Validators.required]),
  })

  constructor(private leaveTypeApi: LeaveTypeApi) {}

  ngOnInit() {
    this.loadLeaveTypes();
  }

  loadLeaveTypes() {
    this.leaveTypeApi.getAll().subscribe({
      next: (data) => this.leaveTypes.set(data),
      error: (err: any) => console.error('İzin türleri yüklenirken hata:', err),
    });
  }

  toggleForm() {
    this.isFormOpen.set(!this.isFormOpen());
  }

  onSubmit() {
    if (this.leaveTypeForm.invalid) {
      this.leaveTypeForm.markAllAsTouched();
      return;
    }

    const newLeaveType = {
      name: this.leaveTypeForm.value.name!,
      description: this.leaveTypeForm.value.description!,
    };

    this.leaveTypeApi.create(newLeaveType).subscribe({
      next: () => {
        this.loadLeaveTypes();
        this.leaveTypeForm.reset();
        this.isFormOpen.set(false);
      },
      error: (err: any) => console.error('İzin türü eklenirken hata:', err),
    });
  }

  startEdit(leaveType: LeaveType) {
    this.editingId.set(leaveType.id);
    this.editForm.setValue({
      name: leaveType.name,
      description: leaveType.description ?? '',
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
      name: this.editForm.value.name!,
      description: this.editForm.value.description!,
    };

    this.leaveTypeApi.update(id, updated).subscribe({
      next: () => {
        this.loadLeaveTypes();
        this.editingId.set(null);
      },
      error: (err: any) => console.error('İzin türü güncellenirken hata:', err),
    });
  }

  deleteLeaveType(id: number) {
    if (!confirm('Bu izin türünü silmek istediğinizden emin misiniz=')) {
      return;
    }

    this.leaveTypeApi.delete(id).subscribe({
      next: () => this.loadLeaveTypes(),
      error: (err: any) => console.error('İzin türü silinirken hata:', err),
    });
  }
}
