using Microsoft.EntityFrameworkCore;

namespace AMishnahADay.Models {
  public class AppDbContext : DbContext {
    public AppDbContext(DbContextOptions options) : base(options) { }

    public DbSet<Masechtah> Masechtahs { get; set; }
    public DbSet<Perek> Perakim { get; set; }
    public DbSet<Mishnah> Mishnayos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
      optionsBuilder.UseSqlite(@"Data Source=Mishnah.db");


    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      base.OnModelCreating(modelBuilder);

      
    }
  }
}
