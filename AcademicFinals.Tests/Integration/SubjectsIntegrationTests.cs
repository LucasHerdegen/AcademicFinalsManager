using System.Net;
using System.Net.Http.Json;
using AcademicFinals.API.DTOs;
using FluentAssertions;

namespace AcademicFinals.Tests.Integration
{
    public class SubjectsIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public SubjectsIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetSubjects_DeberiaRetornarOk_YListaVacia_AlInicio()
        {
            // arrange
            string path = "/api/Subject";

            // act
            var response = await _client.GetAsync(path);

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var subjects = await response.Content.ReadFromJsonAsync<List<SubjectDto>>();
            subjects.Should().BeEmpty(); // Como es una DB nueva en memoria, debería estar vacía
        }
    }
}