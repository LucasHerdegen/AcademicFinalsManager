using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademicFinalsSpaceManager.API.DTOs;

namespace AcademicFinalsSpaceManager.API.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>?> GetUsers();

        Task<UserDto?> GetUserByEmail(string email);
    }
}