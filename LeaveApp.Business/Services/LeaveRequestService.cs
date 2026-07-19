using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using LeaveApp.Business.Common;
using LeaveApp.Business.DTOs;
using LeaveApp.Business.Interfaces;
using LeaveApp.Data.Repositories;
using LeaveApp.Domain.Entities;
using LeaveApp.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace LeaveApp.Business.Services
{
    public class LeaveRequestService : ILeaveRequestService
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILeaveTypeRepository _leaveTypeRepository;

        public LeaveRequestService(ILeaveRequestRepository leaveRequestRepository, UserManager<ApplicationUser> userManager, ILeaveTypeRepository leaveTypeRepository)
        {
            _leaveRequestRepository = leaveRequestRepository;
            _userManager = userManager;
            _leaveTypeRepository = leaveTypeRepository;
        }

        public async Task<ServiceResult<LeaveRequest>> CreateLeaveRequest(int userId, CreateLeaveRequestDto createLeaveRequestDto)
        {
            var newLeaveRequest = new LeaveRequest
            {
                UserId = userId,
                LeaveTypeId = createLeaveRequestDto.LeaveTypeId,
                StartDate = createLeaveRequestDto.StartDate,
                EndDate = createLeaveRequestDto.EndDate,
                Description = createLeaveRequestDto.Description,
                Status = (int)LeaveStatus.Pending,
                CreatedDate = DateTime.UtcNow,
                };

            _leaveRequestRepository.Add(newLeaveRequest);

            return new ServiceResult<LeaveRequest>
            {
                Success = true,
                Data = newLeaveRequest
            };
        }

        public Task<ServiceResult<List<LeaveRequest>>> GetMyLeaveRequests(int userId)
        {
            var myLeaveRequests = _leaveRequestRepository.GetByUserId(userId);

            return Task.FromResult(new ServiceResult<List<LeaveRequest>>
            {
                Success = true,
                Data = myLeaveRequests
            });
        }

        public async Task<ServiceResult<List<LeaveRequestWithUserDto>>> GetTeamLeaveRequests(int managerId)
        {
            var manager = await _userManager.FindByIdAsync(managerId.ToString());

            if (manager == null)
            {
                return new ServiceResult<List<LeaveRequestWithUserDto>>
                {
                    Success = false,
                    Message = "Yönetici bulunamadı."
                };
            }

            var teamLeaveRequests = _leaveRequestRepository.GetByDepartmentId(manager.DepartmentId);

            var resultDtos = teamLeaveRequests.Select(lr => new LeaveRequestWithUserDto
            {
                Id = lr.Id,
                UserId = lr.UserId,
                UserFullName = lr.User != null ? $"{lr.User.FirstName} {lr.User.LastName}" : "-",
                LeaveTypeId = lr.LeaveTypeId,
                StartDate = lr.StartDate,
                EndDate = lr.EndDate,
                Description = lr.Description,
                Status = lr.Status,
                ApprovedByUserId = lr.ApprovedByUserId,
                DecisionDate = lr.DecisionDate,
            }).ToList();

            return new ServiceResult<List<LeaveRequestWithUserDto>>
            {
                Success = true,
                Data = resultDtos
            };
        }

        public Task<ServiceResult<LeaveRequest>> UpdateLeaveRequestStatus(int leaveRequestId, int managerId, LeaveStatus newStatus)
        {
            var leaveRequest = _leaveRequestRepository.GetById(leaveRequestId);

            if (leaveRequest == null)
            {
                return Task.FromResult(new ServiceResult<LeaveRequest>
                {
                    Success = false,
                    Message = "Talep Bulunamadı"
                });
            }

            leaveRequest.Status = newStatus;
            leaveRequest.ApprovedByUserId = managerId;
            leaveRequest.DecisionDate = DateTime.Now;
            _leaveRequestRepository.Update(leaveRequest);

            return Task.FromResult(new ServiceResult<LeaveRequest>
            {
                Success = true,
                Data = leaveRequest
            });
        }

        public Task<ServiceResult<List<LeaveTypeSummaryDto>>> GetMyLeaveSummary(int userId)
        {
            var myLeaveRequests = _leaveRequestRepository.GetByUserId(userId);

            var approvedRequests = myLeaveRequests
                .Where(lr => lr.Status == LeaveStatus.Approved)
                .ToList();

            var groupedByType = approvedRequests
                .GroupBy(lr => lr.LeaveTypeId)
                .Select(g => new
                {
                    LeaveTypeId = g.Key,
                    TotalDays = g.Sum(lr => (lr.EndDate - lr.StartDate).Days + 1)
                })
                .ToList();

            var resultDtos = new List<LeaveTypeSummaryDto>();

            foreach (var group in groupedByType)
            {
                var leaveType = _leaveTypeRepository.GetById(group.LeaveTypeId);
                resultDtos.Add(new LeaveTypeSummaryDto
                {
                    LeaveTypeName = leaveType != null ? leaveType.Name : "-",
                    TotalDays = group.TotalDays
                });
            }

            return Task.FromResult(new ServiceResult<List<LeaveTypeSummaryDto>>
            {
                Success = true,
                Data = resultDtos
            });
        }

        public Task<ServiceResult<AnnualLeaveSummaryDto>> GetMyAnnualSummary(int userId)
        {
            const int annualQuota = 14; // Şimdilik sabit; ileride kullanıcı bazlı/admin ayarlı kota olabilir

            var myLeaveRequests = _leaveRequestRepository.GetByUserId(userId);

            var allLeaveTypes = _leaveTypeRepository.GetAll();
            var annualLeaveType = allLeaveTypes.FirstOrDefault(lt => lt.Name == "Yıllık İzin");

            int usedDays = 0;

            if (annualLeaveType != null)
            {
                usedDays = myLeaveRequests
                    .Where(lr => lr.Status == LeaveStatus.Approved && lr.LeaveTypeId == annualLeaveType.Id)
                    .Sum(lr => (lr.EndDate - lr.StartDate).Days + 1);
            }

            var summary = new AnnualLeaveSummaryDto
            {
                UsedDays = usedDays,
                RemainingDays = Math.Max(annualQuota - usedDays, 0),
                TotalQuota = annualQuota
            };

            return Task.FromResult(new ServiceResult<AnnualLeaveSummaryDto>
            {
                Success = true,
                Data = summary
            });
        }

        public Task<ServiceResult<List<TeamLeaveBarChartDto>>> GetTeamLeaveBarChart(int managerId)
        {
            var manager = _userManager.FindByIdAsync(managerId.ToString()).Result;

            if (manager == null)
            {
                return Task.FromResult(new ServiceResult<List<TeamLeaveBarChartDto>>
                {
                    Success = false,
                    Message = "Yönetici bulunamadı."
                });
            }

            var teamLeaveRequests = _leaveRequestRepository.GetByDepartmentId(manager.DepartmentId);

            var currentYear = DateTime.Now.Year;

            var approvedThisYear = teamLeaveRequests
                .Where(lr => lr.Status == LeaveStatus.Approved && lr.StartDate.Year == currentYear)
                .ToList();

            var groupedByUser = approvedThisYear.GroupBy(lr => lr.UserId);

            var resultDtos = new List<TeamLeaveBarChartDto>();

            foreach (var userGroup in groupedByUser)
            {
                var user = userGroup.First().User;

                var groupedByType = userGroup
                    .GroupBy(lr => lr.LeaveTypeId)
                    .Select(g => new
                    {
                        LeaveTypeId = g.Key,
                        TotalDays = g.Sum(lr => (lr.EndDate - lr.StartDate).Days + 1)
                    })
                    .ToList();

                var breakdown = new List<LeaveTypeSummaryDto>();

                foreach (var typeGroup in groupedByType)
                {
                    var leaveType = _leaveTypeRepository.GetById(typeGroup.LeaveTypeId);
                    breakdown.Add(new LeaveTypeSummaryDto
                    {
                        LeaveTypeName = leaveType != null ? leaveType.Name : "-",
                        TotalDays = typeGroup.TotalDays
                    });
                }

                resultDtos.Add(new TeamLeaveBarChartDto
                {
                    UserFullName = user != null ? $"{user.FirstName} {user.LastName}" : "-",
                    LeaveBreakdown = breakdown
                });
            }

            return Task.FromResult(new ServiceResult<List<TeamLeaveBarChartDto>>
            {
                Success = true,
                Data = resultDtos
            });
        }

        public Task<ServiceResult<List<MonthlyLeaveCountDto>>> GetMonthlyLeaveCount(int managerId)
        {
            var manager = _userManager.FindByIdAsync(managerId.ToString()).Result;

            if (manager == null)
            {
                return Task.FromResult(new ServiceResult<List<MonthlyLeaveCountDto>>
                {
                    Success = false,
                    Message = "Yönetici bulunamadı."
                });
            }

            var teamLeaveRequests = _leaveRequestRepository.GetByDepartmentId(manager.DepartmentId);

            var currentYear = DateTime.Now.Year;

            var approvedThisYear = teamLeaveRequests
                .Where(lr => lr.Status == LeaveStatus.Approved && lr.StartDate.Year == currentYear)
                .ToList();

            var monthNames = new[]
            {
                "Ocak", "Şubat", "Mart", "Nisan", "Mayıs", "Haziran",
                "Temmuz", "Ağustos", "Eylül", "Ekim", "Kasım", "Aralık"
            };

            var resultDtos = new List<MonthlyLeaveCountDto>();

            for (int month = 1; month <= 12; month++)
            {
                var count = approvedThisYear.Count(lr => lr.StartDate.Month == month);

                resultDtos.Add(new MonthlyLeaveCountDto
                {
                    Month = month,
                    MonthName = monthNames[month - 1],
                    TotalRequests = count
                });
            }

            return Task.FromResult(new ServiceResult<List<MonthlyLeaveCountDto>>
            {
                Success = true,
                Data = resultDtos
            });
        }
    }
}
