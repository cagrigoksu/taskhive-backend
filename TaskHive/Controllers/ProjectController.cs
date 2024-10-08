using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TaskHive.Models;
using TaskHive.Models.Project;

namespace TaskHive.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : Controller
    {
        private readonly IHttpClientFactory? _httpClientFactory;
        private readonly HttpClient? _apiClient;
        private readonly string _gateway = "gateway/Project/";
        private readonly string _contentType = "application/json";

        public ProjectController(IHttpClientFactory? httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _apiClient = _httpClientFactory.CreateClient("api-gateway");
        }

        [HttpPost("CreateProject")]
        [EnableCors("default")]
        public async Task<IActionResult> CreateProjectAsync(ProjectModel project)
        {
            
            var jsonContent = new StringContent(JsonConvert.SerializeObject(project), null, _contentType);

            var response = await _apiClient.PostAsync(_gateway + "create-project", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var projectList = JsonConvert.DeserializeObject<ProjectModel>(result);

                    return Ok(projectList);
                }

                return StatusCode((int)response.StatusCode, response.ReasonPhrase);
        }

        [HttpPut("EditProject")]
        [EnableCors("default")]
        public async Task<IActionResult> EditProjectAsync(ProjectModel project)
        {
            
            var jsonContent = new StringContent(JsonConvert.SerializeObject(project), null, _contentType);

            var response = await _apiClient.PutAsync(_gateway + "edit-project", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var projectList = JsonConvert.DeserializeObject<ProjectModel>(result);

                    return Ok(projectList);
                }

                return StatusCode((int)response.StatusCode, response.ReasonPhrase);
        }


        [HttpGet("GetProjects")]
        [EnableCors("default")]
        public async Task<IActionResult> GetProjectsAsync()
        {
            var response = await _apiClient.GetAsync(_gateway + "get-projects");
            
            if(response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var projectList = JsonConvert.DeserializeObject<List<ProjectModel>>(result);
                    
                return Ok(projectList);
            }
            
            return StatusCode((int)response.StatusCode, response.ReasonPhrase);
        }

        [HttpGet("GetProjectById/{projectId}")]
        [EnableCors("default")]
        public async Task<IActionResult> GetProjectByIdAsync(int projectId){

            var response = await _apiClient.GetAsync(_gateway + "get-project-by-id/" + projectId.ToString());

            if(response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var project = JsonConvert.DeserializeObject<ProjectModel>(result);

                return Ok(project);
            }

            return StatusCode((int)response.StatusCode, response.ReasonPhrase);
        }

        [HttpGet("GetStatusEnum")]
        [EnableCors("default")]
        public async Task<IActionResult> GetStatusEnum()
        {
            var response = await _apiClient.GetAsync(_gateway + "get-status-enum");

            if(response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var projectList = JsonConvert.DeserializeObject<List<StatusModel>>(result);
                    
                return Ok(projectList);
            }
            
            return StatusCode((int)response.StatusCode, response.ReasonPhrase);
        }

        [HttpGet("GetPriorityEnum")]
        [EnableCors("default")]
        public async Task<IActionResult> GetPriorityEnum()
        {
            var response = await _apiClient.GetAsync(_gateway + "get-priority-enum");

            if(response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var projectList = JsonConvert.DeserializeObject<List<PriorityModel>>(result);
                    
                return Ok(projectList);
            }
            
            return StatusCode((int)response.StatusCode, response.ReasonPhrase);
        }

    }
}