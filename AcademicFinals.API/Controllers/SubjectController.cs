using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademicFinals.API.DTOs;
using AcademicFinals.API.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcademicFinals.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectService _subjectService;
        private readonly IValidator<SubjectPostDto> _subjectPostValidator;
        private readonly IValidator<SubjectPutDto> _subjectPutValidator;

        public SubjectController(ISubjectService subjectService,
            IValidator<SubjectPostDto> subjectPostValidator,
            IValidator<SubjectPutDto> subjectPutValidator)
        {
            _subjectService = subjectService;
            _subjectPostValidator = subjectPostValidator;
            _subjectPutValidator = subjectPutValidator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubjectDto>?>> GetSubjects()
        {
            var subjects = await _subjectService.GetSubjects();
            return Ok(subjects);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SubjectDto?>> GetSubject(int id)
        {
            if (id <= 0)
                return BadRequest("The id must be greater than zero.");

            var subject = await _subjectService.GetSubject(id);

            if (subject == null)
                return NotFound();

            return Ok(subject);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateSubject(SubjectPostDto subjectPostDto)
        {
            var validation = _subjectPostValidator.Validate(subjectPostDto);

            if (!validation.IsValid)
                return BadRequest(validation.Errors);

            var subject = await _subjectService.CreateSubject(subjectPostDto);

            if (subject == null)
                return Conflict("The subject already exist");

            return CreatedAtAction(nameof(GetSubject), new { Id = subject.Id }, subject);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> UpdateSubject(SubjectPutDto subjectPutDto)
        {
            var validation = _subjectPutValidator.Validate(subjectPutDto);

            if (!validation.IsValid)
                return BadRequest(validation.Errors);

            var result = await _subjectService.UpdateSubject(subjectPutDto);

            if (!result)
                return NotFound();

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubject(int id)
        {
            if (id <= 0)
                return BadRequest();

            var result = await _subjectService.DeleteSubject(id);

            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}