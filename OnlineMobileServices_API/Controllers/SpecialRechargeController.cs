using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineMobileServices_API.Models;
using OnlineMobileServices_Models.Models;

namespace OnlineMobileServices_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpecialRechargeController : ControllerBase
    {
        private readonly DatabaseContext _context;
        public SpecialRechargeController(DatabaseContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SpecialRechargePackage>>> GetSpecialRechargePackages()
        {
            return await _context.SpecialRechargePackages.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<SpecialRechargePackage>> GetSpecialRechargePackage(int id)
        {
            var specialRechargePackage = await _context.SpecialRechargePackages.FindAsync(id);
            if (specialRechargePackage == null)
            {
                return NotFound();
            }
            return specialRechargePackage;
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSpecialRechargePackage(int id, SpecialRechargePackage specialRechargePackage)
        {
            if (id != specialRechargePackage.SpecialRechargePackageID)
            {
                return BadRequest();
            }
            _context.Entry(specialRechargePackage).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SpecialRechargePackageExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }
        [HttpPost]
        public async Task<ActionResult<SpecialRechargePackage>> PostSpecialRechargePackage(SpecialRechargePackage specialRechargePackage)
        {
            _context.SpecialRechargePackages.Add(specialRechargePackage);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetSpecialRechargePackage", new { id = specialRechargePackage.SpecialRechargePackageID }, specialRechargePackage);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<SpecialRechargePackage>> DeleteSpecialRechargePackage(int id)
        {
            var specialRechargePackage = await _context.SpecialRechargePackages.FindAsync(id);
            if (specialRechargePackage == null)
            {
                return NotFound();
            }
            _context.SpecialRechargePackages.Remove(specialRechargePackage);
            await _context.SaveChangesAsync();
            return specialRechargePackage;
        }
        private bool SpecialRechargePackageExists(int id)
        {
            return _context.SpecialRechargePackages.Any(e => e.SpecialRechargePackageID == id);
        }

    }
}