using CommandService.Data.DTOs.Platform;
using CommandService.Services;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers
{
    [ApiController]
    public class PlatformController : ControllerBase
    {
        #region Constructors

        public PlatformController(PlatformService platformService)
        {
            _platformService = platformService;
        }

        #endregion Constructors

        #region Properties

        private readonly PlatformService _platformService;

        #endregion Properties

        #region Methods

        /// <summary>
        /// Asynchronous method for loading all platforms
        /// </summary>
        /// <returns>A list of objects containing information about each platform</returns>
        /// <response code="500">Error message</response>
        /// <response code="200">A list of objects containing information about each platform</response>
        [HttpGet]
        [Route("c/Platform/GetPlatformsAsync")]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(List<ReadPlatformDTO>), 200)]
        public async Task<IActionResult> GetPlatformsAsync()
        {
            var platforms = await _platformService.GetPlatformsAsync();

            if (platforms == null)
            {
                return StatusCode(500, "Server error while executing the operation");
            }

            return Ok(platforms);
        }

        #endregion Methods
    }
}