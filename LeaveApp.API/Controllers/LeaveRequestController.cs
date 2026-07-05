using LeaveApp.Business.Common;
using LeaveApp.Business.DTOs;
using LeaveApp.Business.Interfaces;
using LeaveApp.Domain.Entities;
using LeaveApp.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LeaveApp.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class LeaveRequestController : ControllerBase
    {
        private readonly ILeaveRequestService _leaveRequestService;

        public LeaveRequestController(ILeaveRequestService leaveRequestService)
        {
            _leaveRequestService = leaveRequestService;
        }



        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateLeaveRequest(CreateLeaveRequestDto createLeaveRequestDto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userId = int.Parse(userIdClaim);

            var result = await _leaveRequestService.CreateLeaveRequest(userId, createLeaveRequestDto);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Data);


        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetMyLeaveRequest()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userId = int.Parse(userIdClaim);

            var result = await _leaveRequestService.GetMyLeaveRequests(userId);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Data);


        }

        [HttpGet("team")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetTeamLeaveRequests()
        {
            var managerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int managerId = int.Parse(managerIdClaim);

            var result = await _leaveRequestService.GetTeamLeaveRequests(managerId);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Data);
        }

        [HttpPut("{leaveRequestId}/status")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> UpdateLeaveRequestStatus(int leaveRequestId, UpdateLeaveRequestStatusDto updateLeaveRequestStatusDto)
        {
            var managerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int managerId = int.Parse(managerIdClaim);

            var result = await _leaveRequestService.UpdateLeaveRequestStatus(leaveRequestId, managerId, updateLeaveRequestStatusDto.NewStatus);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Data);
        }
    }

}
