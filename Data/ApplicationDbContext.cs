﻿using Expense_Tracker_WebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Expense_Tracker_WebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<Bid> Bids { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Projects> Projects { get; set; }
        public DbSet<Payslip> Payslips { get; set; }
        public DbSet<Payroll> Payrolls { get; set; }

        public ApplicationDbContext(DbContextOptions options)
             : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

        }

    }
}
