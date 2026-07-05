using System;
using System.Collections.Generic;
using System.Text;
using LeaveApp.Business.Common;
using LeaveApp.Business.DTOs;
using LeaveApp.Domain.Entities;
using LeaveApp.Domain.Enums;
using System.Threading.Tasks;

namespace LeaveApp.Business.Interfaces
{
    public interface ILeaveRequestService
    {
        Task<ServiceResult<LeaveRequest>> CreateLeaveRequest(int userId, CreateLeaveRequestDto createLeaveRequestDto);

        Task<ServiceResult<List<LeaveRequest>>> GetMyLeaveRequests(int userId);

        Task<ServiceResult<List<LeaveRequest>>> GetTeamLeaveRequests(int managerId);

        Task<ServiceResult<LeaveRequest>> UpdateLeaveRequestStatus(int leaveRequestId, int managerId, LeaveStatus newStatus);
    }
}
