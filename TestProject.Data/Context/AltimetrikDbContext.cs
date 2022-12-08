
namespace TestProject.Data.Context
{
	public class AltimetrikDbContext : DbContext
	{
		public AltimetrikDbContext(DbContextOptions options) : base(options)
		{

		}

		public DbSet<User> Users { get; set; }
		public DbSet<Account> Accounts { get; set; }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<User>(entity =>
			{
				entity.Property(e => e.Name).HasMaxLength(90);

				entity.HasIndex(e => e.Email).IsUnique();

				entity.Property(e => e.Name).HasMaxLength(120);
			});

			modelBuilder.Entity<Account>(entity =>
			{
				entity.Property(e => e.Name).HasMaxLength(50);

				entity.HasOne<User>().WithMany()
					.HasForeignKey(d => d.UserId);
			});
		}
	}
}
