using System;
using System.Collections.Generic;
using System.Text;
using LeaveApp.Business.Common;
using LeaveApp.Business.DTOs;
using LeaveApp.Domain.Entities;
using LeaveApp.Domain.Enums;

namespace LeaveApp.Business.Interfaces
{
    public interface IDepartmentService
    {
        Task<ServiceResult<List<Department>>> GetAllDepartments();

        Task<ServiceResult<Department>> GetDepartmentById(int departmentId);

        Task<ServiceResult<Department>> CreateDepartment(CreateDepartmentDto createDepartmentDto);

        Task<ServiceResult<Department>> UpdateDepartment(int departmentId, CreateDepartmentDto createDepartmentDto);

        Task<ServiceResult<bool>> DeleteDepartment(int departmentId);
    }
}
