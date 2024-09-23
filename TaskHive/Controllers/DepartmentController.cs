using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TaskHive.Models.Department;

namespace TaskHive
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentController: Controller
    {
          private readonly IHttpClientFactory? _httpClientFactory;
        private readonly HttpClient? _apiClient;
        private readonly string _gateway = "gateway/Department/";
        private readonly string _contentType = "application/json";

        public DepartmentController(IHttpClientFactory? httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _apiClient = _httpClientFactory.CreateClient("api-gateway");
        }

        [HttpGet("GetDepartments")]
        [EnableCors("default")]
        public async Task<IActionResult> GetDepartmentsAsync()
        {
            var response = await _apiClient.GetAsync(_gateway + "get-departments");
            
            if(response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var departmentList = JsonConvert.DeserializeObject<List<DepartmentModel>>(result);
                    
                return Ok(departmentList);
            }
            
            return StatusCode((int)response.StatusCode, response.ReasonPhrase);
        }


    }
}