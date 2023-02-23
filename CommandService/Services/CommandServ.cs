using CommandService.Data.DTOs.Command;
using CommandService.Data.Entities;
using CommandService.Data.Repos;
using Microsoft.EntityFrameworkCore;

namespace CommandService.Services
{
    public class CommandServ
    {
        #region Constructors

        public CommandServ(CommandRepo commandRepo, PlatformRepo platformRepo)
        {
            _commandRepo = commandRepo;
            _platformRepo = platformRepo;
        }

        #endregion Constructors

        #region Properties

        private readonly CommandRepo _commandRepo;
        private readonly PlatformRepo _platformRepo;

        #endregion Properties

        #region Methods

        public async Task<ReadCommandDTO> GetCommandAsync(int platformId, int commandId)
        {
            try
            {
                bool platformExists = await _platformRepo.
                    PlatformExistsAsync(platformId);

                if (platformExists == false)
                {
                    return null;
                }

                Command command = await _commandRepo.
                    GetCommandForPlatformAsync(platformId, commandId);

                if (command == null)
                {
                    return null;
                }

                ReadCommandDTO result = new ReadCommandDTO()
                {
                    Id = command.Id,
                    Describtion = command.Describtion,
                    CommandLine = command.CommandLine,
                    PlatformId = command.PlatformId,
                };

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<ReadCommandDTO>> GetCommandsForPlatformAsync(int platformId)
        {
            try
            {
                bool platformExists = await _platformRepo.
                    PlatformExistsAsync(platformId);

                if (platformExists == false)
                {
                    return null;
                }

                List<ReadCommandDTO> result = await _commandRepo
                    .GetCommandsForPlatform(platformId)
                    .Select(c => new ReadCommandDTO()
                    {
                        Id = c.Id,
                        Describtion = c.Describtion,
                        CommandLine = c.CommandLine,
                        PlatformId = c.PlatformId
                    })
                    .ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ReadCommandDTO> AddCommandAsync(CreateCommandDTO dto)
        {
            try
            {
                bool platformExists = await _platformRepo.
                    PlatformExistsAsync(dto.PlatformId);

                if (platformExists == false)
                {
                    return null;
                }

                Command command = new Command()
                {
                    Describtion = dto.Describtion,
                    CommandLine = dto.CommandLine,
                    PlatformId = dto.PlatformId,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now
                };

                command = await _commandRepo.AddCommandAsync(command);

                ReadCommandDTO result = new ReadCommandDTO()
                {
                    Id = command.Id,
                    Describtion = command.Describtion,
                    CommandLine = command.CommandLine,
                    PlatformId = command.PlatformId
                };

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> UpdateCommandAsync(UpdateCommandDTO dto)
        {
            try
            {
                bool platformExists = await _platformRepo.
                        PlatformExistsAsync(dto.NewPlatformId);

                if (platformExists == false)
                {
                    return false;
                }

                Command oldCommand = await _commandRepo.
                    GetCommandForPlatformAsync(dto.CurrentPlatformId, dto.Id);

                if (oldCommand == null)
                {
                    return false;
                }

                oldCommand.Describtion = dto.Describtion;
                oldCommand.CommandLine = dto.CommandLine;
                oldCommand.PlatformId = dto.NewPlatformId;
                oldCommand.UpdatedDate = DateTime.Now;

                await _commandRepo.UpdateCommandAsync(oldCommand);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> RemoveCommandAsync(int platformId, int commandId)
        {
            try
            {
                bool platformExists = await _platformRepo.
                    PlatformExistsAsync(platformId);

                if (platformExists == false)
                {
                    return false;
                }

                Command command = await _commandRepo.GetCommandForPlatformAsync(platformId, commandId);

                if (command == null)
                {
                    return false;
                }

                command.DeletedDate = DateTime.Now;

                await _commandRepo.UpdateCommandAsync(command);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion Methods
    }
}