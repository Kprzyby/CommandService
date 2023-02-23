using CommandService.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CommandService.Data.Repos
{
    public class CommandRepo
    {
        #region Constructors

        public CommandRepo(DataContext context)
        {
            _context = context;
        }

        #endregion Constructors

        #region Properties

        private readonly DataContext _context;

        #endregion Properties

        #region Methods

        private async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Command> GetCommandForPlatformAsync(int platformId, int commandId)
        {
            Command result = await _context.Commands
                .Where(c => c.DeletedDate == null)
                .SingleOrDefaultAsync(c => c.Id == commandId && c.PlatformId == platformId);

            return result;
        }

        public IQueryable<Command> GetCommandsForPlatform(int platformId)
        {
            IQueryable<Command> result = _context.Commands
                .Where(c => c.DeletedDate == null && c.PlatformId == platformId);

            return result;
        }

        public async Task<Command> AddCommandAsync(Command command)
        {
            await _context.Commands.AddAsync(command);

            await _context.SaveChangesAsync();

            return command;
        }

        public async Task UpdateCommandAsync(Command command)
        {
            _context.Commands.Update(command);

            await SaveChangesAsync();
        }

        #endregion Methods
    }
}