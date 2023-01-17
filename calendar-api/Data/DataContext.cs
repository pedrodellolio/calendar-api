using calendar_api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace calendar_api.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext() : base() { }
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<CalendarTask> Tasks { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
