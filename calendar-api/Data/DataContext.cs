using calendar_api.Models;
using Microsoft.EntityFrameworkCore;

namespace calendar_api.Data
{
    public class DataContext : DbContext
    {
        public DataContext() : base() { }
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<CalendarTask> Tasks { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
