// I, Manh Truong Nguyen, student number 000893836, certify that this material is my
// original work. No other person's work has been used without due
// acknowledgement and I have not made my work available to anyone else.

namespace Assignment1.Controllers
{

    /// <summary>
    /// Manages CRUD operations for <see cref="Company"/> entities.
    /// </summary>
    [Authorize(Roles = "Supervisor,Employee")]
    public class CompaniesController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;

        /// <summary>
        /// Lists all companies (read-only for both roles).
        /// </summary>
        public async Task<IActionResult> Index()
        {
            return View(await _context.Companies.ToListAsync());
        }

        /// <summary>
        /// Shows details for a specific company.
        /// </summary>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Companies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        /// <summary>
        /// Displays the create form (Supervisor only).
        /// </summary>
        [Authorize(Roles = "Supervisor")]
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Creates a company (Supervisor only).
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Supervisor")]
        public async Task<IActionResult> Create([Bind("Id,Name,YearsInBusiness,Website,Province")] Company company)
        {
            if (ModelState.IsValid)
            {
                _context.Add(company);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }

        /// <summary>
        /// Displays the edit form for a company (Supervisor only).
        /// </summary>
        [Authorize(Roles = "Supervisor")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Companies.FindAsync(id);
            if (company == null)
            {
                return NotFound();
            }
            return View(company);
        }

        /// <summary>
        /// Updates a company (Supervisor only).
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Supervisor")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,YearsInBusiness,Website,Province")] Company company)
        {
            if (id != company.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(company);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyExists(company.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }

        /// <summary>
        /// Displays the delete confirmation view (Supervisor only).
        /// </summary>
        [Authorize(Roles = "Supervisor")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Companies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        /// <summary>
        /// Deletes a company (Supervisor only).
        /// </summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Supervisor")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company != null)
            {
                _context.Companies.Remove(company);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Checks if a company exists by id.
        /// </summary>
        private bool CompanyExists(int id)
        {
            return _context.Companies.Any(e => e.Id == id);
        }
    }
}
