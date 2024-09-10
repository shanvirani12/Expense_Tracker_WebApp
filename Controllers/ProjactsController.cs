using Expense_Tracker_WebApp.Data;
using Expense_Tracker_WebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Expense_Tracker_WebApp.Controllers
{
    public class ProjactsController : Controller
    {

        private readonly ApplicationDbContext _context;

        public ProjactsController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: ProjactsController
        public async Task<IActionResult> Index(int? accountId, string userEmail)
        {
            var projectsQuery = _context.Projacts
                .Include(p => p.Account)
                .Include(p => p.User)
                .AsQueryable();

            if (accountId.HasValue)
            {
                projectsQuery = projectsQuery.Where(p => p.AccountID == accountId.Value);
            }

            if (!string.IsNullOrEmpty(userEmail))
            {
                projectsQuery = projectsQuery.Where(p => p.User.Email == userEmail);
            }

            var projacts = await projectsQuery.ToListAsync();

            var accounts = await _context.Accounts.ToListAsync();
            var users = await _context.Users.ToListAsync();

            ViewData["Accounts"] = new SelectList(accounts, "AccountID", "AccountName");
            ViewData["Users"] = new SelectList(users, "UserEmail", "UserEmail");

            return View(projacts);
        }

        // GET: Projects/SearchBids
        public async Task<IActionResult> SearchBids(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return Json(new { });
            }

            var bids = await _context.Bids
                .Include(b => b.User)
                .Include(b => b.Account)
                .Where(b => b.Link.Contains(query))
                .Select(b => new
                {
                    id = b.BidId,
                    link = b.Link,
                    user = b.User.UserName,
                    account = b.Account.Name
                })
                .ToListAsync();

            return Json(bids);
        }

        // GET: ProjactsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProjactsController/Create
        public async Task<IActionResult> Create()
        {
            var currencies = await _context.Currencies.ToListAsync();
            ViewData["Currencies"] = new SelectList(currencies, "Code", "Code");
            var model = new Projects();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Projects project)
        {
            if (ModelState.IsValid)
            {
                _context.Add(project);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            // Repopulate currency dropdown if model state is invalid
            ViewData["Currencies"] = new SelectList(await _context.Currencies.ToListAsync(), "Code", "Code");
            return View(project);
        }

        // GET: ProjactsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProjactsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProjactsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProjactsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
