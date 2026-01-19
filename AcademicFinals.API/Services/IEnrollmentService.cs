using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademicFinals.API.DTOs;

namespace AcademicFinals.API.Services
{
    public interface IEnrollmentService
    {
        Task<IEnumerable<EnrollmentDto>?> GetEnrollments(string userId);
        Task<EnrollmentDto?> GetEnrollment(string userId, int id);
        Task<IEnumerable<EnrollmentDto>?> GetEnrollments();
        Task<EnrollmentDto?> CreateEnrollment(string userId, int examDateId);
        Task<bool> DeleteEnrollment(string userId, int enrollmentId);
    }
}