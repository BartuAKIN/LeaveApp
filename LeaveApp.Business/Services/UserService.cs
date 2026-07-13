using LeaveApp.Business.Common;
using LeaveApp.Business.DTOs;
using LeaveApp.Business.Interfaces;
using LeaveApp.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace LeaveApp.Business.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ServiceResult<List<UserDto>>> GetAllUsers()
        {
            var users = _userManager.Users.ToList();

            var userDtos = users.Select(u => new UserDto
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                DepartmentId = u.DepartmentId,
                IsManager = u.IsManager,
                IsActive = u.IsActive
            }).ToList();

            return new ServiceResult<List<UserDto>>
            {
                Success = true,
                Data = userDtos
            };
        }

        public async Task<ServiceResult<UserDto>> GetUserById(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                return new ServiceResult<UserDto>
                {
                    Success = false,
                    Message = "Personel bulunamadı."
                };
            }

            var userDto = new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                DepartmentId = user.DepartmentId,
                IsManager = user.IsManager,
                IsActive = user.IsActive
            };

            return new ServiceResult<UserDto>
            {
                Success = true,
                Data = userDto
            };
        }

        public async Task<ServiceResult<UserDto>> CreateUser(CreateUserDto createUserDto)
        {
            var newUser = new ApplicationUser
            {
                UserName = createUserDto.Email,
                Email = createUserDto.Email,
                FirstName = createUserDto.FirstName,
                LastName = createUserDto.LastName,
                DepartmentId = createUserDto.DepartmentId,
                IsManager = createUserDto.IsManager,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(newUser, createUserDto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return new ServiceResult<UserDto>
                {
                    Success = false,
                    Message = errors
                };
            }

            await _userManager.AddToRoleAsync(newUser, "User");

            var userDto = new UserDto
            {
                Id = newUser.Id,
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                Email = newUser.Email,
                DepartmentId = newUser.DepartmentId,
                IsManager = newUser.IsManager,
                IsActive = newUser.IsActive
            };

            return new ServiceResult<UserDto>
            {
                Success = true,
                Data = userDto
            };
        }

        public async Task<ServiceResult<UserDto>> UpdateUser(int userId, UpdateUserDto updateUserDto)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                return new ServiceResult<UserDto>
                {
                    Success = false,
                    Message = "Personel bulunamadı."
                };
            }

            user.FirstName = updateUserDto.FirstName;
            user.LastName = updateUserDto.LastName;
            user.DepartmentId = updateUserDto.DepartmentId;
            user.IsManager = updateUserDto.IsManager;
            user.IsActive = updateUserDto.IsActive;

            await _userManager.UpdateAsync(user);

            var userDto = new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                DepartmentId = user.DepartmentId,
                IsManager = user.IsManager,
                IsActive = user.IsActive
            };

            return new ServiceResult<UserDto>
            {
                Success = true,
                Data = userDto
            };
        }

        public async Task<ServiceResult<bool>> DeleteUser(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                return new ServiceResult<bool>
                {
                    Success = false,
                    Message = "Personel bulunamadı."
                };
            }

            user.IsActive = false;
            await _userManager.UpdateAsync(user);

            return new ServiceResult<bool>
            {
                Success = true,
                Message = $"{user.FirstName} {user.LastName} pasif edildi."
            };
        }
    }
}