using System.ComponentModel.DataAnnotations;

namespace Expense_Tracker_WebApp.Models
{
    public class Currency
    {
        public int Id { get; set; }

        [Required]
        [StringLength(3)]
        public string Code { get; set; } // e.g., USD, EUR, JPY

        [Required]
        public decimal ExchangeRate { get; set; } // Exchange rate to PKR

        public DateTime LastUpdated { get; set; }
    }
}
