using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineMobileServices_Models.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }
        [Required]
        [Column(TypeName = "varchar(10)")]
        public string MobileNumber { get; set; }
        [Required]
        [Column(TypeName = "varchar(128)")]
        public string Password { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? FullName { get; set; }

        [Column(TypeName = "nvarchar(150)")]
        public string? Address { get; set; }
        
        public string? Email { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string? Role { get; set; }
        //register date
        [Required]
        public DateTime RegisterDate { get; set; }
        public IEnumerable<RechargePackageHistory>? RechargePackageHistories { get; set; }
        public IEnumerable<SpecialRechargePackageHistory>? SpecialRechargePackageHistories { get; set; }
        public IEnumerable<ServiceHistory>? ServiceHistories { get; set; }

    }
}
