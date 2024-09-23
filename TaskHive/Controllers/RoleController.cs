using System.Text.Json;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using TaskHive.Models.Role;

namespace TaskHive
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController: Controller
    {
          private readonly IHttpClientFactory? _httpClientFactory;
        private readonly HttpClient? _apiClient;
        private readonly JsonSerializerOptions? _options;
        private readonly string _gateway;

        public RoleController(IHttpClientFactory? httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _apiClient = _httpClientFactory.CreateClient("api-gateway");
            _options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
            _gateway = "gateway/Role/";
        }

        [HttpGet("GetRoles")]
        [EnableCors("default")]
        public async Task<IActionResult> GetRolesAsync()
        {
            var response = await _apiClient.GetAsync(_gateway + "get-roles");
            
            if(response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var roleList = JsonSerializer.Deserialize<List<RoleModel>>(result, _options);
                    
                return Ok(roleList);
            }
            
            return StatusCode((int)response.StatusCode, response.ReasonPhrase);
        }


    }
}