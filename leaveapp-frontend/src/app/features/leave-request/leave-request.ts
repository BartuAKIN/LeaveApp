import { Component, OnInit, signal } from '@angular/core';
import { ReactiveFormsModule, FormGroup, FormControl, Validators, AbstractControl, ValidationErrors } from '@angular/forms';
import { LeaveRequestApi } from './leave-request.api';
import { LeaveTypeApi } from '../leave-type/leave-type.api';
import { LeaveType } from '../leave-type/leave-type.model';
import { LeaveRequest } from './leave-request.model';
import { LeaveTypeComponent } from '../leave-type/leave-type';
import { CommonModule } from '@angular/common';

function dateRangeValidator(group: AbstractControl): ValidationErrors | null {
  const start = group.get('startDate')?.value;
  const end = group.get ('endDate')?.value;

  if (start && end && new Date(end) < new Date(start)) {
    return { dateRangeInvalid: true} 
  };

  return null;
}

@Component({
  selector: 'app-leave-request',
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './leave-request.html',
  styleUrl: './leave-request.css',
})
export class LeaveRequestComponent implements OnInit {
  leaveTypes = signal<LeaveType[]>([]);
  isFormOpen = signal(false);
  successMessage = signal<string | null>(null);

  leaveRequestForm = new FormGroup(
    {
      leaveTypeId: new FormControl<number | null>(null, [Validators.required]),
      startDate: new FormControl('', [Validators.required]),
      endDate: new FormControl('', [Validators.required]),
      description: new FormControl('', [Validators.required]),
    },
    { validators: dateRangeValidator }
  );

  constructor(
    private leaveRequestApi: LeaveRequestApi,
    private leaveTypeApi: LeaveTypeApi
  ) {}

  ngOnInit() {
    this.loadLeaveTypes();
    this.loadMyLeaveRequest();
  }

  myLeaveRequests = signal<LeaveRequest[]>([]);

  getStatusText(status: number): string {
    switch (status) {
      case 0: return 'Bekliyor';
      case 1: return 'Onaylandı';
      case 2: return 'Reddedildi';
      default: return 'Bilinmiyor'
    }
  }

  getLeaveTypeName(leaveTypeId: number): string {
    const lt = this.leaveTypes().find(l => l.id === leaveTypeId);
    return lt ? lt.name : '-';
  }

  loadMyLeaveRequest() {
    this.leaveRequestApi.getMyLeaveRequests().subscribe({
      next: (data) => this.myLeaveRequests.set(data),
      error: (err: any) => console.error('İzinlerim yüklenirken hara:', err),
    });
  }

  loadLeaveTypes() {
    this.leaveTypeApi.getAll().subscribe({
      next: (data) => this.leaveTypes.set(data),
      error: (err: any) => console.error('İzin türleri yüklenirken hata:', err),
    });
  }

  toggleForm() {
    this.isFormOpen.set(!this.isFormOpen());
    this.successMessage.set(null);
  }

  onSubmit() {
    if (this.leaveRequestForm.invalid) {
      this.leaveRequestForm.markAllAsTouched();
      return;
    }

    const newRequest = {
      leaveTypeId: this.leaveRequestForm.value.leaveTypeId!,
      startDate: this.leaveRequestForm.value.startDate!,
      endDate: this.leaveRequestForm.value.endDate!,
      description: this.leaveRequestForm.value.description ?? '',
    };

    this.leaveRequestApi.create(newRequest).subscribe({
      next: () => {
        this.leaveRequestForm.reset();
        this.isFormOpen.set(false);
        this.successMessage.set('İzin talebiniz başarıyla oluşturuldu.');
      },
      error: (err: any) => console.error('İzin talebi oluşturulurken hata:', err),
    });
  }
}
