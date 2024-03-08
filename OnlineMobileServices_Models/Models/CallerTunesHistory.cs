using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineMobileServices_Models.Models
{
    public class CallerTunesHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int HistoryID { get; set; }
        [Required]
        [Column(TypeName = "varchar(10)")]
        public string MobileNumber { get; set; }
        
        public int? UserID { get; set; }
        [ForeignKey("UserID")]
        public User? User { get; set; }
        [Required]
        public int PackageID { get; set; }
        [ForeignKey("PackageID")]
        public CallerTunesPackage? OriginalPackage { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public double Amount { get; set; }
        [Required]
        [Column(TypeName = "varchar(20)")]
        public string? PaymentMethod { get; set; }
        [Required]
        [Column(TypeName = "varchar(20)")]
        public string? Status { get; set; }
        public CallerTunesPackage? CallerTunesPackage { get; set; }
    }
}