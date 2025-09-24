namespace Assignment1.Data
{
    /// <summary>
    /// Provides startup seeding for Identity roles/users and sample Company data.
    /// </summary>
    public static class Seed
    {
        /// <summary>
        /// Ensures the required roles exist and creates seed users using configuration-backed credentials.
        /// </summary>
        public static async Task SeedAsync(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<IdentitySeedOptions> seedOptions,
            IWebHostEnvironment env // optional: to enforce stricter checks in Production
        )
        {
            string[] roleNames = { "Supervisor", "Employee" };

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                    await roleManager.CreateAsync(new IdentityRole(roleName));
            }

            var opts = seedOptions.Value;

            // helper local function to seed a user if missing
            static async Task CreateIfMissingAsync(UserManager<ApplicationUser> um, string email, string password, string role)
            {
                var existing = await um.FindByEmailAsync(email);
                if (existing != null) return;

                var user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    FirstName = role == "Supervisor" ? "Super" : "Emp",
                    LastName = role == "Supervisor" ? "Visor" : "Loyee",
                    EmailConfirmed = true
                };

                var result = await um.CreateAsync(user, password);
                if (result.Succeeded)
                    await um.AddToRoleAsync(user, role);
                else
                    throw new InvalidOperationException(
                        $"Failed to create seed user '{email}': {string.Join("; ", result.Errors.Select(e => e.Description))}");
            }

            // Basic safety: don’t allow empty secrets in Production
            if (env.IsProduction())
            {
                if (string.IsNullOrWhiteSpace(opts.SupervisorEmail) || string.IsNullOrWhiteSpace(opts.SupervisorPassword) ||
                    string.IsNullOrWhiteSpace(opts.EmployeeEmail) || string.IsNullOrWhiteSpace(opts.EmployeePassword))
                {
                    throw new InvalidOperationException(
                        "Seed:Identity settings are missing in Production. Set environment variables (Seed__Identity__...).");
                }
            }

            if (!string.IsNullOrWhiteSpace(opts.SupervisorEmail) && !string.IsNullOrWhiteSpace(opts.SupervisorPassword))
                await CreateIfMissingAsync(userManager, opts.SupervisorEmail, opts.SupervisorPassword, "Supervisor");

            if (!string.IsNullOrWhiteSpace(opts.EmployeeEmail) && !string.IsNullOrWhiteSpace(opts.EmployeePassword))
                await CreateIfMissingAsync(userManager, opts.EmployeeEmail, opts.EmployeePassword, "Employee");
        }

        /// <summary>
        /// Seeds sample Company records if they do not already exist.
        /// </summary>
        public static async Task SeedCompaniesAsync(ApplicationDbContext context)
        {
            await context.Database.MigrateAsync();

            if (!await context.Companies.AnyAsync(c => c.Name == "Company 1"))
            {
                context.Companies.Add(new Company
                {
                    Name = "Company 1",
                    YearsInBusiness = 15,
                    Website = "https://www.company-1.com",
                    Province = "ON"
                });
            }

            if (!await context.Companies.AnyAsync(c => c.Name == "Google"))
            {
                context.Companies.Add(new Company
                {
                    Name = "Google",
                    YearsInBusiness = 20,
                    Website = "https://www.google.com",
                    Province = "CA"
                });
            }

            if (!await context.Companies.AnyAsync(c => c.Name == "Company 2"))
            {
                context.Companies.Add(new Company
                {
                    Name = "Company 2",
                    YearsInBusiness = 22,
                    Website = "https://www.company-2.com"
                });
            }

            await context.SaveChangesAsync();
        }
    }

    public class IdentitySeedOptions
    {
        public string SupervisorEmail { get; set; } = "";
        public string SupervisorPassword { get; set; } = "";
        public string EmployeeEmail { get; set; } = "";
        public string EmployeePassword { get; set; } = "";
    }
}
