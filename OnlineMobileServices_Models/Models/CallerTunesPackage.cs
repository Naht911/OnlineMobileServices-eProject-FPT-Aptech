using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineMobileServices_Models.Models
{
    public class CallerTunesPackage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PackageID { get; set; }
        [Required]
        [Column(TypeName = "varchar(50)")]
        public string PackageName { get; set; }
        [Required]
        public double Amount { get; set; }
        [Required]
        public int Validity { get; set; }
        [Required]
        [Column(TypeName = "varchar(20)")]
        public string Status { get; set; }
        //logo img url
        [Required]
        [Column(TypeName = "varchar(256)")]
        public string Icon { get; set; }
        public IEnumerable<CallerTunesHistory>? CallerTunesHistories { get; set; }
    }
}