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

        [HttpGet]
        [Route("c/Platform/GetPlatformsAsync")]
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