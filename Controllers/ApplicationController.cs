using CapitalApp.Data;
using CapitalApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CapitalApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationRepository _applicationRepository;
        public ApplicationController(IApplicationRepository applicationRepository)
        {
            this._applicationRepository = applicationRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateApplication(ApplicationDto applicationDto)
        {
            try
            {
                var createdApplication = await _applicationRepository.CreateApplicationAsync(applicationDto);
                return CreatedAtAction(nameof(GetApplicationById), new { applicationId = createdApplication.ApplicationID }, createdApplication);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }


        [HttpGet("{applicationId}")]
        public async Task<IActionResult> GetApplicationById(string applicationId)
        { 
            var application = await _applicationRepository.GetApplicationByIdAsync(applicationId);
            if (application == null)
            {
                return NotFound();
            }
            return Ok(application);
        }



    }
}
