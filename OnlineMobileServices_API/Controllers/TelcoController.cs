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
    public class TelcoController : ControllerBase
    {
        private readonly DatabaseContext _context;
        public TelcoController(DatabaseContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Telco>>> GetTelcos()
        {
            return await _context.Telcos.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Telco>> GetTelco(int id)
        {
            var telco = await _context.Telcos.FindAsync(id);
            if (telco == null)
            {
                return NotFound();
            }
            return telco;
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTelco(int id, Telco telco)
        {
            if (id != telco.TelcoID)
            {
                return BadRequest();
            }
            _context.Entry(telco).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TelcoExists(id))
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
        private bool TelcoExists(int id)
        {
            return _context.Telcos.Any(e => e.TelcoID == id);
        }


    }
}