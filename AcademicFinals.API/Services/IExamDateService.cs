using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademicFinals.API.DTOs;

namespace AcademicFinals.API.Services
{
    public interface IExamDateService
    {
        public Task<IEnumerable<ExamDateDto>> GetExamDates();
        public Task<ExamDateDto?> GetExamDate(int id);
        public Task<ExamDateDto?> CreateExamDate(ExamDatePostDto dto);
        public Task<bool> UpdateExamDate(ExamDatePutDto dto);
        public Task<bool> DeleteExamDate(int id);
    }
}