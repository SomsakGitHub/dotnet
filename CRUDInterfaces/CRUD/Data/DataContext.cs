namespace CRUD.Models
{
    public class DataContext : DbContext
    {
        public DataContext()
        {
        }

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public virtual DbSet<Car> Car { get; set; } = null!;
    }
}
