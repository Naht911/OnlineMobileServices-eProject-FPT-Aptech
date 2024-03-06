using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineMobileServices_Models.Models
{
    public class WebsiteSettings
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string? Name { get; set; }
        [Column(TypeName = "text")]
        public string? Value { get; set; }

        //last edit date
        public DateTime? LastEditDate { get; set; }
        //last edit by user
        public int? LastEditedByID { get; set; }
        [ForeignKey("LastEditedByID")]
        //liên kết với bảng user
        public User? LastEditedBy { get; set; }

        
    }
}
