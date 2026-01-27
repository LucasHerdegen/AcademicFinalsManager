using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademicFinals.API.Controllers;
using AcademicFinals.API.DTOs;
using AcademicFinals.API.Services;
using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace AcademicFinals.Tests.Controllers
{
    public class SubjectControllerTests
    {
        private readonly Mock<ISubjectService> _mockService;
        private readonly Mock<IValidator<SubjectPostDto>> _mockPostValidator;
        private readonly Mock<IValidator<SubjectPutDto>> _mockPutValidator;

        private readonly SubjectController _controller;

        public SubjectControllerTests()
        {
            _mockService = new Mock<ISubjectService>();
            _mockPostValidator = new Mock<IValidator<SubjectPostDto>>();
            _mockPutValidator = new Mock<IValidator<SubjectPutDto>>();

            _controller = new SubjectController(_mockService.Object, _mockPostValidator.Object, _mockPutValidator.Object);
        }

        [Fact]
        public async Task GetSubject_DeberiaRetornarBadRequest_CuandoElId_NoEsPositivo()
        {
            // arrange
            int id = -10;

            // act
            var result = await _controller.GetSubject(id);

            // assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task GetSubject_DeberiaRetornarNotFound_CuandoLaMateria_NoExiste()
        {
            // arrange
            int id = 10;

            _mockService.Setup(service => service.GetSubject(id))
                .ReturnsAsync((SubjectDto?)null);

            // act
            var result = await _controller.GetSubject(id);

            // assert
            result.Result.Should().BeOfType<NotFoundResult>();

            // var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            // var dto = okResult.Value.Should().BeAssignableTo<SubjectDto>().Subject;

            // dto.Id.Should().Be(id);
            // _mockService.Verify(service => service.GetSubject(id), Times.Once);
        }

        [Fact]
        public async Task GetSubject_DeberiaRetornarLaMateria_CuandoEstaExiste()
        {
            // arrange
            int id = 10;
            string name = "Matemáticas";

            var subject = new SubjectDto
            {
                Id = id,
                Name = name
            };

            _mockService.Setup(service => service.GetSubject(id))
                .ReturnsAsync(subject);

            // act
            var result = await _controller.GetSubject(id);

            // assert
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var dto = okResult.Value.Should().BeAssignableTo<SubjectDto>().Subject;

            dto.Id.Should().Be(id);
            _mockService.Verify(service => service.GetSubject(id), Times.Once);
        }
    }
}