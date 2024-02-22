using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineMobileServices_Models.Models
{
    public class Services
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ServiceID { get; set; }
        [Required]
        [Column(TypeName = "varchar(50)")]
        public string ServiceName { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? ServiceDescription { get; set; }
    }
}
