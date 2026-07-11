using LeaveApp.Business.DTOs;
using LeaveApp.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LeaveApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LeaveTypeController : ControllerBase
    {
        private readonly ILeaveTypeService _leaveTypeService;

        public LeaveTypeController(ILeaveTypeService leaveTypeService)
        {
            _leaveTypeService = leaveTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllLeaveTypes()
        {
            var result = await _leaveTypeService.GetAllLeaveTypes();

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLeaveTypeId(int id)
        {
            var result = await _leaveTypeService.GetLeaveTypeById(id);

            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateLeaveType([FromBody] CreateLeaveTypeDto createLeaveTypeDto)
        {
            var result = await _leaveTypeService.CreateLeaveType(createLeaveTypeDto);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateLeaveType(int id, [FromBody] CreateLeaveTypeDto createLeaveTypeDto)
        {
            var result = await _leaveTypeService.UpdateLeaveType(id, createLeaveTypeDto);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteLeaveType(int id)
        {
            var result = await _leaveTypeService.DeleteLeaveType(id);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
    
}
