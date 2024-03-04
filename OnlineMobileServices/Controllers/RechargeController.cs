using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace OnlineMobileServices.Controllers
{
    [Route("[controller]")]
    public class RechargeController : Controller
    {
        private readonly ILogger<RechargeController> _logger;

        public RechargeController(ILogger<RechargeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }


    }
}