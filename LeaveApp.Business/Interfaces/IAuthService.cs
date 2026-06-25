using System;
using System.Collections.Generic;
using System.Text;
using LeaveApp.Business.Interfaces;
using LeaveApp.Business.DTOs;

namespace LeaveApp.Business.Interfaces
{
    public interface IAuthService
    {
        Task<string> Login(LoginDto loginDto);
        Task<bool> Register(RegisterDto registerDto);
    }
}
