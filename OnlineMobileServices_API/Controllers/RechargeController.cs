using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OnlineMobileServices_API.Models;
using OnlineMobileServices_Models.DTOs;
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



        #region [ForUser]
        //get RechargePackageHistory
        [HttpGet("history")]
        public async Task<ActionResult<IEnumerable<RechargeHistory>>> GetRechargePackageHistory(string token)
        {
            var user_id = _userService.GetUserIdFromToken(token);
            if (user_id == -1)
            {
                return Unauthorized();
            }
            return await _context.RechargeHistories.Where(x => x.UserID == user_id).ToListAsync();
        }
        //get otp (input: phone number, token? | output: otp[fake:1234])
        [HttpPost("otp")]
        public async Task<IActionResult> GetOTP(string MobileNumber, int RechargePackageId)
        {
            Object rsObject;
            var rsJson = "";
            //check phone number is valid (10 digits)
            if (MobileNumber == null || MobileNumber.Length != 10)
            {
                rsObject = new { message = "Phone number must be 10 digits" };
                rsJson = JsonConvert.SerializeObject(rsObject);
                return StatusCode(400, rsJson);
            }
            //check RechargePackageId is valid
            var rechargePackage = await _context.RechargePackages.FindAsync(RechargePackageId);
            if (rechargePackage == null)
            {
                rsObject = new { message = "Recharge Package not found or invalid" };
                rsJson = JsonConvert.SerializeObject(rsObject);
                return StatusCode(400, rsJson);
            }
            //get otp
            rsObject = new
            {
                status = 1,
                message = "OTP sent to your phone number",
            };
            rsJson = JsonConvert.SerializeObject(rsObject);
            return Ok(rsJson);


        }
        //recharge (input: phone number, otp, RechargePackageId, token? | output: status, message)
        [HttpPost("recharge")]
        public async Task<IActionResult> Recharge(string MobileNumber, string otp, int RechargePackageId, string token = "")
        {
            Object rsObject;
            var rsJson = "";
            try
            {
                //check phone number is valid (10 digits)
                if (MobileNumber.Length != 10)
                {
                    rsObject = new
                    {
                        status = 0,
                        message = "Phone number must be 10 digits"
                    };
                    rsJson = JsonConvert.SerializeObject(rsObject);
                    return StatusCode(400, rsJson);
                }
                //check otp is valid
                if (otp != "1234")
                {
                    rsObject = new
                    {
                        status = 0,
                        message = "OTP is not correct"
                    };
                    rsJson = JsonConvert.SerializeObject(rsObject);
                    return StatusCode(400, rsJson);
                }
                //check RechargePackageId is valid
                var rechargePackage = await _context.RechargePackages.FindAsync(RechargePackageId);
                if (rechargePackage == null)
                {
                    rsObject = new
                    {
                        status = 0,
                        message = "Recharge Package not found or invalid"
                    };
                    rsJson = JsonConvert.SerializeObject(rsObject);
                    return StatusCode(400, rsJson);
                }
                //check token is valid
                int user_id = -1;
                if (token != "")
                {
                    if (!_userService.ValidateToken(token))
                    {
                        return Unauthorized();
                    }
                    user_id = _userService.GetUserIdFromToken(token);
                }
                //add RechargePackageHistory

                RechargeHistory rechargePackageHistory = new RechargeHistory
                {
                    UserID = user_id == -1 ? null : user_id,
                    MobileNumber = MobileNumber,
                    PackageID = RechargePackageId,
                    Date = DateTime.Now,
                    PaymentMethod = "Payal",
                    Status = "Pending",
                    Amount = rechargePackage.Price
                };
                _context.Entry(rechargePackageHistory).State = EntityState.Added;
                _context.RechargeHistories.Add(rechargePackageHistory);
                await _context.SaveChangesAsync();
                rsObject = new
                {
                    status = 1,
                    message = "Order created successfully. You will be redirected to the payment page",
                    rechargePackageHistoryID = rechargePackageHistory.HistoryID,
                    amount = rechargePackage.Price,
                    service = "Recharge"
                };
                rsJson = JsonConvert.SerializeObject(rsObject);
                return Ok(rsJson);
            }
            catch (System.Exception e)
            {
                rsObject = new
                {
                    status = 0,
                    message = "Something went wrong. Refresh the page and try again",
                    error = e
                };
                rsJson = JsonConvert.SerializeObject(rsObject);
                return StatusCode(500, rsJson);
            }
        }
        //delete DeleteRecharge (input: RechargePackageHistoryID, token | output: status, message)
        [HttpDelete("history")]
        public async Task<IActionResult> DeleteRechargeHistory(int RechargePackageHistoryID, string token)
        {
            //check token is valid
            if (!_userService.ValidateToken(token))
            {
                return Unauthorized();
            }
            //check RechargePackageHistoryID is valid
            var rechargePackageHistory = await _context.RechargeHistories.FindAsync(RechargePackageHistoryID);
            if (rechargePackageHistory == null)
            {
                return BadRequest("Invalid RechargePackageHistoryID");
            }
            //delete DeleteRecharge
            _context.RechargeHistories.Remove(rechargePackageHistory);
            await _context.SaveChangesAsync();
            return Ok("Delete success");
        }
        //delete DeleteRecharge belong on phone number (input: phone number, otp | output: status, message)
        [HttpDelete("history/phone")]
        public async Task<IActionResult> DeleteRechargeHistoryPhone(string MobileNumber, string otp)
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
            var rechargePackageHistories = await _context.RechargeHistories.Where(x => x.MobileNumber == MobileNumber).ToListAsync();
            foreach (var rechargePackageHistory in rechargePackageHistories)
            {
                _context.RechargeHistories.Remove(rechargePackageHistory);
            }
            await _context.SaveChangesAsync();
            return Ok("Delete success");
        }




        #endregion
    }
}