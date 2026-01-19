using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademicFinals.API.DTOs;
using FluentValidation;

namespace AcademicFinals.API.Validators
{
    public class SubjectPutValidator : AbstractValidator<SubjectPutDto>
    {
        public SubjectPutValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("The name field is required");
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("The id have to be greater than zero");
        }
    }
}