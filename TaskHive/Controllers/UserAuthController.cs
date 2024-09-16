using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TaskHive.Models;
using TaskHive.Models.Data;

namespace TaskHive.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserAuthController : Controller
    {
        private readonly IHttpClientFactory? _httpClientFactory;

        public UserAuthController(IHttpClientFactory? httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost("LogIn")]
        [EnableCors("default")]
        public async Task<IActionResult> LogIn([FromBody] LoginModel dataObject)
        {
            var email = dataObject.Email;
            var pwd = dataObject.Password;

            IEnumerable<KeyValuePair<string, string>> content = new List<KeyValuePair<string, string>>
            {
                new("email", email),
                new("password", pwd)
            };

            // create client and post 
            var apiClient = _httpClientFactory.CreateClient("api-gateway");

            var result = apiClient.PostAsync("gateway/User/login", new FormUrlEncodedContent(content)).Result;

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

        [HttpPost("LogOn")]
        [EnableCors("default")]
        public async Task<IActionResult> LogOn([FromBody] LogonModel dataObject)
        {

            var email = dataObject.Email;
            var pwd = dataObject.Password;
            var pwdConf = dataObject.PasswordConfirmation;

            IEnumerable<KeyValuePair<string, string>> content = new List<KeyValuePair<string, string>>
            {
                new("email", email),
                new("password", pwd),
                new("passwordconf", pwdConf)
            };

            // create client and post
            var apiClient = _httpClientFactory.CreateClient("api-gateway");
            var result = apiClient.PostAsync("gateway/User/logon", new FormUrlEncodedContent(content)).Result;

            if (result.IsSuccessStatusCode)
            {
                var data = await result.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<LoginDataModel>(data);

                Globals.UserId = user.Id;
                HttpContext.Session.SetInt32("Id", user.Id);
                HttpContext.Session.SetString("Email", user.Email);

                return Json(new
                {
                    user = user.Id,
                    email = user.Email
                });
            }

            return Json(null);
        }
    }
}
