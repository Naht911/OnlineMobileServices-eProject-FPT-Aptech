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
            //get data remove object cycle 
            var data = await _context.RechargePackages.Include(rp => rp.Telco).ToListAsync();
            foreach (var item in data)
            {
                item.RechargePackageHistories = null;
                item.Telco.RechargePackages = null;

            }
            return data;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RechargePackage>> GetRechargePackage(int id)
        {
            var rechargePackage = await _context.RechargePackages.Include(rp => rp.Telco).FirstOrDefaultAsync(rp => rp.RechargePackageID == id);

            if (rechargePackage == null)
            {
                return NotFound();
            }
            rechargePackage.RechargePackageHistories = null;
            rechargePackage.Telco.RechargePackages = null;
            return rechargePackage;
        }

        ///RechargePackage/Special
        [HttpGet("Special")]
        public async Task<ActionResult<IEnumerable<SpecialRechargePackage>>> GetSpecialRechargePackages()
        {
            var data = await _context.SpecialRechargePackages.Include(rp => rp.Telco).ToListAsync();
            foreach (var item in data)
            {
                item.SpecialRechargePackageHistories = null;
                item.Telco.RechargePackages = null;

            }
            return data;
        }

        //get RechargePackage/Special id
        [HttpGet("Special/{id}")]
        public async Task<ActionResult<SpecialRechargePackage>> GetSpecialRechargePackage(int id)
        {
            var specialRechargePackage = await _context.SpecialRechargePackages.Include(rp => rp.Telco).FirstOrDefaultAsync(rp => rp.SpecialRechargePackageID == id);

            if (specialRechargePackage == null)
            {
                return NotFound();
            }
            specialRechargePackage.SpecialRechargePackageHistories = null;
            specialRechargePackage.Telco.RechargePackages = null;
            return specialRechargePackage;
        }




    }
}