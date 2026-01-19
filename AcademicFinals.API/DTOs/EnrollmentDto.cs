using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademicFinals.API.Models;

namespace AcademicFinals.API.DTOs
{
    public class EnrollmentDto
    {
        public int Id { get; set; }
        public DateTime EnrolledAt { get; set; }
        public ExamDate? ExamDate { get; set; }
    }
}