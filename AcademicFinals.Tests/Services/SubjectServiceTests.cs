using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Xunit;
using AcademicFinals.API.Models;
using AcademicFinals.API.Repository;
using AutoMapper;
using AcademicFinals.API.Services;
using FluentAssertions;
using AcademicFinals.API.DTOs;

namespace AcademicFinals.Tests.Services
{
    public class SubjectServiceTests
    {
        private readonly Mock<IRepository<Subject>> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;

        private readonly SubjectService _service;

        public SubjectServiceTests()
        {
            _mockRepo = new Mock<IRepository<Subject>>();
            _mockMapper = new Mock<IMapper>();

            _service = new SubjectService(_mockRepo.Object, _mockMapper.Object);
        }


        [Fact]
        public async Task GetSubject_DeberiaRetornalNull_CuandoLaMateria_NoExiste()
        {
            // arrange
            int id = 10;
            _mockRepo.Setup(repo => repo.GetById(id)).ReturnsAsync((Subject?)null);

            // act
            var result = await _service.GetSubject(id);

            // assert
            result.Should().BeNull();
            _mockRepo.Verify(repo => repo.GetById(id), Times.Once);
        }

        [Fact]
        public async Task GetSubject_DeberiaRetornarUnaMateria_CuandoLaMateriaExiste()
        {
            //arrange
            int id = 10;
            var subject = new Subject
            {
                Name = "Matemáticas",
                Id = id
            };
            var subjectDto = new SubjectDto
            {
                Name = subject.Name,
                Id = subject.Id
            };
            _mockRepo.Setup(repo => repo.GetById(id)).ReturnsAsync(subject);
            _mockMapper.Setup(mapper => mapper.Map<SubjectDto>(subject)).Returns(subjectDto);

            // act
            var result = await _service.GetSubject(id);

            // assert
            result.Should().NotBeNull();
            result.Should().Be(subjectDto);
            _mockRepo.Verify(repo => repo.GetById(id), Times.Once);
            _mockMapper.Verify(mapper => mapper.Map<SubjectDto>(subject), Times.Once);
        }
    }
}