using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademicFinals.API.DTOs;

namespace AcademicFinals.API.Services
{
    public interface ISubjectService
    {
        public Task<IEnumerable<SubjectDto>?> GetSubjects();
        public Task<SubjectDto?> GetSubject(int id);
        public Task<SubjectDto> CreateSubject(SubjectPostDto dto);
        public Task<bool> UpdateSubject(SubjectPutDto dto);
        public Task<bool> DeleteSubject(int id);
    }
}