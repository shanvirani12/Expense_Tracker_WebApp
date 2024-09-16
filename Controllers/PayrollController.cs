using Microsoft.AspNetCore.Mvc;
using Expense_Tracker_WebApp.Models;
using Expense_Tracker_WebApp.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Expense_Tracker_WebApp.Data;
using Microsoft.EntityFrameworkCore;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using System.IO;
using iText.Layout;

namespace Expense_Tracker_WebApp.Controllers
{
    public class PayrollController : Controller
    {
        private readonly PayrollService _payrollService;
        private readonly ApplicationDbContext _context;

        public PayrollController(PayrollService payrollService, ApplicationDbContext context)
        {
            _payrollService = payrollService;
            _context = context;
        }

        // Display form for entering basic salary
        public async Task<IActionResult> GeneratePayroll()
        {
            var users = await _context.Users.ToListAsync();
            var viewModel = new GeneratePayrollViewModel
            {
                Users = users,
                MonthYear = DateTime.Now
            };

            return View(viewModel);
        }

        // POST: Generate payroll based on entered salaries
        [HttpPost]
        public async Task<IActionResult> GeneratePayroll(GeneratePayrollViewModel model)
        {

            // Build user salary dictionary from the form
            var userSalaries = new Dictionary<string, double>();
            foreach (var userSalary in model.UserSalaries)
            {
                userSalaries[userSalary.UserId] = userSalary.Salary;
            }

            // Calculate payroll
            var payrolls = await _payrollService.CalculatePayrollForMonth(model.MonthYear, userSalaries);

            return RedirectToAction("ReviewPayroll", new { monthYear = model.MonthYear });
        }

        // Review the generated payroll before saving as PDF
        public async Task<IActionResult> ReviewPayroll(DateTime monthYear)
        {
            var payrolls = await _context.Payrolls
                .Include(p => p.User)
                .Where(p => p.MonthYear.Year == monthYear.Year && p.MonthYear.Month == monthYear.Month)
                .ToListAsync();

            return View(payrolls);
        }

        public async Task<IActionResult> DownloadPayrollPdf(DateTime monthYear)
        {
            var payrolls = await _context.Payrolls
                .Include(p => p.User)
                .Where(p => p.MonthYear.Year == monthYear.Year && p.MonthYear.Month == monthYear.Month)
                .ToListAsync();

            using (var memoryStream = new MemoryStream())
            {
                var writer = new PdfWriter(memoryStream);
                var pdf = new PdfDocument(writer);
                var document = new Document(pdf);

                document.Add(new Paragraph($"Payroll for {monthYear.ToString("MMMM yyyy")}")
                    .SetFontSize(16).SetBold());

                document.Add(new Table(4).AddCell("User")
                                            .AddCell("Basic Salary")
                                            .AddCell("Total Commission")
                                            .AddCell("Total Salary"));

                foreach (var payroll in payrolls)
                {
                    document.Add(new Table(4).AddCell($"{payroll.User.FirstName} {payroll.User.LastName}")
                                                .AddCell($"{payroll.BasicSalary:N2} PKR")
                                                .AddCell($"{payroll.TotalCommission:N2} PKR")
                                                .AddCell($"{payroll.TotalSalary:N2} PKR"));
                }

                document.Close();

                var fileBytes = memoryStream.ToArray();
                return File(fileBytes, "application/pdf", $"Payroll_{monthYear.ToString("yyyy_MM")}.pdf");
            }
        }

    }
}
