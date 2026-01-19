using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademicFinals.API.Models;

namespace AcademicFinals.API.DTOs
{
    public class ExamDateDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int MaxCapacity { get; set; }
        public Subject? Subject { get; set; }
        public int EnrollmentsCount { get; set; }
    }
}