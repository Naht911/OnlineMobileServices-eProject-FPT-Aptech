using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace OnlineMobileServices_Models.DTOs
{
    public class CallerTunesPackageDTO
    {

        public string PackageName { get; set; }

        public double Amount { get; set; }
        public int Validity { get; set; }
        public string Status { get; set; }
        [Required]
        [Display(Name = "File")]
        public IFormFile? Mp3 { get; set; }



    }
}