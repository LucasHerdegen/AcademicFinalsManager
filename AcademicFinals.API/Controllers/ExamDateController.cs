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
    public class ExamDateController : ControllerBase
    {
        private readonly IExamDateService _examDateService;
        private readonly IValidator<ExamDatePostDto> _examDatePostValidator;
        private readonly IValidator<ExamDatePutDto> _examDatePutValidator;

        public ExamDateController(IExamDateService examDateService,
            IValidator<ExamDatePostDto> examDatePostValidator,
            IValidator<ExamDatePutDto> examDatePutValidator)
        {
            _examDateService = examDateService;
            _examDatePostValidator = examDatePostValidator;
            _examDatePutValidator = examDatePutValidator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExamDateDto>>> GetExamDates()
        {
            var examDates = await _examDateService.GetExamDates();
            return Ok(examDates);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ExamDateDto>> GetExamDate(int id)
        {
            if (id <= 0)
                return BadRequest("The id must be greater than zero");

            var examDate = await _examDateService.GetExamDate(id);

            if (examDate == null)
                return NotFound();

            return Ok(examDate);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateExamDate(ExamDatePostDto examDatePostDto)
        {
            var validation = await _examDatePostValidator.ValidateAsync(examDatePostDto);

            if (!validation.IsValid)
                return BadRequest(validation.Errors);

            var examDate = await _examDateService.CreateExamDate(examDatePostDto);

            if (examDate == null)
                return Conflict("Exam date already exist!");

            return CreatedAtAction(nameof(GetExamDate), new { Id = examDate.Id }, examDate);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> UpdateExamDate(ExamDatePutDto examDatePutDto)
        {
            var validation = await _examDatePutValidator.ValidateAsync(examDatePutDto);

            if (!validation.IsValid)
                return BadRequest(validation.Errors);

            var result = await _examDateService.UpdateExamDate(examDatePutDto);

            if (!result)
                return NotFound();

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExamDate(int id)
        {
            if (id <= 0)
                return BadRequest("The id must be greater than zero");

            var result = await _examDateService.DeleteExamDate(id);

            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}