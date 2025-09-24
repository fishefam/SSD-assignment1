namespace Assignment1.Data
{
    /// <summary>
    /// Entity Framework Core context for the application, integrating ASP.NET Core Identity
    /// via IdentityDbContext{TUser} with ApplicationUser.
    /// </summary>
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<Company> Companies { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
