using Microsoft.AspNetCore.Mvc;
using Expense_Tracker_WebApp.Models;
using System.Linq;
using System.Threading.Tasks;
using Expense_Tracker_WebApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Expense_Tracker_WebApp.Controllers
{
    [Authorize]
    public class BidsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public BidsController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Step 1: Account Selection Page
        public IActionResult SelectAccount()
        {
            ViewBag.Accounts = _context.Accounts.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult SelectAccount(int accountId)
        {
            if (accountId > 0)
            {
                return RedirectToAction("Index", new { accountId });
            }

            ViewBag.Accounts = _context.Accounts.ToList();
            ModelState.AddModelError("", "Please select a valid account.");
            return View();
        }

        // Step 2: Main Bid Screen
        public IActionResult Index(int? accountId)
        {
            string userId = _userManager.GetUserId(User);
            // Redirect to SelectAccount if accountId is not provided
            if (!accountId.HasValue || accountId <= 0)
            {
                return RedirectToAction("SelectAccount");
            }

            var todayBids = _context.Bids
                                    .Where(b => b.AccountID == accountId && b.UserId == userId && b.DateTime.Date == DateTime.Today)
                                    .OrderByDescending(b => b.DateTime)
                                    .ToList();

            ViewBag.AccountId = accountId;
            ViewBag.Accounts = _context.Accounts.ToList();
            return View(todayBids);
        }

        [HttpPost]
        public async Task<IActionResult> AddBid(int accountId, string link)
        {
            string userId = _userManager.GetUserId(User);
            if (ModelState.IsValid)
            {
                var bid = new Bid
                {
                    AccountID = accountId,
                    Link = link,
                    UserId = userId,
                    DateTime = DateTime.Now
                };

                _context.Add(bid);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { accountId });
            }

            // If we got this far, something failed; redisplay form.
            var todayBids = _context.Bids
                                    .Where(b => b.AccountID == accountId && b.UserId == userId && b.DateTime.Date == DateTime.Today)
                                    .OrderByDescending(b => b.DateTime)
                                    .ToList();

            ViewBag.AccountId = accountId;
            ViewBag.Accounts = _context.Accounts.ToList();
            return View("Index", todayBids);
        }
    }
}
