using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineMobileServices_API.Models;
using OnlineMobileServices_Models.Models;
using OnlineMobileServices_Models.Services;

namespace OnlineMobileServices_API.Controllers
{
    [Area("Home")]
    [ApiController]
    [Route("api/[controller]")]
    public class RechargePackageController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly UserService _userService;
        public RechargePackageController(DatabaseContext context, UserService userService)
        {
            _context = context;
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RechargePackage>>> GetRechargePackages()
        {
            return await _context.RechargePackages.ToListAsync();
        }

        // RechargePackageExists


    }
}