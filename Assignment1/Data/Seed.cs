namespace Assignment1.Data
{
    /// <summary>
    /// Provides startup seeding for Identity roles/users and sample Company data.
    /// </summary>
    public static class Seed
    {
        /// <summary>
        /// Ensures the required roles (Supervisor, Employee) exist and creates one user for each role.
        /// </summary>
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            string[] roleNames = { "Supervisor", "Employee" };

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            if (await userManager.FindByEmailAsync("supervisor@test.com") == null)
            {
                var supervisor = new ApplicationUser
                {
                    UserName = "supervisor@test.com",
                    Email = "supervisor@test.com",
                    FirstName = "Super",
                    LastName = "Visor",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(supervisor, "Supervisor123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(supervisor, "Supervisor");
                }
            }

            if (await userManager.FindByEmailAsync("employee@test.com") == null)
            {
                var employee = new ApplicationUser
                {
                    UserName = "employee@test.com",
                    Email = "employee@test.com",
                    FirstName = "Emp",
                    LastName = "Loyee",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(employee, "Employee123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(employee, "Employee");
                }
            }
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
}
