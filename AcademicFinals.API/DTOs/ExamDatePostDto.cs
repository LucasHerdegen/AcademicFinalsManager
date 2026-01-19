using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademicFinals.API.DTOs
{
    public class ExamDatePostDto
    {
        public DateTime Date { get; set; }
        public int MaxCapacity { get; set; }
        public int SubjectId { get; set; }
    }
}