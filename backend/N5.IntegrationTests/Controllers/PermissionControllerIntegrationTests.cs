using Microsoft.AspNetCore.Mvc.Testing;
using N5.Core.DTOs;
using N5.Shared.Pagination;
using N5.WebAPI.ViewModels;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace N5.IntegrationTests.Controllers
{
    public class PermissionControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly CustomWebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public PermissionControllerIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task RequestPermission_ReturnsOkResult()
        {
            // Arrange
            var permissionDto = new RequestPermissionViewModel
            {
                NombreEmpleado = "John",
                ApellidoEmpleado = "Doe",
                Tipo = 1
            };
            var json = JsonConvert.SerializeObject(permissionDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/Permission", content);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetPermissions_ReturnsOkResultWithPagedData()
        {
            // Arrange
            var paginationParams = new PaginationParams
            {
                PageNumber = 0,
                PageSize = 10
            };
            var queryString = $"?pageNumber={paginationParams.PageNumber}&pageSize={paginationParams.PageSize}";

            // Act
            var response = await _client.GetAsync($"/api/Permission{queryString}");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();
            var permissions = JsonConvert.DeserializeObject<PagedResult<PermissionDto>>(responseContent);

            // Perform additional assertions on the returned paged permissions
            Assert.NotNull(permissions);
            // Add more assertions as needed
        }
    }
}