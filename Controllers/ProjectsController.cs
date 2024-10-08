﻿using Expense_Tracker_WebApp.Data;
using Expense_Tracker_WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Expense_Tracker_WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProjectsController : Controller
    {
        private readonly ApplicationDbContext _context; // Your DbContext

        public ProjectsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProjactsController
        public async Task<IActionResult> Index(int? accountId, string userEmail)
        {
            var projectsQuery = _context.Projects
                .Include(p => p.Account)
                .Include(p => p.User)
                .Include(p => p.Currency)
                .AsQueryable();

            if (accountId.HasValue)
            {
                projectsQuery = projectsQuery.Where(p => p.Account.Id == accountId.Value);
            }

            if (!string.IsNullOrEmpty(userEmail))
            {
                projectsQuery = projectsQuery.Where(p => p.User.Email == userEmail);
            }

            var projects = await projectsQuery.ToListAsync();

            var accounts = await _context.Accounts.ToListAsync();
            var users = await _context.Users.ToListAsync();
            var currencies = await _context.Currencies.ToListAsync();

            ViewData["Accounts"] = new SelectList(accounts, "AccountID", "AccountName");
            ViewData["Users"] = new SelectList(users, "UserEmail", "UserEmail");
            ViewBag.Currencies = currencies;
            return View(projects);
        }


        // GET: Projects/Create
        public IActionResult Create()
        {
            ViewData["Bids"] = new SelectList(_context.Bids.Include(b => b.User).Include(b => b.Account), "BidId", "Link");
            ViewData["CurrencyId"] = new SelectList(_context.Currencies, "Id", "Code");
            return View();
        }

        // POST: Projects/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Projects project)
        {          
            project.User = await _context.Users.FindAsync(project.UserId);
            project.Account = await _context.Accounts.FindAsync(project.AccountID);
            project.Currency = await _context.Currencies.FindAsync(project.CurrencyId);
            _context.Add(project);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
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
                    username = b.User.UserName,
                    accountname = b.Account.Name,
                    userId = b.User.Id,
                    accountId = b.Account.Id,
                    user = b.User,
                    account = b.Account
                    
                })
                .ToListAsync();

            return Json(bids);
        }

        [HttpGet]
        public IActionResult CalculateBudget(double grossBudget, int currencyId, bool isRecruiter)
        {
            var currency = _context.Currencies.Find(currencyId);

            if (currency == null)
            {
                return Json(new { success = false, message = "Currency not found." });
            }

            // Calculate platform fee based on IsRecruiter status
            double platformFee = isRecruiter ? grossBudget * 0.15 : grossBudget * 0.10;
            double netBudget = grossBudget - platformFee;
            double budgetInPKR = netBudget * (double)currency.ExchangeRate;

            return Json(new
            {
                success = true,
                netBudget = netBudget,
                budgetInPKR = budgetInPKR
            });
        }


        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .Include(p => p.Account)
                .Include(p => p.User)
                .Include(p => p.Currency) // Load related Currency entity
                .FirstOrDefaultAsync(p => p.ProjectId == id);

            if (project == null)
            {
                return NotFound();
            }

            // Ensure Currencies list is not null
            var currencies = await _context.Currencies.ToListAsync();
            if (currencies == null)
            {
                return NotFound(); // Or handle this case as needed
            }

            ViewBag.BudgetInPKR = project.NetBudget * (double)project.Currency.ExchangeRate;
            ViewData["Currencies"] = new SelectList(currencies, "Id", "Code", project.CurrencyId);
            return View(project);
        }



        // POST: Projects/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,Projects project)
        {
            if (id != project.ProjectId)
            {
                return NotFound();
            }

            
                try
                {
                    _context.Update(project);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.ProjectId))
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

        // GET: Projects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .Include(p => p.Currency)
                .FirstOrDefaultAsync(p => p.ProjectId == id);

            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.ProjectId == id);
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .Include(p => p.Currency)  // Load related Currency entity
                .Include(p => p.Account)    // Load related Account entity
                .Include(p => p.User)       // Load related User entity
                .FirstOrDefaultAsync(p => p.ProjectId == id);

            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }


    }
}
