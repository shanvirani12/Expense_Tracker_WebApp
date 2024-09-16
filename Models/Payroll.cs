using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Expense_Tracker_WebApp.Models
{
    public class Payroll
    {
        [Key]
        public int PayrollId { get; set; }

        [Required]
        public string UserId { get; set; }
        public User User { get; set; }

        [Required]
        public DateTime MonthYear { get; set; } // To store the specific month and year

        [Column(TypeName = "decimal(18, 2)")]
        public double BasicSalary { get; set; } // Provided by the user

        [Column(TypeName = "decimal(18, 2)")]
        public double TotalCommission { get; set; } // Calculated as 5% commission from projects

        [Column(TypeName = "decimal(18, 2)")]
        public double TotalSalary
        {
            get
            {
                return BasicSalary + TotalCommission; // Total Salary is Basic + Commission
            }
        }

        public DateTime CreatedAt { get; set; }
    }
}
