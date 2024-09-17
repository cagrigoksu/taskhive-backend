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

        [HttpPost("AddOrEditUserProfile")]
        [EnableCors("default")]
        public async Task<IActionResult> AddOrEditUserProfile([FromBody] UserProfileModel userProfile)
        {
            var content = new Dictionary<string, string>
            {
                ["userId"] = userProfile.UserId.ToString(),
                ["name"] = userProfile.Name,
                ["surname"] = userProfile.Surname,
                ["phoneNumber"] = userProfile.PhoneNumber,
                ["department"] = userProfile.Department,
                ["role"] = userProfile.Role
            };

            var response = _apiClient.PostAsync(_gateway+"add-or-edit-user-profile", new FormUrlEncodedContent(content)).Result;
                
            if(response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                userProfile = JsonSerializer.Deserialize<UserProfileModel>(result, _options);
                    
                return Ok(userProfile);
            }
            
            return StatusCode((int)response.StatusCode, response.ReasonPhrase);
        }

        [HttpPost("EditUserEmail")]
        [EnableCors("default")]
        public async Task<IActionResult> EditUserEmailAsync([FromBody] UserModel user)
        {
            var content = new Dictionary<string,string>
            {
                ["userId"] = user.UserId.ToString(),
                ["email"] = user.Email
            };

            var response = _apiClient.PostAsync(_gateway+"edit-user-email", new FormUrlEncodedContent(content)).Result;
                
            if(response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                user = JsonSerializer.Deserialize<UserModel>(result, _options);
                    
                return Ok(user);
            }
            
            return StatusCode((int)response.StatusCode, response.ReasonPhrase);
        }

        [HttpGet("GetUserProfileByUserId/{userId}")]
        [EnableCors("default")]
        public async Task<IActionResult> GetUserProfileByUserIdAsync(int userId)
        {
            var response = await _apiClient.GetAsync(_gateway+"get-user-profile-by-userId/" + userId.ToString());
                
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