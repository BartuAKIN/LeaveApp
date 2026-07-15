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

        public LeaveRequestService(ILeaveRequestRepository leaveRequestRepository, UserManager<ApplicationUser> userManager)
        {
            _leaveRequestRepository = leaveRequestRepository;
            _userManager = userManager;
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
    }
}
