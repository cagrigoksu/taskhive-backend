using System.Text.Json;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using TaskHive.Models;

namespace TaskHive.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : Controller
    {
        private readonly IHttpClientFactory? _httpClientFactory;
        private readonly HttpClient? _apiClient;
        private readonly JsonSerializerOptions? _options;

        public ProjectController(IHttpClientFactory? httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _apiClient = _httpClientFactory.CreateClient("api-gateway");
            _options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
        }

        [HttpGet("GetProjectListAsync")]
        [EnableCors("default")]
        public async Task<IActionResult> GetProjectListAsync()
        {
            var response = await _apiClient.GetAsync("gateway/Project/getProjectListAsync");
            
            if(response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var projectList = JsonSerializer.Deserialize<List<ProjectModel>>(result, _options);
                    
                return Ok(projectList);
            }
            
            return StatusCode((int)response.StatusCode, response.ReasonPhrase);
        }

    }
}