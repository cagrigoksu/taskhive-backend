using System.Text.Json;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using TaskHive.Models.User;

namespace TaskHive.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class UserController : Controller
    {
        private readonly IHttpClientFactory? _httpClientFactory;
         private readonly HttpClient? _apiClient;
        private readonly JsonSerializerOptions? _options;
        private readonly string _gateway;

        public UserController(IHttpClientFactory? httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _apiClient = _httpClientFactory.CreateClient("api-gateway");
            _options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
            _gateway = "gateway/User/";
        }

        [HttpPost("AddUserProfile")]
        [EnableCors("default")]
        public async Task<IActionResult> AddUserProfileAsync([FromForm] UserProfileModel userProfile)
        {
            var content = new Dictionary<string, string>
            {
                ["name"] = userProfile.Name,
                ["surname"] = userProfile.Surname,
                ["email"] = userProfile.Email,
                ["phoneNumber"] = userProfile.PhoneNumber,
                ["department"] = userProfile.Department,
                ["role"] = userProfile.Role
            };
            var response = _apiClient.PostAsync(_gateway+"addUserProfile", new FormUrlEncodedContent(content)).Result;
                
            if(response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                userProfile = JsonSerializer.Deserialize<UserProfileModel>(result, _options);
                    
                return Ok(userProfile);
            }
            
            return StatusCode((int)response.StatusCode, response.ReasonPhrase);
        }

        [HttpPost("EditUserProfile")]
        [EnableCors("default")]
        public async Task<IActionResult> EditUserProfileAsync([FromForm] UserProfileModel userProfile)
        {
            var content = new Dictionary<string,string>
            {
                ["userId"] = userProfile.UserId.ToString(),
                ["name"] = userProfile.Name,
                ["surname"] = userProfile.Surname,
                ["email"] = userProfile.Email,
                ["phoneNumber"] = userProfile.PhoneNumber,
                ["department"] = userProfile.Department,
                ["role"] = userProfile.Role
            };

            var response = _apiClient.PostAsync(_gateway+"editUserProfile", new FormUrlEncodedContent(content)).Result;
                
            if(response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                userProfile = JsonSerializer.Deserialize<UserProfileModel>(result, _options);
                    
                return Ok(userProfile);
            }
            
            return StatusCode((int)response.StatusCode, response.ReasonPhrase);
        }

        [HttpGet("GetUserProfileByUserId/{userId}")]
        [EnableCors("default")]
        public async Task<IActionResult> GetUserProfileByUserIdAsync(int userId)
        {
            var response = await _apiClient.GetAsync(_gateway+"getUserProfileByUserId/" + userId.ToString());
                
            if(response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var userProfile = JsonSerializer.Deserialize<UserProfileModel>(result, _options);
                    
                return Ok(userProfile);
            }
            
            return StatusCode((int)response.StatusCode, response.ReasonPhrase);
        }       
    }
}