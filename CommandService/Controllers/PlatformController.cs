using CommandService.Data.DTOs.Platform;
using CommandService.Services;
using CommandService.ViewModels.Platform;
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
        /// <param name="filterViewModel">Object containing information about the paging, filtering and order</param>
        /// <remarks>
        /// The number of the first page is 1 and the minimal size of the page is also 1.
        ///
        /// The "SortInfo" parameter's key should be "Name" and its value - "asc" or "desc" depending on the desired sort order.
        /// If this parameter is not provided, the platforms will be sorted by name ascendingly.
        ///
        /// This method returns all platforms staring with the value of the "NameFilterValue" parameter (not case sensitive).
        /// </remarks>
        /// <returns>Object containing a list of platforms along with information about paging, filtering and order</returns>
        /// <response code="500">Error message</response>
        /// <response code="200">Object containing a list of platforms along with information about paging, filtering and order</response>
        [HttpPost]
        [Route("c/Platform/GetPlatformsAsync")]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(ReadPlatformsResponseDTO), 200)]
        public async Task<IActionResult> GetPlatformsAsync(ReadPlatformsViewModel filterViewModel)
        {
            PlatformFilteringDTO dto = new PlatformFilteringDTO()
            {
                PageSize = filterViewModel.PageSize,
                PageNumber = filterViewModel.PageNumber,
                SortInfo = filterViewModel.SortInfo,
                NameFilterValue = filterViewModel.NameFilterValue,
            };

            var response = await _platformService.GetPlatformsAsync(dto);

            if (response == null)
            {
                return StatusCode(500, "Server error while executing the operation");
            }

            return Ok(response);
        }

        #endregion Methods
    }
}