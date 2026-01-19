using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademicFinals.API.Models
{
    public class ExamDate
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int MaxCapacity { get; set; }
        public int SubjectId { get; set; }
        public Subject? Subject { get; set; }
        public ICollection<Enrollment>? Enrollments { get; set; }
    }
}