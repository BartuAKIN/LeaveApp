using System;
using System.Collections.Generic;
using System.Text;
using LeaveApp.Business.Common;
using LeaveApp.Business.DTOs;
using LeaveApp.Business.Interfaces;
using LeaveApp.Data.Repositories;
using LeaveApp.Domain.Entities;

namespace LeaveApp.Business.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentService(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public Task<ServiceResult<Department>> CreateDepartment(CreateDepartmentDto createDepartmentDto)
        {
            var newDepartment = new Department
            {
                Name = createDepartmentDto.Name,
                CreatedDate = DateTime.UtcNow
            };

            _departmentRepository.Add(newDepartment);

            return Task.FromResult(new ServiceResult<Department>
            {
                Success = true,
                Data = newDepartment
            });
        }

        public Task<ServiceResult<bool>> DeleteDepartment(int departmentId)
        {
            var department = _departmentRepository.GetById(departmentId);

            if (department == null)
            {
                return Task.FromResult(new ServiceResult<bool>
                {
                    Success = false,
                    Message = "Departman bulunamadı."
                });
            }

            _departmentRepository.Delete(departmentId);
            return Task.FromResult(new ServiceResult<bool>
            {
                Success = true,
                Message = $"{department.Name} departmanı silindi.",
                Data = true
            });
        }

        public Task<ServiceResult<List<Department>>> GetAllDepartments()
        {
            var departments = _departmentRepository.GetAll();

            return Task.FromResult(new ServiceResult<List<Department>>
                {
                Success = true,
                Data = departments
                });
        }

        public Task<ServiceResult<Department>> GetDepartmentById(int departmentId)
        {
            var department = _departmentRepository.GetById(departmentId);

            if (department == null) 
            {
                return Task.FromResult(new ServiceResult<Department>
                {
                    Success = false,
                    Message = "Departman bulunamadı."
                });
            }

            return Task.FromResult(new ServiceResult<Department>
            {
                Success = true,
                Data = department
            });
        }

        public Task<ServiceResult<Department>> UpdateDepartment(int departmentId, CreateDepartmentDto createDepartmentDto)
        {
            var department = _departmentRepository.GetById(departmentId);
            

            if (department == null)
            {
                return Task.FromResult(new ServiceResult<Department>
                { 
                    Success = false,
                    Message = "Departman bulunamadı."
                });
            }

            department.Name = createDepartmentDto.Name;

            _departmentRepository.Update(department);

            return Task.FromResult(new ServiceResult<Department>
            {
                Success = true,
                Data = department
            });
        }
    }
}
