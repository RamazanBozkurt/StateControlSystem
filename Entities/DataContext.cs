using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace StateControlSystem.Entities
{
    public class DataContext : DbContext
    {
        public DataContext()
        {
        }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        public DbSet<InvoiceStatusLog> InvoiceStatusLogs { get; set; }
    }
}
