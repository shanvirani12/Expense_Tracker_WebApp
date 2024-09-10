using Expense_Tracker_WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Expense_Tracker_WebApp.Data;
using Microsoft.AspNetCore.Identity;

namespace Expense_Tracker.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public DashboardController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<ActionResult> Index()
        {
            //Last 7 Days



            int TotalTodayBid = await _context.Bids
                .CountAsync(b => b.DateTime.Date == DateTime.Today && b.UserId == _userManager.GetUserId(User));
            ViewBag.TotalTodayBid = TotalTodayBid.ToString();

            int TotalProjectbyUser = await _context.Projects
                .CountAsync(p => p.Bid.UserId == _userManager.GetUserId(User));
            ViewBag.TotalProjectbyUser = TotalProjectbyUser.ToString();

            // Total Bids per User for Today
            var doughnutChartData = await _context.Bids
            .Where(b => b.DateTime.Date == DateTime.Today)
            .GroupBy(b => new { b.User.FirstName, b.User.LastName })
            .Select(group => new
            {
                UserName = $"{group.Key.FirstName} {group.Key.LastName}",
                BidCount = group.Count()
            })
            .OrderByDescending(x => x.BidCount)
            .ToListAsync();

            // Check if all bids are zero
            bool allBidsZero = doughnutChartData.All(data => data.BidCount == 0);
            ViewBag.AllBidsZero = allBidsZero;

            // Serialize data to JSON
            ViewBag.DoughnutChartLabels = Newtonsoft.Json.JsonConvert.SerializeObject(doughnutChartData.Select(d => d.UserName).ToList());
            ViewBag.DoughnutChartData = Newtonsoft.Json.JsonConvert.SerializeObject(doughnutChartData.Select(d => d.BidCount).ToList());



            // Fetch all bids and projects with associated users
            var selectedBids = _context.Bids
                .Include(b => b.User) // Include User data
                .ToList();

            var selectedProjects = _context.Projects
                .Include(p => p.Bid) // Include Bid data
                .ThenInclude(b => b.User) // Include User data via Bid
                .ToList();

            // Aggregate bids by user
            var bidsSummary = selectedBids
                .GroupBy(b => b.User.FirstName) // Aggregate by user
                .Select(g => new
                {
                    Username = g.Key,
                    Bids = g.Count()
                })
                .ToList();

            // Aggregate projects by user
            var projectsSummary = selectedProjects
                .GroupBy(p => p.Bid.User.FirstName) // Aggregate by user via Bid
                .Select(g => new
                {
                    Username = g.Key,
                    Projects = g.Count()
                })
                .ToList();

            // Combine Bids & Projects
            var userList = bidsSummary
                .Select(b => b.Username)
                .Union(projectsSummary.Select(p => p.Username))
                .ToList();

            var combinedData = from user in userList
                               join bid in bidsSummary on user equals bid.Username into bidJoin
                               from bid in bidJoin.DefaultIfEmpty()
                               join project in projectsSummary on user equals project.Username into projectJoin
                               from project in projectJoin.DefaultIfEmpty()
                               select new
                               {
                                   User = user,
                                   Bids = bid?.Bids ?? 0,
                                   Projects = project?.Projects ?? 0
                               };

            ViewBag.Users = Newtonsoft.Json.JsonConvert.SerializeObject(combinedData.Select(d => d.User).ToList());
            ViewBag.Bids = Newtonsoft.Json.JsonConvert.SerializeObject(combinedData.Select(d => d.Bids).ToList());
            ViewBag.Projects = Newtonsoft.Json.JsonConvert.SerializeObject(combinedData.Select(d => d.Projects).ToList());






            // Fetch recent projects with related entities
            ViewBag.RecentProjects = await _context.Projects
                .Include(p => p.Bid) // Include the Bid information
                    .ThenInclude(b => b.Account) // Include the Account information within Bid
                .Include(p => p.Bid) // Include Bid again to access User information
                    .ThenInclude(b => b.User) // Include the User information within Bid
                .OrderByDescending(p => p.Bid.DateTime) // Sort by the Bid's DateTime property
                .Take(5) // Get the top 5 recent projects
                .ToListAsync();




            return View();
        }
    }

    public class SplineChartData
    {
        public string day { get; set; }
        public int bids { get; set; }
        public int projects { get; set; }
    }

}