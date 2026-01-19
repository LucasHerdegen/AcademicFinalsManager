using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademicFinals.API.DTOs;
using FluentValidation;

namespace AcademicFinals.API.Validators
{
    public class ExamDatePutValidator : AbstractValidator<ExamDatePutDto>
    {
        public ExamDatePutValidator()
        {
            RuleFor(x => x.Date).GreaterThan(DateTime.UtcNow).WithMessage("The exam date has to be in the future");
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("The id must be greater than 0");
            RuleFor(x => x.MaxCapacity).GreaterThan(0).WithMessage("The capacity must be greater than 0");
        }
    }
}