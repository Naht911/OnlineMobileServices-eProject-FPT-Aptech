using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineMobileServices_Models.Models
{
    public class Telco
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TelcoID { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string TelcoName { get; set; }
        //description
        [Required]
        [Column(TypeName = "nvarchar(512)")]
        public string Description { get; set; }
        //logo img url
        [Required]
        [Column(TypeName = "varchar(256)")]
        public string Logo { get; set; }
        
        public IEnumerable<RechargePackage>? RechargePackages { get; set; }
        public IEnumerable<SpecialRechargePackage>? SpecialRechargePackages { get; set; }
    }
}
