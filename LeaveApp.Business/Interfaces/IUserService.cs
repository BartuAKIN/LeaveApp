using LeaveApp.Business.Common;
using LeaveApp.Business.DTOs;

namespace LeaveApp.Business.Interfaces
{
    public interface IUserService
    {
        Task<ServiceResult<List<UserDto>>> GetAllUsers();
        Task<ServiceResult<UserDto>> GetUserById(int userId);
        Task<ServiceResult<UserDto>> CreateUser(CreateUserDto createUserDto);
        Task<ServiceResult<UserDto>> UpdateUser(int userId, UpdateUserDto updateUserDto);
        Task<ServiceResult<bool>> DeleteUser(int userId);
    }
}