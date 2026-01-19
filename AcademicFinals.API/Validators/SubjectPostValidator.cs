using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademicFinals.API.DTOs;
using FluentValidation;

namespace AcademicFinals.API.Validators
{
    public class SubjectPostValidator : AbstractValidator<SubjectPostDto>
    {
        public SubjectPostValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("The name is required");
        }
    }
}