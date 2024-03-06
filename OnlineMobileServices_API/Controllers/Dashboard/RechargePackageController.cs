using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineMobileServices_API.Models;
using OnlineMobileServices_Models.Models;
using OnlineMobileServices_Models.Services;

namespace OnlineMobileServices_API.Controllers.Dashboard
{
    [ApiController]
    [Route("api/dashboard/[controller]")]
    public class RechargePackageController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly UserService _userService;
        public RechargePackageController(DatabaseContext context, UserService userService)
        {
            _context = context;
            _userService = userService;
        }

        //create a new recharge package
        [HttpPost]
        public async Task<ActionResult<RechargePackage>> PostRechargePackage(RechargePackage rechargePackage, String token)
        {
            if (!CheckRole(token))
            {
                return Unauthorized();
            }


            _context.RechargePackages.Add(rechargePackage);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRechargePackage", new { id = rechargePackage.RechargePackageID }, rechargePackage);
        }

        //update a recharge package
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRechargePackage(int id, RechargePackage rechargePackage, String token)
        {
            if (!CheckRole(token))
            {
                return Unauthorized();
            }


            if (id != rechargePackage.RechargePackageID)
            {
                return BadRequest();
            }


            _context.Entry(rechargePackage).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RechargePackageExists(id))
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

        //delete a recharge package
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRechargePackage(int id, String token)
        {
            if (!CheckRole(token))
            {
                return Unauthorized();
            }


            var rechargePackage = await _context.RechargePackages.FindAsync(id);
            if (rechargePackage == null)
            {
                return NotFound();
            }
            _context.RechargePackages.Remove(rechargePackage);
            await _context.SaveChangesAsync();
            return NoContent();
        }


        // RechargePackageExists
        [HttpGet("RechargePackageExists")]
        private bool RechargePackageExists(int id)
        {
            return _context.RechargePackages.Any(e => e.RechargePackageID == id);
        }

        //GetRechargePackage
        [HttpGet("{id}")]
        public async Task<ActionResult<RechargePackage>> GetRechargePackage(int id)
        {
            var rechargePackage = await _context.RechargePackages.FindAsync(id);
            if (rechargePackage == null)
            {
                return NotFound();
            }
            return rechargePackage;
        }
        //create a new special recharge package
        [HttpPost("special")]
        public async Task<ActionResult<RechargePackage>> PostSpecialRechargePackage(RechargePackage rechargePackage, String token)
        {
            if (!CheckRole(token))
            {
                return Unauthorized();
            }

            _context.RechargePackages.Add(rechargePackage);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRechargePackage", new { id = rechargePackage.RechargePackageID }, rechargePackage);
        }


        //update a special recharge package

        [HttpPut("special/{id}")]
        public async Task<IActionResult> PutSpecialRechargePackage(int id, RechargePackage rechargePackage, String token)
        {
            if (!CheckRole(token))
            {
                return Unauthorized();
            }

            if (id != rechargePackage.RechargePackageID)
            {
                return BadRequest();
            }
            _context.Entry(rechargePackage).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RechargePackageExists(id))
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


        //delete a special recharge package
        [HttpDelete("special/{id}")]
        public async Task<IActionResult> DeleteSpecialRechargePackage(int id, String token)
        {
            if (!CheckRole(token))
            {
                return Unauthorized();
            }

            var rechargePackage = await _context.RechargePackages.FindAsync(id);
            if (rechargePackage == null)
            {
                return NotFound();
            }
            _context.RechargePackages.Remove(rechargePackage);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        //GetSpecialRechargePackage
        [HttpGet("special/{id}")]
        public async Task<ActionResult<RechargePackage>> GetSpecialRechargePackage(int id)
        {
            var rechargePackage = await _context.RechargePackages.FindAsync(id);
            if (rechargePackage == null)
            {
                return NotFound();
            }
            return rechargePackage;
        }

        //check role 
        [NonAction]
        private bool CheckRole(String token)
        {
            Console.WriteLine("Token_: " + token);
            if (token == null | token == "" || String.IsNullOrEmpty(token))
            {
                return false;
            }

            if (!_userService.ValidateToken(token))
            {
                Console.WriteLine("Invalid token");
                return false;
            }
            //Get role from token
            var uid = _userService.GetUserIdFromToken(token);
            var role = _context.Users.Find(uid)?.Role ?? "User";
            if (role != "Admin")
            {
                Console.WriteLine("Role: " + role);
                return false;
            }
            return true;
        }




    }
}