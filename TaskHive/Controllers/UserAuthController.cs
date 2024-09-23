using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TaskHive.Models.Data;
using TaskHive.Models.UserAuth;

namespace TaskHive.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserAuthController : Controller
    {
        private readonly IHttpClientFactory? _httpClientFactory;
        private readonly HttpClient? _apiClient;
        private readonly string _gateway = "gateway/User/";
        private readonly string _contentType = "application/json";


        public UserAuthController(IHttpClientFactory? httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;    
            _apiClient = _httpClientFactory.CreateClient("api-gateway");          
        }

        [HttpPost("Login")]
        [EnableCors("default")]
        public async Task<IActionResult> Login([FromBody] LoginModel dataObject)
        {
            // serialize to json
            var content = new StringContent(JsonConvert.SerializeObject(dataObject), null, _contentType);

            var result = _apiClient.PostAsync(_gateway + "login", content).Result;

            if (result.IsSuccessStatusCode)
            {
                var data = await result.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<LoginDataModel>(data);

                Globals.UserId = user.Id;
                HttpContext.Session.SetInt32("Id", user.Id);
                HttpContext.Session.SetString("Email", user.Email);

                return Json(new
                {
                    userId = user.Id,
                    email = user.Email
                });
            }

            return Json(null);
        }

        [HttpPost("Logon")]
        [EnableCors("default")]
        public async Task<IActionResult> Logon([FromBody] LogonModel dataObject)
        {
            var content = new StringContent(JsonConvert.SerializeObject(dataObject), null, _contentType);

            var result = _apiClient.PostAsync(_gateway + "logon", content).Result;

            if (result.IsSuccessStatusCode)
            {
                var data = await result.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<LoginDataModel>(data);

                Globals.UserId = user.Id;
                HttpContext.Session.SetInt32("Id", user.Id);
                HttpContext.Session.SetString("Email", user.Email);

                return Json(new
                {
                    userId = user.Id,
                    email = user.Email
                });
            }

            return Json(null);
        }
    }
}
