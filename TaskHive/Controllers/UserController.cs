using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TaskHive.Models.User;

namespace TaskHive.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class UserController : Controller
    {
        private readonly IHttpClientFactory? _httpClientFactory;
         private readonly HttpClient? _apiClient;
        private readonly string _gateway = "gateway/User/";
        private readonly string _contentType = "application/json";

        public UserController(IHttpClientFactory? httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _apiClient = _httpClientFactory.CreateClient("api-gateway");
        }

        [HttpPost("AddOrEditUserProfile")]
        [EnableCors("default")]
        public async Task<IActionResult> AddOrEditUserProfile([FromBody] UserProfileModel userProfile)
        {
           
            var content = new StringContent(JsonConvert.SerializeObject(userProfile), null, _contentType);

            var response = _apiClient.PostAsync(_gateway + "add-or-edit-user-profile", content).Result;
                
            if(response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var profile = JsonConvert.DeserializeObject<UserProfileModel>(result);
                    
                return Ok(profile);
            }
            
            return StatusCode((int)response.StatusCode, response.ReasonPhrase);
        }

        [HttpPost("EditUserEmail")]
        [EnableCors("default")]
        public async Task<IActionResult> EditUserEmailAsync([FromBody] UserModel user)
        {
            var content = new StringContent(JsonConvert.SerializeObject(user), null, _contentType);

            var response = _apiClient.PostAsync(_gateway + "edit-user-email", content).Result;
                
            if(response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<UserModel>(result);
                    
                return Ok(responseData);
            }
            
            return StatusCode((int)response.StatusCode, response.ReasonPhrase);
        }

        [HttpGet("GetUserProfileByUserId/{userId}")]
        [EnableCors("default")]
        public async Task<IActionResult> GetUserProfileByUserIdAsync(int userId)
        {
            var response = await _apiClient.GetAsync(_gateway + "get-user-profile-by-userId/" + userId.ToString());
                
            if(response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var userProfile = JsonConvert.DeserializeObject<UserProfileModel>(result);
                    
                return Ok(userProfile);
            }
            
            return StatusCode((int)response.StatusCode, response.ReasonPhrase);
        }       
    }
}