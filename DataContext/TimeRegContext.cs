using Microsoft.EntityFrameworkCore;

namespace TimeReg_Api.DataContext
{
        public partial class TimeRegContext : DbContext
        {
            public TimeRegContext()
            {
            }

            public TimeRegContext(DbContextOptions<TimeRegContext> options)
              : base(options)
            {
            }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                if (!optionsBuilder.IsConfigured)
                {
                    optionsBuilder.UseNpgsql("Name=TimeReg");
                }
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.HasAnnotation("Relational:Collation", "en_US.utf8");

                OnModelCreatingPartial(modelBuilder);
            }
            partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
        }
}
