namespace CRUD.Models
{
    public class DataContext : DbContext
    {
        public DataContext()
        {
        }

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public virtual DbSet<SuperHero> SuperHeroes { get; set; } = null!;
    }
}
