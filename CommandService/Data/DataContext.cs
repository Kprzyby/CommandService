using CommandService.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CommandService.Data
{
    public class DataContext : DbContext
    {
        #region Constructors

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        #endregion Constructors

        #region Properties

        public DbSet<Command> Commands { get; set; }
        public DbSet<Platform> Platforms { get; set; }

        #endregion Properties
    }
}