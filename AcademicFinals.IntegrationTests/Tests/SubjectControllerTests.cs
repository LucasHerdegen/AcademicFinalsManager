using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace AcademicFinals.IntegrationTests.Tests
{
    public class SubjectControllerTests : IClassFixture<AcademicFinalsWebApplicationFactory>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public SubjectControllerTests(AcademicFinalsWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetSubjects_RetornaUnauthorized_CuandoNoTengoToken()
        {
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
            // arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("Authorization", "Test");

            // act
            var response = await client.GetAsync("/api/subject");

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}