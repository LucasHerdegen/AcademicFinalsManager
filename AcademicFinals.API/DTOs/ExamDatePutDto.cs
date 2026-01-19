using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademicFinals.API.DTOs
{
    public class ExamDatePutDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int MaxCapacity { get; set; }
    }
}