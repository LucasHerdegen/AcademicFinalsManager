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
    public class SubjectService : ISubjectService
    {
        private readonly IRepository<Subject> _subjectRepository;
        private readonly IMapper _mapper;

        public SubjectService(IRepository<Subject> subjectRepository, IMapper mapper)
        {
            _subjectRepository = subjectRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SubjectDto>?> GetSubjects()
        {
            var subjects = await _subjectRepository.Get();
            var subjectsDto = _mapper.Map<IEnumerable<SubjectDto>>(subjects);

            return subjectsDto;
        }

        public async Task<SubjectDto?> GetSubject(int id)
        {
            var subject = await _subjectRepository.GetById(id);

            if (subject == null)
                return null;

            var subjectDto = _mapper.Map<SubjectDto>(subject);

            return subjectDto;
        }

        public async Task<SubjectDto?> CreateSubject(SubjectPostDto dto)
        {
            var exist = await _subjectRepository.Exists(s => s.Name.ToUpper() == dto.Name!.ToUpper());

            if (exist)
                return null;

            var subject = _mapper.Map<Subject>(dto);

            await _subjectRepository.Create(subject);
            await _subjectRepository.Save();

            var newSubject = await _subjectRepository.GetById(subject.Id);

            var subjectDto = _mapper.Map<SubjectDto>(newSubject);

            return subjectDto;
        }

        public async Task<bool> UpdateSubject(SubjectPutDto dto)
        {
            var subject = await _subjectRepository.GetById(dto.Id);

            if (subject == null)
                return false;

            _mapper.Map(dto, subject);
            _subjectRepository.Update(subject);
            await _subjectRepository.Save();

            return true;
        }

        public async Task<bool> DeleteSubject(int id)
        {
            var subject = await _subjectRepository.GetById(id);

            if (subject == null)
                return false;

            _subjectRepository.Delete(subject);
            await _subjectRepository.Save();

            return true;
        }
    }
}