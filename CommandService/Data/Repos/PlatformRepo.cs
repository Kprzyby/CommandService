using CommandService.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CommandService.Data.Repos
{
    public class PlatformRepo
    {
        #region Constructors

        public PlatformRepo(DataContext context)
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

        public async Task<Platform> GetPlatformByIdAsync(int id)
        {
            Platform result = await _context.Platforms
                .SingleOrDefaultAsync(p => p.Id == id);

            return result;
        }

        public async Task<Platform> GetPlatformByExternalIdAsync(int id)
        {
            Platform result = await _context.Platforms
                .SingleOrDefaultAsync(p => p.ExternalId == id);

            return result;
        }

        public IQueryable<Platform> GetPlatforms()
        {
            IQueryable<Platform> result = _context.Platforms;

            return result;
        }

        public async Task<Platform> AddPlatformAsync(Platform platform)
        {
            await _context.Platforms
                .AddAsync(platform);

            await SaveChangesAsync();

            return platform;
        }

        public async Task UpdatePlatformAsync(Platform platform)
        {
            _context.Platforms
                .Update(platform);

            await SaveChangesAsync();
        }

        public async Task RemovePlatformAsync(Platform platform)
        {
            _context.Platforms
                .Remove(platform);

            await SaveChangesAsync();
        }

        #endregion Methods
    }
}