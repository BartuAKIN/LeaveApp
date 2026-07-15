import { Component, OnInit, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LeaveRequestApi } from '../leave-request/leave-request.api';
import { LeaveTypeApi } from '../leave-type/leave-type.api';
import { LeaveType } from '../leave-type/leave-type.model';
import { LeaveRequestWithUser } from '../leave-request/leave-request.model';

@Component({
  selector: 'app-leave-approval',
  imports: [CommonModule],
  templateUrl: './leave-approval.html',
  styleUrl: './leave-approval.css',
})
export class LeaveApproval implements OnInit{
  teamLeaveRequests = signal<LeaveRequestWithUser[]>([]);
  leaveTypes = signal<LeaveType[]>([]);

  pendingRequests = computed(() =>
    this.teamLeaveRequests().filter((r) => r.status === 0)
  );

  constructor(
    private leaveRequestApi: LeaveRequestApi,
    private leaveTypeApi: LeaveTypeApi
  ) {}

  ngOnInit() {
    this.loadTeamRequests();
    this.loadLeaveTypes();
  }

  loadTeamRequests() {
    this.leaveRequestApi.getTeamLeaveRequests().subscribe({
      next: (data) => this.teamLeaveRequests.set(data),
      error: (err: any) => console.error('Takım izinleri yüklenirken hata:', err),
    });
  }

  loadLeaveTypes() {
  this.leaveTypeApi.getAll().subscribe({
    next: (data) => this.leaveTypes.set(data),
    error: (err: any) => console.error('İzin türleri yüklenirken hata:', err),
  });
  }

  getLeaveTypeName(leaveTypeId: number): string {
    const lt = this.leaveTypes().find((l) => l.id === leaveTypeId);
    return lt ? lt.name : '-';
  }

  getStatusText(status: number): string {
    switch (status) {
      case 0: return 'Bekliyor';
      case 1: return 'Onaylandı';
      case 2: return 'Reddedildi';
      default: return 'Bilinmiyor';
    }
  }

  approve(id: number) {
    this.leaveRequestApi.updateStatus(id, 1).subscribe({
      next: () => this.loadTeamRequests(),
      error: (err: any) => console.error('Onaylama sırasında hata:', err),
    });
  }

  reject(id:number) {
    this.leaveRequestApi.updateStatus(id, 2).subscribe({
      next: () => this.loadTeamRequests(),
      error: (err: any) => console.error('Reddetme sırasında hata:', err),
    });
  }
}
