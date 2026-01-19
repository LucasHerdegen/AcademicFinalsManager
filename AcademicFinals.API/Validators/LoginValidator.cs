using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademicFinalsSpaceManager.API.DTOs;
using FluentValidation;

namespace AcademicFinalsSpaceManager.API.Validators
{
    public class LoginValidator : AbstractValidator<LoginDto>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("The email is required");
            RuleFor(x => x.Password).NotEmpty().WithMessage("The password is required");
        }
    }
}