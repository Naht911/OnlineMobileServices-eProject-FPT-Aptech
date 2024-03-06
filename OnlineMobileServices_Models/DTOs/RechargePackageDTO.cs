using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineMobileServices_Models.DTOs
{
    public class RechargePackageDTO
    {
        
        public string PackageName { get; set; }
        public string SubscriptionCode { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }        
        public int Validity { get; set; }
        public int DataVolume { get; set; }
        public int VoiceCall { get; set; }
        public int SMS { get; set; }
        public string Image { get; set; }        
        public int TelcoID { get; set; }


    }
}