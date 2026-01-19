using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademicFinals.API.DTOs
{
    public class SubjectDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
    }
}