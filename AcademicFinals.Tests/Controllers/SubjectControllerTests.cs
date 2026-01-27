using AcademicFinals.API.Controllers;
using AcademicFinals.API.DTOs;
using AcademicFinals.API.Services;
using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Moq;

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

        [Fact]
        public async Task CreateSubject_DeberiaRetornarBadRequest_SiLaValidacion_NoPasa()
        {
            // arrange
            string name = "";

            var subjectPostDto = new SubjectPostDto
            {
                Name = name,
            };

            var validationResult = new FluentValidation.Results.ValidationResult(new[] { new FluentValidation.Results.ValidationFailure("Name", "Name is required") });

            _mockPostValidator.Setup(validator => validator.ValidateAsync(subjectPostDto, It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            // act
            var result = await _controller.CreateSubject(subjectPostDto);

            // assert
            result.Should().BeOfType<BadRequestObjectResult>();
            _mockPostValidator
                .Verify(validator => validator.ValidateAsync(subjectPostDto, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreateSubject_DeberiaRetornarConflict_SiLaMateria_YaExiste()
        {
            // arrange
            string name = "Matemáticas";

            var subjectPostDto = new SubjectPostDto
            {
                Name = name,
            };

            var validationResult = new FluentValidation.Results.ValidationResult();

            _mockPostValidator.Setup(validator => validator.ValidateAsync(subjectPostDto, It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            _mockService.Setup(service => service.CreateSubject(subjectPostDto)).ReturnsAsync((SubjectDto?)null);

            // act
            var result = await _controller.CreateSubject(subjectPostDto);

            // assert
            result.Should().BeOfType<ConflictObjectResult>();
            _mockPostValidator
                .Verify(validator => validator.ValidateAsync(subjectPostDto, It.IsAny<CancellationToken>()), Times.Once);
            _mockService.Verify(service => service.CreateSubject(subjectPostDto), Times.Once);
        }

        [Fact]
        public async Task CreateSubject_DeberiaRetornarCreated_CuandoLaMateria_NoExiste()
        {
            // arrange
            string name = "Matemáticas";
            int id = 10;

            var subjectPostDto = new SubjectPostDto
            {
                Name = name,
            };
            var subject = new SubjectDto
            {
                Name = name,
                Id = id,
            };

            var validationResult = new FluentValidation.Results.ValidationResult();

            _mockPostValidator.Setup(validator => validator.ValidateAsync(subjectPostDto, It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            _mockService.Setup(service => service.CreateSubject(subjectPostDto)).ReturnsAsync(subject);

            // act
            var result = await _controller.CreateSubject(subjectPostDto);

            // assert
            var createdResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
            createdResult.StatusCode.Should().Be(201);
            createdResult.Value.Should().Be(subject);
            createdResult.ActionName.Should().Be(nameof(SubjectController.GetSubject));
            createdResult.RouteValues!["Id"].Should().Be(id);

            _mockPostValidator
                .Verify(validator => validator.ValidateAsync(subjectPostDto, It.IsAny<CancellationToken>()), Times.Once);
            _mockService.Verify(service => service.CreateSubject(subjectPostDto), Times.Once);
        }

        [Fact]
        public async Task UpdateSubject_DeberiaRetornarBadRequest_SiLaValidacion_NoPasa()
        {
            // arrange
            string name = "";
            int id = 10;

            var subjectPutDto = new SubjectPutDto
            {
                Name = name,
                Id = id
            };

            var validationResult = new FluentValidation.Results.ValidationResult(new[] { new FluentValidation.Results.ValidationFailure("Name", "Name is required") });

            _mockPutValidator.Setup(validator => validator.ValidateAsync(subjectPutDto, It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            // act
            var result = await _controller.UpdateSubject(subjectPutDto);

            // assert
            result.Should().BeOfType<BadRequestObjectResult>();
            _mockPutValidator
                .Verify(validator => validator.ValidateAsync(subjectPutDto, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateSubject_DeberiaRetornarNotFound_SiElServicio_NoLaEncuentra()
        {
            // arrange
            string name = "Matemáticas";
            int id = 10;

            var subjectPutDto = new SubjectPutDto
            {
                Name = name,
                Id = id
            };

            var validationResult = new FluentValidation.Results.ValidationResult();

            _mockPutValidator.Setup(validator => validator.ValidateAsync(subjectPutDto, It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);
            _mockService.Setup(service => service.UpdateSubject(subjectPutDto)).ReturnsAsync(false);

            // act
            var result = await _controller.UpdateSubject(subjectPutDto);

            // assert
            result.Should().BeOfType<NotFoundResult>();
            _mockPutValidator
                .Verify(validator => validator.ValidateAsync(subjectPutDto, It.IsAny<CancellationToken>()), Times.Once);
            _mockService.Verify(service => service.UpdateSubject(subjectPutDto), Times.Once);
        }

        [Fact]
        public async Task UpdateSubject_DeberiaRetornarNoContent_SiElServicio_LaEncuentra()
        {
            // arrange
            string name = "Matemáticas";
            int id = 10;

            var subjectPutDto = new SubjectPutDto
            {
                Name = name,
                Id = id
            };

            var validationResult = new FluentValidation.Results.ValidationResult();

            _mockPutValidator.Setup(validator => validator.ValidateAsync(subjectPutDto, It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);
            _mockService.Setup(service => service.UpdateSubject(subjectPutDto)).ReturnsAsync(true);

            // act
            var result = await _controller.UpdateSubject(subjectPutDto);

            // assert
            result.Should().BeOfType<NoContentResult>();
            _mockPutValidator
                .Verify(validator => validator.ValidateAsync(subjectPutDto, It.IsAny<CancellationToken>()), Times.Once);
            _mockService.Verify(service => service.UpdateSubject(subjectPutDto), Times.Once);
        }

        [Fact]
        public async Task DeleteSubject_DeberiaRetornarBadRequest_SiElId_NoEsPositivo()
        {
            // arrange
            int id = -10;

            // act
            var result = await _controller.DeleteSubject(id);

            // assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task DeleteSubject_DeberiaRetornarNotFound_SiLaMateria_NoExiste()
        {
            // arrange
            int id = 10;

            _mockService.Setup(service => service.DeleteSubject(id)).ReturnsAsync(false);

            // act
            var result = await _controller.DeleteSubject(id);

            // assert
            result.Should().BeOfType<NotFoundResult>();
            _mockService.Verify(service => service.DeleteSubject(id), Times.Once);
        }

        [Fact]
        public async Task DeleteSubject_DeberiaRetornarNoContent_SiLaMateria_SiExiste()
        {
            // arrange
            int id = 10;

            _mockService.Setup(service => service.DeleteSubject(id)).ReturnsAsync(true);

            // act
            var result = await _controller.DeleteSubject(id);

            // assert
            result.Should().BeOfType<NoContentResult>();
            _mockService.Verify(service => service.DeleteSubject(id), Times.Once);
        }
    }
}