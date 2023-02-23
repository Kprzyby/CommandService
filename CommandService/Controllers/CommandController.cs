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

        [HttpGet("c/Platform/{platformId}/Command/GetCommandForPlatformAsync/{commandId}",
            Name = "GetCommandForPlatformAsync")]
        public async Task<IActionResult> GetCommandForPlatformAsync(int platformId, int commandId)
        {
            var command = await _commandService.GetCommandAsync(platformId, commandId);

            if (command == null)
            {
                return NotFound("Command not found");
            }

            return Ok(command);
        }

        [HttpGet]
        [Route("c/Platform/{platformId}/Command/GetCommandsForPlatformAsync")]
        public async Task<IActionResult> GetCommandsForPlatformAsync(int platformId)
        {
            var commands = await _commandService.GetCommandsForPlatformAsync(platformId);

            if (commands == null)
            {
                return NotFound("Platform doesn't exist");
            }

            return Ok(commands);
        }

        [HttpPost]
        [Route("c/Platform/{platformId}/Command/AddCommandAsync")]
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

        [HttpPut]
        [Route("c/Platform/{currentPlatformId}/Command/UpdateCommandAsync")]
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
                PlatformId = command.PlatformId
            };

            bool result = await _commandService.UpdateCommandAsync(dto);

            if (result == false)
            {
                return NotFound("The platform or the command doesn't exist");
            }

            return StatusCode(204);
        }

        [HttpDelete]
        [Route("c/Platform/{platformId}/Command/RemoveCommandForPlatformAsync/{commandId}")]
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