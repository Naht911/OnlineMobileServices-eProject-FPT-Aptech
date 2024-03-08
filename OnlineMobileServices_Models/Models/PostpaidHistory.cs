using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineMobileServices_Models.Models
{
    public class PostPaidHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int HistoryID { get; set; }
        [Required]
        public int EnterBillID { get; set; }
        public string PhoneNumber { get; set; }
        public double Amount { get; set; }
        public DateTime Date { get; set; }
        public int? UserID { get; set; }
        [ForeignKey("UserID")]
        public User? User { get; set; }
        [Required]
        [Column(TypeName = "varchar(20)")]
        public string? PaymentMethod { get; set; }
        [Required]
        [Column(TypeName = "varchar(20)")]
        public string? Status { get; set; }


    }
}