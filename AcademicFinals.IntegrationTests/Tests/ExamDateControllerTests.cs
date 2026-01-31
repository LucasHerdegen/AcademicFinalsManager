using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using AcademicFinals.API.DTOs;
using AcademicFinals.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AcademicFinals.IntegrationTests.Tests
{
    public class ExamDateControllerTests : IClassFixture<AcademicFinalsWebApplicationFactory>
    {
        private readonly AcademicFinalsWebApplicationFactory _factory;

        public ExamDateControllerTests(AcademicFinalsWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetExamDate_RetornaBadRequest_SiLaValidacion_NoPasa()
        {
            // arrange
            await _factory.EnsureCleanDatabaseAsync();

            int id = -1;

            var client = CreateAuthenticatedClient();

            // act
            var response = await client.GetAsync($"/api/examdate/{id}");

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GetExamDate_RetornaNotFound_SiLaFecha_NoExiste()
        {
            // arrange
            await _factory.EnsureCleanDatabaseAsync();

            int id = 1;

            var client = CreateAuthenticatedClient();

            // act
            var response = await client.GetAsync($"/api/examdate/{id}");

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetExamDate_RetornaLaFecha_SiLaFechaExiste()
        {
            // arrange
            await _factory.EnsureCleanDatabaseAsync();
            var client = CreateAuthenticatedClient();

            string subjectName = "Matematicas";
            int subjectId = 1;

            int examId = -1;

            using (var scope = _factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<ApplicationContext>();

                if (context == null)
                    throw new Exception("Something went wrong finding the context...");

                var subject = new Subject
                {
                    Name = subjectName,
                    Id = subjectId
                };

                await context.Subjects.AddAsync(subject);
                await context.SaveChangesAsync();

                var examDate = new ExamDate
                {
                    Date = DateTime.Now.AddDays(5),
                    MaxCapacity = 20,
                    SubjectId = subjectId
                };

                await context.ExamDates.AddAsync(examDate);
                await context.SaveChangesAsync();

                examId = examDate.Id;
            }

            // act
            var response = await client.GetAsync($"/api/examdate/{examId}");
            var getResult = await response.Content.ReadFromJsonAsync<ExamDateDto>();

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            getResult!.Id.Should().Be(examId);
        }

        [Fact]
        public async Task CreateExamDate_RetornaBadRequest_SiLaValidacion_NoPasa()
        {
            // arrange
            await _factory.EnsureCleanDatabaseAsync();
            var client = CreateAuthenticatedClient();

            var examDatePostDto = new ExamDatePostDto
            {
                Date = DateTime.Now.AddDays(-1),
                MaxCapacity = -1,
                SubjectId = -1
            };

            // act
            var response = await client.PostAsync("/api/examdate", JsonContent.Create(examDatePostDto));

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task CreateExamDate_RetornaConflict_SiLaFecha_YaExiste()
        {
            // arrange
            await _factory.EnsureCleanDatabaseAsync();
            var client = CreateAuthenticatedClient();

            string subjectName = "Matematicas";
            int subjectId = 1;

            var examDateUtc = DateTime.Now.AddDays(5);
            int maxCapaxity = 20;

            int examId = -1;

            using (var scope = _factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<ApplicationContext>();

                if (context == null)
                    throw new Exception("Something went wrong finding the context...");

                var subject = new Subject
                {
                    Name = subjectName,
                    Id = subjectId
                };

                await context.Subjects.AddAsync(subject);
                await context.SaveChangesAsync();

                var examDate = new ExamDate
                {
                    Date = examDateUtc,
                    MaxCapacity = maxCapaxity,
                    SubjectId = subjectId
                };

                await context.ExamDates.AddAsync(examDate);
                await context.SaveChangesAsync();

                examId = examDate.Id;
            }

            var examDatePostDto = new ExamDatePostDto
            {
                Date = examDateUtc,
                MaxCapacity = maxCapaxity,
                SubjectId = subjectId
            };

            // act
            var response = await client.PostAsync("/api/examdate", JsonContent.Create(examDatePostDto));

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task CreateExamDate_RetornaCreated_SiLaFecha_NoExiste()
        {
            // arrange
            await _factory.EnsureCleanDatabaseAsync();
            var client = CreateAuthenticatedClient();

            string subjectName = "Matematicas";
            int subjectId = 1;

            var examDateUtc = DateTime.Now.AddDays(5);
            int maxCapaxity = 20;

            using (var scope = _factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<ApplicationContext>();

                if (context == null)
                    throw new Exception("Something went wrong finding the context...");

                var subject = new Subject
                {
                    Name = subjectName,
                    Id = subjectId
                };

                await context.Subjects.AddAsync(subject);
                await context.SaveChangesAsync();
            }

            var examDatePostDto = new ExamDatePostDto
            {
                Date = examDateUtc,
                MaxCapacity = maxCapaxity,
                SubjectId = subjectId
            };

            // act
            var response = await client.PostAsync("/api/examdate", JsonContent.Create(examDatePostDto));
            var headers = response.Headers;

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            headers.Contains("location").Should().BeTrue();
        }



        private HttpClient CreateAuthenticatedClient()
        {
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("Authorization", "Test");

            return client;
        }
    }
}