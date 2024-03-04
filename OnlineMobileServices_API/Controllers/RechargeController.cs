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
    [ApiController]
    [Route("api/[controller]")]
    public class RechargeController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly UserService _userService;
        public RechargeController(DatabaseContext context, UserService userService)
        {
            _context = context;
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RechargePackage>>> GetRechargePackages()
        {
            return await _context.RechargePackages.ToListAsync();
        }
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
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRechargePackage(int id, RechargePackage rechargePackage)
        {
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
        [HttpPost]
        public async Task<ActionResult<RechargePackage>> PostRechargePackage(RechargePackage rechargePackage)
        {
            _context.RechargePackages.Add(rechargePackage);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetRechargePackage", new { id = rechargePackage.RechargePackageID }, rechargePackage);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<RechargePackage>> DeleteRecharge(int id)
        {
            var rechargePackage = await _context.RechargePackages.FindAsync(id);
            if (rechargePackage == null)
            {
                return NotFound();
            }
            _context.RechargePackages.Remove(rechargePackage);
            await _context.SaveChangesAsync();
            return rechargePackage;
        }
        private bool RechargePackageExists(int id)
        {
            return _context.RechargePackages.Any(e => e.RechargePackageID == id);
        }


        #region [ForUser]
        //get RechargePackageHistory
        [HttpGet("history")]
        public async Task<ActionResult<IEnumerable<RechargePackageHistory>>> GetRechargePackageHistory(String token)
        {
            var user_id = _userService.GetUserIdFromToken(token);
            if (user_id == -1)
            {
                return Unauthorized();
            }
            return await _context.RechargePackageHistories.Where(x => x.UserID == user_id).ToListAsync();
        }
        //get otp (input: phone number, token? | output: otp[fake:1234])
        [HttpPost("otp")]
        public async Task<ActionResult<String>> GetOTP(String MobileNumber, int RechargePackageId, String token = "")
        {
            //check phone number is valid (10 digits)
            if (MobileNumber.Length != 10)
            {
                return BadRequest("Invalid phone number");
            }
            //check RechargePackageId is valid
            var rechargePackage = await _context.RechargePackages.FindAsync(RechargePackageId);
            if (rechargePackage == null)
            {
                return BadRequest("Invalid RechargePackageId");
            }
            //check token is valid
            if (token != "")
            {
                if (!_userService.IsTokenValid(token))
                {
                    return Unauthorized();
                }
            }
            //return fake otp
            return "1234";
        }
        //recharge (input: phone number, otp, RechargePackageId, token? | output: status, message)
        [HttpPost("recharge")]
        public async Task<IActionResult> Recharge(String MobileNumber, String otp, int RechargePackageId, String token = "")
        {
            //check phone number is valid (10 digits)
            if (MobileNumber.Length != 10)
            {
                return BadRequest("Invalid phone number");
            }
            //check otp is valid
            if (otp != "1234")
            {
                return BadRequest("Invalid OTP");
            }
            //check RechargePackageId is valid
            var rechargePackage = await _context.RechargePackages.FindAsync(RechargePackageId);
            if (rechargePackage == null)
            {
                return BadRequest("Invalid RechargePackageId");
            }
            //check token is valid
            int user_id = -1;
            if (token != "")
            {
                if (!_userService.IsTokenValid(token))
                {
                    return Unauthorized();
                }
                user_id = _userService.GetUserIdFromToken(token);
            }
            //add RechargePackageHistory

            RechargePackageHistory rechargePackageHistory = new RechargePackageHistory
            {
                UserID = user_id,
                MobileNumber = MobileNumber,
                RechargePackageID = RechargePackageId,
                RechargeDate = DateTime.Now
            };

            _context.RechargePackageHistories.Add(rechargePackageHistory);
            await _context.SaveChangesAsync();
            return Ok("Recharge success");
        }
        //delete DeleteRecharge (input: RechargePackageHistoryID, token | output: status, message)
        [HttpDelete("history")]
        public async Task<IActionResult> DeleteRechargeHistory(int RechargePackageHistoryID, String token)
        {
            //check token is valid
            if (!_userService.IsTokenValid(token))
            {
                return Unauthorized();
            }
            //check RechargePackageHistoryID is valid
            var rechargePackageHistory = await _context.RechargePackageHistories.FindAsync(RechargePackageHistoryID);
            if (rechargePackageHistory == null)
            {
                return BadRequest("Invalid RechargePackageHistoryID");
            }
            //delete DeleteRecharge
            _context.RechargePackageHistories.Remove(rechargePackageHistory);
            await _context.SaveChangesAsync();
            return Ok("Delete success");
        }
        //delete DeleteRecharge belong on phone number (input: phone number, otp | output: status, message)
        [HttpDelete("history/phone")]
        public async Task<IActionResult> DeleteRechargeHistoryPhone(String MobileNumber, String otp)
        {
            //check phone number is valid (10 digits)
            if (MobileNumber.Length != 10)
            {
                return BadRequest("Invalid phone number");
            }
            //check otp is valid
            if (otp != "1234")
            {
                return BadRequest("Invalid OTP");
            }
            //delete DeleteRecharge
            var rechargePackageHistories = await _context.RechargePackageHistories.Where(x => x.MobileNumber == MobileNumber).ToListAsync();
            foreach (var rechargePackageHistory in rechargePackageHistories)
            {
                _context.RechargePackageHistories.Remove(rechargePackageHistory);
            }
            await _context.SaveChangesAsync();
            return Ok("Delete success");
        }




        #endregion
    }
}