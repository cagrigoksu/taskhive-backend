using System.Text.Json;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TaskHive.Models.Role;

namespace TaskHive
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController: Controller
    {
          private readonly IHttpClientFactory? _httpClientFactory;
        private readonly HttpClient? _apiClient;
        private readonly string _gateway = "gateway/Role/";
        private readonly string _contentType = "application/json";
        public RoleController(IHttpClientFactory? httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _apiClient = _httpClientFactory.CreateClient("api-gateway");
        }

        [HttpGet("GetRoles")]
        [EnableCors("default")]
        public async Task<IActionResult> GetRolesAsync()
        {
            var response = await _apiClient.GetAsync(_gateway + "get-roles");
            
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