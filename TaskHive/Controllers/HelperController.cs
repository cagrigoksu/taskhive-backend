using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TaskHive.Models.Department;
using TaskHive.Models.Role;

namespace TaskHive.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HelperController: Controller
    {
        private readonly IHttpClientFactory? _httpClientFactory;
        private readonly HttpClient? _apiClient;
        private readonly string _gatewayDepartment = "gateway/Department/";
        private readonly string _gatewayRole = "gateway/Role/";
        private readonly string _contentType = "application/json";

        public HelperController(IHttpClientFactory? httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _apiClient = _httpClientFactory.CreateClient("api-gateway");
        }

        [HttpGet("GetDepartments")]
        [EnableCors("default")]
        public async Task<IActionResult> GetDepartmentsAsync()
        {
            var response = await _apiClient.GetAsync(_gatewayDepartment + "get-departments");
            
            if(response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var departmentList = JsonConvert.DeserializeObject<List<DepartmentModel>>(result);
                    
                return Ok(departmentList);
            }
            
            return StatusCode((int)response.StatusCode, response.ReasonPhrase);
        }

        [HttpGet("GetRoles")]
        [EnableCors("default")]
        public async Task<IActionResult> GetRolesAsync()
        {
            var response = await _apiClient.GetAsync(_gatewayRole + "get-roles");
            
            if(response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var roleList = JsonConvert.DeserializeObject<List<RoleModel>>(result);
                    
                return Ok(roleList);
            }
            
            return StatusCode((int)response.StatusCode, response.ReasonPhrase);
        }
    }
}