import { Component, OnInit, AfterViewInit, ViewChild, ElementRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Chart, registerables } from 'chart.js';
import { LeaveRequestApi } from '../leave-request/leave-request.api';
import { LeaveTypeSummary, AnnualLeaveSummary, TeamLeaveBarChart, MonthlyLeaveCount } from '../leave-request/leave-request.model';
import { Auth } from '../../core/services/auth';

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
  @ViewChild('barChart') barChartRef!: ElementRef<HTMLCanvasElement>;
  @ViewChild('lineChart') lineChartRef!: ElementRef<HTMLCanvasElement>;

  leaveTypeSummary: LeaveTypeSummary[] = [];
  annualSummary: AnnualLeaveSummary | null = null;
  teamBarData: TeamLeaveBarChart[] = [];
  monthlyData: MonthlyLeaveCount[] = [];

  isManager = false;
  userFullName: string | null = null;

  private leaveTypeChartInstance: Chart | null = null;
  private annualChartInstance: Chart | null = null;
  private barChartInstance: Chart | null = null;
  private lineChartInstance: Chart | null = null;

  constructor(
    private leaveRequestApi: LeaveRequestApi,
    private authService: Auth
  ) {}

  ngOnInit() {
    this.isManager = this.authService.isManager();
    this.userFullName = this.authService.getUserFullName();
    
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
    });

    if (this.isManager) {
    this.leaveRequestApi.getTeamLeaveBarChart().subscribe({
      next: (data) => {
        this.teamBarData = data;
        this.renderBarChart();
      },
      error: (err: any) => console.error('Takım izin özeti yüklenirken hata:', err),
    });

    this.leaveRequestApi.getMonthlyLeaveCount().subscribe({
      next: (data) => {
        this.monthlyData = data;
        this.renderLineChart();
      },
      error: (err: any) => console.error('Aylık izin sayısı yüklenirken hata:', err),
    });
    }
  }

  ngAfterViewInit() {
    // Veriler ngOnInit'te henüz gelmemiş olabilir, geldiğinde renderX metodları zaten çağrılıyor.
    // Burada ekstra bir şey yapmaya gerek yok, ViewChild referansları artık hazır.
  }

  renderLeaveTypeChart() {
    if (!this.leaveTypeChartRef) return;
    if (this.leaveTypeChartInstance) this.leaveTypeChartInstance.destroy();
    
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
        maintainAspectRatio: false,
        plugins: {
          title: { display: true, text: 'İzin Türüne Göre Kullanılan Gün' },
        },
      },
    });
  }

  renderAnnualChart() {
    if (!this.annualChartRef || !this.annualSummary) return;
    if (this.annualChartInstance) this.annualChartInstance.destroy();

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

  renderBarChart() {
    if (!this.barChartRef) return;
    if (this.barChartInstance) this.barChartInstance.destroy();

    const labels = this.teamBarData.map((u) => u.userFullName);

    const leaveTypeNames = Array.from(
      new Set(this.teamBarData.flatMap((u) => u.leaveBreakdown.map((b) => b.leaveTypeName)))
    );

    const colors = ['#4e78a7', '#f28e2b', '#e15759', '#76b7b2', '#59a14f'];

    const datasets = leaveTypeNames.map((typeName, index) => ({
      label: typeName,
      data: this.teamBarData.map((u) => {
        const match = u.leaveBreakdown.find((b) => b.leaveTypeName === typeName);
        return match ? match.totalDays: 0;
      }),
      backgroundColor: colors[index % colors.length],
    }));

    this.barChartInstance = new Chart(this.barChartRef.nativeElement, {
      type: 'bar',
      data: { labels, datasets },
      options: {
        responsive: true,
        scales: {
          x: { stacked: true },
          y: { stacked: true, beginAtZero: true },
        },
        plugins: { title: { display: true, text: 'Çalışanların İzin Kullanımı (Bu Yıl)' } },
      },
    });
  }

  renderLineChart () {
    if (!this.lineChartRef) return;
    if (this.lineChartInstance) this.lineChartInstance.destroy();

    this.lineChartInstance = new Chart(this.lineChartRef.nativeElement, {
      type: 'line',
      data: {
        labels: this.monthlyData.map((m) => m.monthName),
        datasets: [
          {
            label: 'İzin Talebi Sayısı',
            data: this.monthlyData.map((m) => m.totalRequests),
            borderColor: '#4e79e7',
            backgroundColor: '#4e79e7',
            tension: 0.3,
          },
        ],
      },
      options: {
        responsive: true,
        scales: { y: { beginAtZero: true, ticks: { stepSize: 1 } } },
        plugins: { title: { display: true, text: 'Aylara Göre Toplam İzin Talebi' } },
      },
    });
  }
}
