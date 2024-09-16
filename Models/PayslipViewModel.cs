namespace Expense_Tracker_WebApp.Models
{
    public class PayslipViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal TotalCommission { get; set; }
        public decimal TotalSalary { get; set; }
    }
}
