using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademicFinals.API.DTOs;
using Microsoft.AspNetCore.Identity;

namespace AcademicFinals.API.Services
{
    public interface IAuthService
    {
        Task<IdentityResult> RegisterUser(RegisterDto dto);
        Task<TokenDto?> Login(LoginDto dto);
    }
}