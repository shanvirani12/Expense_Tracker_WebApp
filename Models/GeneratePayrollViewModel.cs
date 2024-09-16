using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Expense_Tracker_WebApp.Models
{
    public class GeneratePayrollViewModel
    {
        public DateTime MonthYear { get; set; }

        public List<User> Users { get; set; }

        public List<UserSalary> UserSalaries { get; set; } = new List<UserSalary>();
    }

    public class UserSalary
    {
        public string UserId { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Please enter a valid salary")]
        public double Salary { get; set; }
    }
}
