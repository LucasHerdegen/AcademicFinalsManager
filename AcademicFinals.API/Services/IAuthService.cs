using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademicFinalsSpaceManager.API.DTOs;
using Microsoft.AspNetCore.Identity;

namespace AcademicFinalsSpaceManager.API.Services
{
    public interface IAuthService
    {
        Task<IdentityResult> RegisterUser(RegisterDto dto);
        Task<TokenDto?> Login(LoginDto dto);
    }
}