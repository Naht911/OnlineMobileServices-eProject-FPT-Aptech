using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace OnlineMobileServices_Models.DTOs
{
    public class TelcoDTO
    {
        public string TelcoName { get; set; }
        public string Description { get; set; }
        
        [Display(Name = "File")]
        public IFormFile? Logo { get; set; }
    }
}