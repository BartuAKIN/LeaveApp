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
    public class LeaveTypeService : ILeaveTypeService
    {
        private readonly ILeaveTypeRepository _leaveTypeRepository;

        public LeaveTypeService(ILeaveTypeRepository leaveTypeRepository)
        {
            _leaveTypeRepository = leaveTypeRepository;
        }

        public Task<ServiceResult<LeaveType>> CreateLeaveType(CreateLeaveTypeDto createLeaveTypeDto)
        {
            var newLeaveType = new LeaveType
            {
                Name = createLeaveTypeDto.Name,
                Description = createLeaveTypeDto.Description,
                CreatedDate = DateTime.UtcNow
            };

            _leaveTypeRepository.Add(newLeaveType);

            return Task.FromResult(new ServiceResult<LeaveType>
            {
                Success = true,
                Data = newLeaveType
            });
        }

        public Task<ServiceResult<bool>> DeleteLeaveType(int leaveTypeId)
        {
            var leaveType = _leaveTypeRepository.GetById(leaveTypeId);

            if (leaveType == null)
            {
                return Task.FromResult(new ServiceResult<bool>
                {
                    Success = false,
                    Message = "İzin türü bulunamadı."
                });
            }

            _leaveTypeRepository.Delete(leaveTypeId);
            return Task.FromResult(new ServiceResult<bool>
            {
                Success = true,
                Message = $"{leaveType.Name} izin türü silindi."
            });
        }

        public Task<ServiceResult<List<LeaveType>>> GetAllLeaveTypes()
        {
            var leaveTypes = _leaveTypeRepository.GetAll();

            return Task.FromResult(new ServiceResult<List<LeaveType>>
            { 
                Success = true,
                Data = leaveTypes
            });
        }

        public Task<ServiceResult<LeaveType>> GetLeaveTypeById(int leaveTypeId)
        {
            var leaveType = _leaveTypeRepository.GetById(leaveTypeId);

            if (leaveType == null)
            {
                return Task.FromResult(new ServiceResult<LeaveType>
                {
                    Success = false,
                    Message = "İzin türü bulunamadı."
                });
            }

            return Task.FromResult(new ServiceResult<LeaveType>
            {
                Success = true,
                Data = leaveType
            });
        }

        public Task<ServiceResult<LeaveType>> UpdateLeaveType(int leaveTypeId, CreateLeaveTypeDto createLeaveTypeDto)
        {
            var leaveType = _leaveTypeRepository.GetById(leaveTypeId);

            if (leaveType == null)
            {
                return Task.FromResult(new ServiceResult<LeaveType>
                { 
                    Success = false,
                    Message = "İzin türü bulunamadı."
                });
            }

            leaveType.Name = createLeaveTypeDto.Name;
            leaveType.Description = createLeaveTypeDto.Description;

            _leaveTypeRepository.Update(leaveType);

            return Task.FromResult(new ServiceResult<LeaveType>
            {
                Success = true,
                Data = leaveType
            });
        }
        
    }
}
