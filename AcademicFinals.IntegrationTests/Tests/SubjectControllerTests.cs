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
    public class SubjectControllerTests : IClassFixture<AcademicFinalsWebApplicationFactory>
    {
        private readonly AcademicFinalsWebApplicationFactory _factory;

        public SubjectControllerTests(AcademicFinalsWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetSubjects_RetornaUnauthorized_CuandoNoTengoToken()
        {
            await _factory.EnsureCleanDatabaseAsync();

            // arrange
            var client = _factory.CreateClient();

            // act
            var response = await client.GetAsync("/api/subject");

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task GetSubjects_RetornaOk_CuandoEstoyAutenticado()
        {
            await _factory.EnsureCleanDatabaseAsync();

            // arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("Authorization", "Test");

            // act
            var response = await client.GetAsync("/api/subject");
            var result = await response.Content.ReadFromJsonAsync<IEnumerable<SubjectDto>>();

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetSubjects_RetornaLista_CuandoHayMaterias()
        {
            // arrange
            await _factory.EnsureCleanDatabaseAsync();

            using (var scope = _factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<ApplicationContext>();

                if (context == null)
                    throw new Exception("Something went wrong finding the context...");

                await context.Subjects.AddAsync(new Subject
                {
                    Id = 1,
                    Name = "Matemáticas"
                });
                await context.SaveChangesAsync();
            }

            var client = CreateAuthenticatedClient();

            // act
            var response = await client.GetAsync("/api/subject");
            var result = await response.Content.ReadFromJsonAsync<IEnumerable<SubjectDto>>();

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Should().ContainSingle(s => s.Name == "Matemáticas");
        }

        [Fact]
        public async Task CreateSubject_RetornaBadRequest_SiLaValidacion_NoPasa()
        {
            // arrange
            await _factory.EnsureCleanDatabaseAsync();

            string name = "";

            var subjectPostDto = new SubjectPostDto
            {
                Name = name
            };

            var client = CreateAuthenticatedClient();

            // act
            var response = await client.PostAsync("/api/subject", JsonContent.Create(subjectPostDto));

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task CreateSubject_RetornaConflict_SiLaMateria_YaExiste()
        {
            // arrange
            await _factory.EnsureCleanDatabaseAsync();

            string name = "Matemáticas";

            using (var scope = _factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<ApplicationContext>();

                if (context == null)
                    throw new Exception("Something went wrong finding the context...");

                await context.Subjects.AddAsync(new Subject
                {
                    Id = 1,
                    Name = name
                });
                await context.SaveChangesAsync();
            }

            var subjectPostDto = new SubjectPostDto
            {
                Name = name
            };

            var client = CreateAuthenticatedClient();

            // act
            var response = await client.PostAsync("/api/subject", JsonContent.Create(subjectPostDto));

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task CreateSubject_RetornaCreated_SiLaMateria_NoExiste()
        {
            // arrange
            await _factory.EnsureCleanDatabaseAsync();

            string name = "Matemáticas";

            var subjectPostDto = new SubjectPostDto
            {
                Name = name
            };

            var client = CreateAuthenticatedClient();

            // act
            var response = await client.PostAsync("/api/subject", JsonContent.Create(subjectPostDto));
            var headers = response.Headers;

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            headers.Contains("location").Should().BeTrue();
        }

        [Fact]
        public async Task UpdateSubject_RetornaBadRequest_SiLaValidacion_NoPasa()
        {
            // arrange
            await _factory.EnsureCleanDatabaseAsync();

            var subjectPutDto = new SubjectPutDto
            {
                Id = -1,
                Name = ""
            };

            var client = CreateAuthenticatedClient();

            // act
            var response = await client.PutAsync("/api/subject", JsonContent.Create(subjectPutDto));

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task UpdateSubject_RetornaNotFound_SiLaMateria_NoExiste()
        {
            // arrange
            await _factory.EnsureCleanDatabaseAsync();

            int id = 1;
            string name = "Matemáticas";

            var subjectPutDto = new SubjectPutDto
            {
                Id = id,
                Name = name
            };

            var client = CreateAuthenticatedClient();

            // act
            var response = await client.PutAsync("/api/subject", JsonContent.Create(subjectPutDto));

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task UpdateSubject_RetornaNoContent_SiLaMateriaExiste()
        {
            // arrange
            await _factory.EnsureCleanDatabaseAsync();

            int id = 1;
            string name = "Matemáticas";

            using (var scope = _factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<ApplicationContext>();

                if (context == null)
                    throw new Exception("Something went wrong finding the context...");

                await context.Subjects.AddAsync(new Subject
                {
                    Id = id,
                    Name = "Quimica"
                });
                await context.SaveChangesAsync();
            }

            var subjectPutDto = new SubjectPutDto
            {
                Id = id,
                Name = name
            };

            var client = CreateAuthenticatedClient();

            // act
            var putResponse = await client.PutAsync("/api/subject", JsonContent.Create(subjectPutDto));
            var getResponse = await client.GetAsync($"/api/subject/{id}");
            var result = await getResponse.Content.ReadFromJsonAsync<SubjectDto>();

            // assert
            putResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
            result!.Name.Should().Be(name);
        }

        [Fact]
        public async Task DeleteSubject_RetornaBadRequest_SiLaValidacion_NoPasa()
        {
            // arrange
            await _factory.EnsureCleanDatabaseAsync();

            int id = -1;

            var client = CreateAuthenticatedClient();

            // act
            var response = await client.DeleteAsync($"/api/subject/{id}");

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task DeleteSubject_RetornaNotFound_SiLaMateria_NoExiste()
        {
            // arrange
            await _factory.EnsureCleanDatabaseAsync();

            int id = 1;

            var client = CreateAuthenticatedClient();

            // act
            var response = await client.DeleteAsync($"/api/subject/{id}");

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteSubject_RetornaNoContent_SiLaMateriaExiste()
        {
            // arrange
            await _factory.EnsureCleanDatabaseAsync();

            int id = 1;
            string name = "Matemáticas";

            using (var scope = _factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<ApplicationContext>();

                if (context == null)
                    throw new Exception("Something went wrong finding the context...");

                await context.Subjects.AddAsync(new Subject
                {
                    Id = id,
                    Name = name
                });
                await context.SaveChangesAsync();
            }

            var client = CreateAuthenticatedClient();

            // act
            var deleteResponse = await client.DeleteAsync($"/api/subject/{id}");
            var getResponse = await client.GetAsync($"/api/subject/{id}");

            // assert
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
            getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }


        private HttpClient CreateAuthenticatedClient()
        {
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("Authorization", "Test");

            return client;
        }
    }
}