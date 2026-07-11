using System;
using System.Collections.Generic;
using System.Text;
using LeaveApp.Business.Common;
using LeaveApp.Business.DTOs;
using LeaveApp.Domain.Entities;

namespace LeaveApp.Business.Interfaces
{
    public interface ILeaveTypeService
    {
        Task<ServiceResult<LeaveType>> CreateLeaveType(CreateLeaveTypeDto createLeaveTypeDto);
        Task<ServiceResult<bool>> DeleteLeaveType(int leaveTypeId);
        Task<ServiceResult<List<LeaveType>>> GetAllLeaveTypes();
        Task<ServiceResult<LeaveType>> GetLeaveTypeById(int leaveTypeId);
        Task<ServiceResult<LeaveType>> UpdateLeaveType(int leaveTypeId, CreateLeaveTypeDto createLeaveTypeDto);
    }
}
