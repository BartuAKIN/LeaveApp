import { Component, OnInit, AfterViewInit, ViewChild, ElementRef, viewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Chart, registerables } from 'chart.js';
import { LeaveRequestApi } from '../leave-request/leave-request.api';
import { LeaveTypeSummary, AnnualLeaveSummary } from '../leave-request/leave-request.model';

Chart.register(...registerables);

@Component({
  selector: 'app-dashboard',
  imports: [CommonModule],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css',
})
export class Dashboard implements OnInit, AfterViewInit {
  @ViewChild('leaveTypeChart') leaveTypeChartRef!: ElementRef<HTMLCanvasElement>;
  @ViewChild('annualChart') annualChartRef!: ElementRef<HTMLCanvasElement>;

  leaveTypeSummary: LeaveTypeSummary[] = [];
  annualSummary: AnnualLeaveSummary | null = null;

  private leaveTypeChartInstance: Chart | null = null;
  private annualChartInstance: Chart | null = null;

  constructor(private leaveRequestApi: LeaveRequestApi) {}

  ngOnInit() {
    this.leaveRequestApi.getMyLeaveSummary().subscribe({
      next: (data) => {
        this.leaveTypeSummary = data;
        this.renderLeaveTypeChart();
      },
      error: (err: any) => console.error('İzin özeti yüklenirken hata:', err),
    });

    this.leaveRequestApi.getMyAnnualSummary().subscribe({
      next: (data) => {
        this.annualSummary = data;
        this.renderAnnualChart();
      },
      error: (err: any) => console.error('Yıllık izin özeti yüklenirken hata:', err)
    })
  }

  ngAfterViewInit() {
    // Veriler ngOnInit'te henüz gelmemiş olabilir, geldiğinde renderX metodları zaten çağrılıyor.
    // Burada ekstra bir şey yapmaya gerek yok, ViewChild referansları artık hazır.
  }

  renderLeaveTypeChart() {
    if (!this.leaveTypeChartRef) {
      return;
    }

    if (this.leaveTypeChartInstance) {
      this.leaveTypeChartInstance.destroy();
    }

    this.leaveTypeChartInstance = new Chart(this.leaveTypeChartRef.nativeElement, {
      type: 'pie',
      data: {
        labels: this.leaveTypeSummary.map((s) => s.leaveTypeName),
        datasets: [
          {
            data: this.leaveTypeSummary.map((s) => s.totalDays),
            backgroundColor: ['#4e79a7', '#f28e2b', '#e15759', '#76b7b2', '#59a14f'],
          },
        ],
      },
      options: {
        responsive: true,
        plugins: {
          title: { display: true, text: 'İzin Türüne Göre Kullanılan Gün' },
        },
      },
    });
  }

  renderAnnualChart() {
    if (!this.annualChartRef || !this.annualSummary) {
      return;
    }

    if (this.annualChartInstance) {
      this.annualChartInstance.destroy();
    }

    this.annualChartInstance = new Chart(this.annualChartRef.nativeElement, {
      type: 'pie',
      data: {
        labels: ['Kullanılan Gün', 'Kalan Gün'],
        datasets: [
          {
            data: [this.annualSummary.usedDays, this.annualSummary.remainingDays],
            backgroundColor: ['#e15759', '#59a14f'],
          },
        ],
      },
      options: {
        responsive: true,
        plugins: {
          title: { display: true, text: 'Yılık İzin Kullanım Durumu' },
        },
      },
    });
  }
}
