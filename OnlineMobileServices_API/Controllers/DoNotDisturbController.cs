using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OnlineMobileServices_API.Models;
using OnlineMobileServices_Models.Models;
using OnlineMobileServices_Models.Services;

namespace OnlineMobileServices_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoNotDisturbController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly UserService _userService;
        public DoNotDisturbController(DatabaseContext context, UserService userService)
        {
            _context = context;
            _userService = userService;
        }

        [HttpPost("otp")]
        public async Task<IActionResult> GetOTP(string MobileNumber)
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

            //get otp
            rsObject = new
            {
                status = 1,
                message = "OTP sent to your phone number",
            };
            rsJson = JsonConvert.SerializeObject(rsObject);
            return Ok(rsJson);

        }



        [HttpPost]
        public async Task<IActionResult> Index(string MobileNumber, string otp, string token = "")
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

                DoNotDisturbHistory doNotDisturbHistory = new DoNotDisturbHistory
                {
                    MobileNumber = MobileNumber,
                    Amount = 5,
                    UserID = user_id == -1 ? null : user_id,
                    PaymentMethod = "Paypal",
                    Status = "Pending",
                    Date = DateTime.Now
                };
                _context.Entry(doNotDisturbHistory).State = EntityState.Added;
                _context.DoNotDisturbHistories.Add(doNotDisturbHistory);
                await _context.SaveChangesAsync();
                rsObject = new
                {
                    status = 1,
                    message = "Order created successfully. You will be redirected to the payment page",
                    historyID = doNotDisturbHistory.HistoryID,
                    amount = doNotDisturbHistory.Amount,
                    service = "DoNotDisturb"
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

    }
}