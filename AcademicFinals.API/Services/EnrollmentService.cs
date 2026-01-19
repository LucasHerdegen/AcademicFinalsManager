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
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IRepository<Enrollment> _enrollmentRepository;
        private readonly IRepository<ExamDate> _examDatesRepository;
        private readonly IMapper _mapper;

        public EnrollmentService(IRepository<Enrollment> enrollmentRepository,
            IRepository<ExamDate> examDatesRepository,
            IMapper mapper)
        {
            _enrollmentRepository = enrollmentRepository;
            _examDatesRepository = examDatesRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EnrollmentDto>?> GetEnrollments(string userId)
        {
            var enrollments = await _enrollmentRepository.Get(x => x.StudentId == userId);
            var enrollmentsDto = _mapper.Map<IEnumerable<EnrollmentDto>>(enrollments);

            return enrollmentsDto;
        }

        public async Task<EnrollmentDto?> GetEnrollment(string userId, int id)
        {
            var enrollments = await _enrollmentRepository.Get(x => x.StudentId == userId);
            var enrollment = enrollments?.FirstOrDefault(x => x.Id == id);

            if (enrollment == null)
                return null;

            var enrollmentDto = _mapper.Map<EnrollmentDto>(enrollment);

            return enrollmentDto;
        }

        public async Task<IEnumerable<EnrollmentDto>?> GetEnrollments()
        {
            var enrollments = await _enrollmentRepository.Get();
            var enrollmentsDto = _mapper.Map<IEnumerable<EnrollmentDto>>(enrollments);

            return enrollmentsDto;
        }

        public async Task<EnrollmentDto?> CreateEnrollment(string userId, int examDateId)
        {
            var examDate = await _examDatesRepository.GetById(examDateId);

            if (examDate == null)
                return null;

            var potentialEnrollments = (examDate.Enrollments == null ? 0 : examDate.Enrollments.Count) + 1;

            var currentEnrollments = await _enrollmentRepository.Count(e => e.ExamDateId == examDateId);

            if (currentEnrollments >= examDate.MaxCapacity)
                return null;


            var existEnrollment = await _enrollmentRepository.Exists(e => e.StudentId == userId && e.ExamDateId == examDateId);

            if (existEnrollment)
                return null;

            var enrollment = new Enrollment
            {
                StudentId = userId,
                ExamDateId = examDateId,
            };

            if (examDate.Enrollments == null)
                examDate.Enrollments = [];

            examDate.Enrollments.Add(enrollment);

            await _enrollmentRepository.Create(enrollment);
            await _enrollmentRepository.Save();

            var newEnrollment = await _enrollmentRepository.GetById(enrollment.Id);
            var enrollmentDto = _mapper.Map<EnrollmentDto>(newEnrollment);

            return enrollmentDto;
        }

        public async Task<bool> DeleteEnrollment(string userId, int enrollmentId)
        {
            var enrollment = await _enrollmentRepository.Find(x => x.StudentId == userId && x.Id == enrollmentId);

            if (enrollment == null)
                return false;

            _enrollmentRepository.Delete(enrollment);
            await _enrollmentRepository.Save();

            return true;
        }
    }
}