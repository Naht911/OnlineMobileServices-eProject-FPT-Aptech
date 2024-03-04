using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineMobileServices_Models.Models
{
    public class RechargePackageHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RechargePackageHistoryID { get; set; }
        [Required]
        [Column(TypeName = "varchar(10)")]
        public string MobileNumber { get; set; }
        [Required]
        public int UserID { get; set; }
        [ForeignKey("UserID")]
        public User User { get; set; }
        [Required]
        public int RechargePackageID { get; set; }
        [ForeignKey("RechargePackageID")]
        public RechargePackage RechargePackage { get; set; }
        [Required]
        public DateTime RechargeDate { get; set; }
        [Required]
        public double Amount { get; set; }
        [Required]
        [Column(TypeName = "varchar(20)")]
        public string PaymentMethod { get; set; }
        [Required]
        [Column(TypeName = "varchar(20)")]
        public string Status { get; set; }

    }
}