namespace Expense_Tracker_WebApp.Models
{
    public class Payslip
    {
        public int PayslipId { get; set; }
        public DateTime MonthYear { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal TotalCommission { get; set; }
        public decimal TotalSalary { get; set; }
        public DateTime CreatedAt { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
    }

}
