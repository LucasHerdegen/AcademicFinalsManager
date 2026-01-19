using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademicFinals.API.DTOs;
using AcademicFinals.API.Models;
using AcademicFinals.API.Repository;
using AutoMapper;

namespace AcademicFinals.API.Services
{
    public class ExamDateService : IExamDateService
    {
        private readonly IRepository<ExamDate> _examDateRepository;
        private readonly IMapper _mapper;

        public ExamDateService(IRepository<ExamDate> examDateRepository, IMapper mapper)
        {
            _examDateRepository = examDateRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ExamDateDto>> GetExamDates()
        {
            var examDates = await _examDateRepository.Get();
            var examDatesDto = _mapper.Map<IEnumerable<ExamDateDto>>(examDates);

            return examDatesDto;
        }

        public async Task<ExamDateDto?> GetExamDate(int id)
        {
            var examDate = await _examDateRepository.GetById(id);

            if (examDate == null)
                return null;

            var examDateDto = _mapper.Map<ExamDateDto>(examDate);

            return examDateDto;
        }

        public async Task<ExamDateDto?> CreateExamDate(ExamDatePostDto dto)
        {
            var exist = await _examDateRepository.Exists(e => e.Date == dto.Date && dto.SubjectId == e.SubjectId);

            if (exist)
                return null;

            var exam = _mapper.Map<ExamDate>(dto);

            await _examDateRepository.Create(exam);
            await _examDateRepository.Save();

            var newExam = await _examDateRepository.GetById(exam.Id);
            var examDto = _mapper.Map<ExamDateDto>(newExam);

            return examDto;
        }

        public async Task<bool> UpdateExamDate(ExamDatePutDto dto)
        {
            var examDate = await _examDateRepository.GetById(dto.Id);

            if (examDate == null)
                return false;

            _mapper.Map(dto, examDate);

            _examDateRepository.Update(examDate);
            await _examDateRepository.Save();

            return true;
        }

        public async Task<bool> DeleteExamDate(int id)
        {
            var examDate = await _examDateRepository.GetById(id);

            if (examDate == null)
                return false;

            _examDateRepository.Delete(examDate);
            await _examDateRepository.Save();

            return true;
        }
    }
}