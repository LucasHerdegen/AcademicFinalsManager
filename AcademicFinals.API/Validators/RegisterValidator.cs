using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AcademicFinals.API.DTOs;
using FluentValidation;

namespace AcademicFinals.API.Validators
{
    public class RegisterValidator : AbstractValidator<RegisterDto>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("The first name must not be empty");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("The last name must not be empty");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Incorrect Email format");
            RuleFor(x => x.Password).MinimumLength(4).WithMessage("The password must be at least 4 characters longs");
        }
    }
}