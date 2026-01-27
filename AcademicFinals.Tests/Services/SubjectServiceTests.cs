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

        [Fact]
        public async Task CreateSubject_DeberiaRetornarNull_SiLaMateriaYaExiste()
        {
            // arrange
            var dto = new SubjectPostDto
            {
                Name = "Matemáticas",
            };

            _mockRepo.Setup(repo => repo.Exists(s => s.Name.ToUpper() == dto.Name!.ToUpper()))
                .ReturnsAsync(true);

            // act
            var result = await _service.CreateSubject(dto);

            // assert
            result.Should().BeNull();
            _mockRepo.Verify(repo => repo.Exists(s => s.Name.ToUpper() == dto.Name!.ToUpper()), Times.Once);
        }

        [Fact]
        public async Task CreateSubject_DeberiaRetornar_UnaMateria_SiLaMateria_NoExiste()
        {
            // arrange
            int id = 10;
            string name = "Matemáticas";

            var dto = new SubjectPostDto
            {
                Name = name,
            };
            var subject = new Subject
            {
                Name = name,
                Id = id
            };
            var subjectDto = new SubjectDto
            {
                Name = name,
                Id = id,
            };

            _mockRepo.Setup(repo => repo.Exists(s => s.Name.ToUpper() == dto.Name!.ToUpper()))
                .ReturnsAsync(false);
            _mockMapper.Setup(mapper => mapper.Map<Subject>(dto))
                .Returns(subject);
            _mockRepo.Setup(repo => repo.GetById(id))
                .ReturnsAsync(subject);
            _mockMapper.Setup(mapper => mapper.Map<SubjectDto>(subject))
                .Returns(subjectDto);

            // act
            var result = await _service.CreateSubject(dto);

            // assert
            result.Should().NotBeNull();
            result.Should().Be(subjectDto);
            _mockRepo.Verify(repo => repo.Exists(s => s.Name.ToUpper() == dto.Name!.ToUpper()), Times.Once);
            _mockMapper.Verify(mapper => mapper.Map<Subject>(dto), Times.Once);
            _mockRepo.Verify(repo => repo.Create(subject), Times.Once);
            _mockRepo.Verify(repo => repo.Save(), Times.Once);
        }

        [Fact]
        public async Task UpdateSubject_DeberiaRetornarFalse_SiLaMateria_NoExiste()
        {
            // arrange
            int id = 10;
            string name = "Matemáticas";

            var dto = new SubjectPutDto
            {
                Id = id,
                Name = name,
            };

            _mockRepo.Setup(repo => repo.GetById(id))
                .ReturnsAsync((Subject?)null);

            // act
            var result = await _service.UpdateSubject(dto);

            // assert
            result.Should().BeFalse();
            _mockRepo.Verify(repo => repo.GetById(id), Times.Once);
        }

        [Fact]
        public async Task UpdateSubject_DeberiaRetornarTrue_SiLaMateriaExiste()
        {
            int id = 10;
            string name = "Matemáticas";

            var dto = new SubjectPutDto
            {
                Id = id,
                Name = name,
            };
            var subject = new Subject
            {
                Id = id,
                Name = name,
            };

            _mockRepo.Setup(repo => repo.GetById(id))
                .ReturnsAsync(subject);

            // act
            var result = await _service.UpdateSubject(dto);

            // assert
            result.Should().BeTrue();
            _mockRepo.Verify(repo => repo.GetById(id), Times.Once);
            _mockMapper.Verify(mapper => mapper.Map(dto, subject), Times.Once);
            _mockRepo.Verify(repo => repo.Update(subject), Times.Once);
            _mockRepo.Verify(repo => repo.Save(), Times.Once);
        }

        [Fact]
        public async Task DeleteSubject_DeberiaRetornarFalse_SiLaMateria_NoExiste()
        {
            // arrange
            int id = 10;

            _mockRepo.Setup(repo => repo.GetById(id))
                .ReturnsAsync((Subject?)null);

            // act
            var result = await _service.DeleteSubject(id);

            // assert
            result.Should().BeFalse();
            _mockRepo.Verify(repo => repo.GetById(id), Times.Once);
        }

        [Fact]
        public async Task DeleteSubject_DeberiaRetornarTrue_SiLaMateriaExiste()
        {
            // arrange
            int id = 10;
            string name = "Matemáticas";

            var subject = new Subject
            {
                Name = name,
                Id = id,
            };

            _mockRepo.Setup(repo => repo.GetById(id))
                .ReturnsAsync(subject);

            // act
            var result = await _service.DeleteSubject(id);

            // assert
            result.Should().BeTrue();
            _mockRepo.Verify(repo => repo.GetById(id), Times.Once);
            _mockRepo.Verify(repo => repo.Delete(subject), Times.Once);
            _mockRepo.Verify(repo => repo.Save(), Times.Once);
        }
    }
}