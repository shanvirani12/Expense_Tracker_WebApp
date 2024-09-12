using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Expense_Tracker_WebApp.Models
{
    public class Projects
    {
        [Key]
        public int ProjectId { get; set; }

        [Column(TypeName = "nvarchar(200)")]
        public string ProjectName { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string ProjectClientName { get; set; }

        [Column(TypeName = "nvarchar(300)")]
        public string ProjectLink { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string ProjectType { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public int AccountID { get; set; }
        public Account Account { get; set; }

        public DateTime AwardDate { get; set; }

        public bool IsRecruiter { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Status { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public double GrossBudget { get; set; } // Total budget of the project

        [Column(TypeName = "decimal(18, 2)")]
        public double NetBudget
        {
            get
            {
                return GrossBudget - PlatformFee; // Deduct the platform fee
            }
            set { }
        }

        public int CurrencyId { get; set; }
        public Currency Currency { get; set; } // Linked to the Currency model

        [NotMapped]
        public double BudgetInPKR
        {
            get;set;
        }

        public DateTime? ClosingDate { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string? AssignedTo { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public double? CostinPKR { get; set; } // Designer or developer fee

        [NotMapped]
        public double PlatformFee
        {
            get
            {
                return IsRecruiter ? GrossBudget * 0.15 : GrossBudget * 0.10; // Calculate platform fee
            }
        }
    }

}
