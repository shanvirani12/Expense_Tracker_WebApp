using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Expense_Tracker_WebApp.Models;
using Expense_Tracker_WebApp.Data;

namespace Expense_Tracker_WebApp.Services
{
    public class PayrollService
    {
        private readonly ApplicationDbContext _context;

        public PayrollService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Payroll>> CalculatePayrollForMonth(DateTime monthYear, Dictionary<string, double> userSalaries)
        {
            var payrolls = new List<Payroll>();

            foreach (var userId in userSalaries.Keys)
            {
                double totalCommission = 0;

                // Get projects with status 'Completed' and matching closing date
                var completedProjects = await _context.Projects
                    .Include(p => p.Currency)
                    .Where(p => p.UserId == userId &&
                                p.Status == "Completed" && // Filter for completed projects
                                p.ClosingDate.HasValue &&
                                p.ClosingDate.Value.Year == monthYear.Year &&
                                p.ClosingDate.Value.Month == monthYear.Month)
                    .ToListAsync();

                foreach (var project in completedProjects)
                {

                    double BudgetInPKR = project.NetBudget * (double)project.Currency.ExchangeRate;
                    double profit = BudgetInPKR - (project.CostinPKR ?? 0);
                    totalCommission += profit * 0.05;
                }

                // Create payroll for the user
                var payroll = new Payroll
                {
                    UserId = userId,
                    MonthYear = new DateTime(monthYear.Year, monthYear.Month, 1),
                    BasicSalary = userSalaries[userId],
                    TotalCommission = totalCommission,
                    CreatedAt = DateTime.Now
                };

                payrolls.Add(payroll);
                _context.Payrolls.Add(payroll);
            }

            // Save changes to the database
            await _context.SaveChangesAsync();
            return payrolls;
        }

    }
}
