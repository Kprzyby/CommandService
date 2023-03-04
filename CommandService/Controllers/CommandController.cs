using CommandService.Data.DTOs.Command;
using CommandService.Services;
using CommandService.ViewModels.Command;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers
{
    [ApiController]
    public class CommandController : ControllerBase
    {
        #region Constructors

        public CommandController(CommandServ commandService)
        {
            _commandService = commandService;
        }

        #endregion Constructors

        #region Properties

        private readonly CommandServ _commandService;

        #endregion Properties

        #region Methods

        /// <summary>
        /// Asynchronous method for loading the command specified by an id for the specified platform
        /// </summary>
        /// <param name="platformId">The id of the platform</param>
        /// <param name="commandId">The id of the command</param>
        /// <returns>The command specified by an id for the chosen platform</returns>
        /// <response code="404">Error message</response>
        /// <response code="200">Object containing information about the command</response>

        [HttpGet("c/Platform/{platformId}/Command/GetCommandForPlatformAsync/{commandId}",
            Name = "GetCommandForPlatformAsync")]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(List<ReadCommandDTO>), 200)]
        public async Task<IActionResult> GetCommandForPlatformAsync(int platformId, int commandId)
        {
            var command = await _commandService.GetCommandAsync(platformId, commandId);

            if (command == null)
            {
                return NotFound("Command not found");
            }

            return Ok(command);
        }

        /// <summary>
        /// Asynchronous method for loading all commands for the platform specified by an id
        /// </summary>
        /// <param name="platformId">The id of the platform</param>
        /// <param name="filteringViewModel">Object containing information about the paging, filtering and order</param>
        /// <remarks>
        /// The number of the first page is 1 and the minimal size of the page is also 1.
        ///
        /// The "SortInfo" parameter's key should be "CommandLine" and its value - "asc" or "desc" depending on the desired sort order.
        /// If this parameter is not provided, the commands will be sorted by command lines ascendingly.
        ///
        /// The filters will return commands that start with the given values (not case sensitive).
        /// </remarks>
        /// <returns>Object containing a list of commands along with information about paging, filtering and order</returns>
        /// <response code="404">Error message</response>
        /// <response code="200">Object containing a list of commands along with information about paging, filtering and order</response>
        [HttpPost]
        [Route("c/Platform/{platformId}/Command/GetCommandsForPlatformAsync")]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(ReadCommandsResponseDTO), 200)]
        public async Task<IActionResult> GetCommandsForPlatformAsync(int platformId, ReadCommandsViewModel filteringViewModel)
        {
            CommandFilteringDTO dto = new CommandFilteringDTO()
            {
                PageSize = filteringViewModel.PageSize,
                PageNumber = filteringViewModel.PageNumber,
                SortInfo = filteringViewModel.SortInfo,
                DescribtionFilterValue = filteringViewModel.DescribtionFilterValue,
                CommandLineFilterValue = filteringViewModel.CommandLineFilterValue
            };

            var response = await _commandService.GetCommandsForPlatformAsync(platformId, dto);

            if (response == null)
            {
                return NotFound("Platform doesn't exist");
            }

            return Ok(response);
        }

        /// <summary>
        /// Asynchronous method for adding a command for the specified platform
        /// </summary>
        /// <param name="platformId">The id of the platform</param>
        /// <param name="newCommand">Object containing information about the new platform</param>
        /// <remarks>The ,,Describtion" field in the ,,newCommand" argument is nullable</remarks>
        /// <returns>Object containing information about the created platform and a route to the
        /// GetCommandForPlatformAsync method with all the routing information</returns>
        /// <response code="400">Error message</response>
        /// <response code="404">Error message</response>
        /// <response code="201">Object containing information about the created platform and a route to the
        /// GetCommandForPlatformAsync method with all the routing information</response>
        [HttpPost]
        [Route("c/Platform/{platformId}/Command/AddCommandAsync")]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(ReadCommandDTO), 201)]
        public async Task<IActionResult> AddCommandAsync(int platformId, CreateCommandViewModel newCommand)
        {
            if (newCommand == null)
            {
                return BadRequest("The newCommand argument was not provided");
            }

            CreateCommandDTO dto = new CreateCommandDTO()
            {
                Describtion = newCommand.Describtion,
                CommandLine = newCommand.CommandLine,
                PlatformId = platformId
            };

            ReadCommandDTO result = await _commandService.AddCommandAsync(dto);

            if (result == null)
            {
                return NotFound("The platform doesn't exist");
            }

            return CreatedAtRoute("GetCommandForPlatformAsync",
                new { platformId = platformId, commandId = result.Id }, result);
        }

        /// <summary>
        /// Asynchronous method for updating the command for a specified platform
        /// </summary>
        /// <param name="currentPlatformId">The id of the platform the command is currently associated with</param>
        /// <param name="command">Object containing new information about the command</param>
        /// <remarks>The ,,Describtion" field in the ,,command" argument is nullable</remarks>
        /// <returns>Nothing if operation executes correctly or an error message if it doesn't</returns>
        /// <response code="400">Error message</response>
        /// <response code="404">Error message</response>
        /// <response code="204"></response>
        [HttpPut]
        [Route("c/Platform/{currentPlatformId}/Command/UpdateCommandAsync")]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(void), 204)]
        public async Task<IActionResult> UpdateCommandAsync(int currentPlatformId, UpdateCommandViewModel command)
        {
            if (command == null)
            {
                return BadRequest("The command argument was not provided");
            }

            UpdateCommandDTO dto = new UpdateCommandDTO()
            {
                Id = command.Id,
                Describtion = command.Describtion,
                CommandLine = command.CommandLine,
                CurrentPlatformId = currentPlatformId,
                NewPlatformId = command.NewPlatformId
            };

            bool result = await _commandService.UpdateCommandAsync(dto);

            if (result == false)
            {
                return NotFound("The platform or the command doesn't exist");
            }

            return StatusCode(204);
        }

        /// <summary>
        /// Asynchronous method for removing the command specified by an id for a specified platform
        /// </summary>
        /// <param name="platformId">The id of the platform</param>
        /// <param name="commandId">The id of the command</param>
        /// <returns>Nothing if operation executes correctly or an error message if it doesn't</returns>
        /// <response code="404">Error message</response>
        /// <response code="204"></response>
        [HttpDelete]
        [Route("c/Platform/{platformId}/Command/RemoveCommandForPlatformAsync/{commandId}")]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(void), 204)]
        public async Task<IActionResult> RemoveCommandForPlatformAsync(int platformId, int commandId)
        {
            bool result = await _commandService.RemoveCommandAsync(platformId, commandId);

            if (result == false)
            {
                return NotFound("The platform or the command doesn't exist");
            }

            return StatusCode(204);
        }

        #endregion Methods
    }
}