using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineMobileServices_Models.Models
{
    public class SpecialRechargePackage
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SpecialRechargePackageID { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string SpecialPackageName { get; set; }
        //subscription code
        [Required]
        [Column(TypeName = "varchar(20)")]
        public string SubscriptionCode { get; set; }
        //description
        [Required]
        [Column(TypeName = "nvarchar(512)")]
        public string Description { get; set; }
        //price
        [Required]
        public double Price { get; set; }
        //validity (in days)
        [Required]
        public double Validity { get; set; }
        //data volume (in GBs)
        [Required]
        public double DataVolume { get; set; }
        //voice call (in minutes)
        [Required]
        public double VoiceCall { get; set; }
        //sms (in count)
        [Required]
        public int SMS { get; set; }
        //image url
        [Required]
        [Column(TypeName = "varchar(256)")]
        public string Image { get; set; }
        //telco id
        [Required]
        public int TelcoID { get; set; }
        //telco
        [ForeignKey("TelcoID")]
        public Telco Telco { get; set; }
        //image file non-mapped (not in database)
        [NotMapped]
        public string ImageFile { get; set; }
        public IEnumerable<SpecialRechargePackageHistory>? SpecialRechargePackageHistories { get; set; }
    }
}