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
    }
}