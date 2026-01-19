using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace AcademicFinalsSpaceManager.API.DTOs
{
    public class TokenDto
    {
        public required string Token { get; set; }
        public required DateTime ValidTo { get; set; }
    }
}