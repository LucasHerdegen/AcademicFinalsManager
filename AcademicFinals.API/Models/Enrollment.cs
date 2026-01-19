using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademicFinals.API.Models
{
    public class Enrollment
    {
        public int Id { get; set; }
        public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;
        public required string StudentId { get; set; }
        public ApplicationUser? Student { get; set; }
        public int ExamDateId { get; set; }
        public ExamDate? ExamDate { get; set; }
    }
}