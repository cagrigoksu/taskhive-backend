using System.Text.Json;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using TaskHive.Models.Department;

namespace TaskHive
{
    public class DepartmentController: Controller
    {
          private readonly IHttpClientFactory? _httpClientFactory;
        private readonly HttpClient? _apiClient;
        private readonly JsonSerializerOptions? _options;

        public DepartmentController(IHttpClientFactory? httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _apiClient = _httpClientFactory.CreateClient("api-gateway/Department/");
            _options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
        }

        [HttpGet("GetDepartments")]
        [EnableCors("default")]
        public async Task<IActionResult> GetDepartmentsAsync()
        {
            var response = await _apiClient.GetAsync("get-departments");
            
            if(response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var departmentList = JsonSerializer.Deserialize<List<DepartmentModel>>(result, _options);
                    
                return Ok(departmentList);
            }
            
            return StatusCode((int)response.StatusCode, response.ReasonPhrase);
        }


    }
}