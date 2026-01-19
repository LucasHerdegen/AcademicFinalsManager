using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademicFinals.API.DTOs;
using FluentValidation;

namespace AcademicFinals.API.Validators
{
    public class ExamDatePostValidator : AbstractValidator<ExamDatePostDto>
    {
        public ExamDatePostValidator()
        {
            RuleFor(x => x.Date).GreaterThan(DateTime.UtcNow).WithMessage("The exam date has to be in the future");
            RuleFor(x => x.MaxCapacity).GreaterThan(0).WithMessage("The capacity must be greater than zero");
            RuleFor(x => x.SubjectId).GreaterThan(0).WithMessage("The subject id must be greater than 0");
        }
    }
}