using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AcademicFinals.API.DTOs;
using AcademicFinals.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcademicFinals.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EnrollmentController : ControllerBase
    {
        private readonly IEnrollmentService _enrollmentService;

        public EnrollmentController(IEnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EnrollmentDto>?>> GetEnrollments()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized("The user could not be identified");

            var enrollments = await _enrollmentService.GetEnrollments(userId);

            return Ok(enrollments);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EnrollmentDto>> GetEnrollment(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized("The user could not be identified");

            var enrollment = await _enrollmentService.GetEnrollment(userId, id);

            if (enrollment == null)
                return NotFound();

            return Ok(enrollment);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<EnrollmentDto>?>> GetAllEnrollments()
        {
            var enrollments = await _enrollmentService.GetEnrollments();
            return Ok(enrollments);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEnrollment(int examDateId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized("The user could not be identified");

            var enrollment = await _enrollmentService.CreateEnrollment(userId, examDateId);

            if (enrollment == null)
                return Conflict();

            return CreatedAtAction(nameof(GetEnrollment), new { Id = enrollment.Id }, enrollment);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteEnrollment(int enrollmentId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized("The user could not be identified");

            var result = await _enrollmentService.DeleteEnrollment(userId, enrollmentId);

            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}