using LeaveApp.Business.DTOs;
using LeaveApp.Business.Interfaces;
using LeaveApp.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;


namespace LeaveApp.Business.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly ITokenService _tokenService;

        public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ITokenService tokenService)
        {
            _userManager = userManager;

            _signInManager = signInManager;

            _tokenService = tokenService;
        }
        public async Task<string> Login(LoginDto loginDto)
        { 
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null) return null;
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded) return null;
            var roles = await _userManager.GetRolesAsync(user);
            return _tokenService.GenerateToken(user, roles);
        }
        public async Task<bool> Register(RegisterDto registerDto)
        {
            var user = new ApplicationUser
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                DepartmentId = registerDto.DepartmentId,
                Email = registerDto.Email,
                UserName = registerDto.Email
            };
            var result = await _userManager.CreateAsync(user, registerDto.Password);

                if (result.Succeeded)
                {
                 await _userManager.AddToRoleAsync(user, "Employee");
                }

            return result.Succeeded;
        }
    }
}
