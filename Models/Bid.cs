using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Expense_Tracker_WebApp.Models
{
    public class Bid
    {
        [Key]
        public int BidId { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Link { get; set; }
        // Foreign key for relationship with Employee
        public string UserId { get; set; }
        public User User { get; set; }
        public int AccountID { get; set; }
        public Account Account { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
    }
}
