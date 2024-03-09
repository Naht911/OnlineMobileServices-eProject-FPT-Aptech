using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OnlineMobileServices.Controllers;
using OnlineMobileServices_API.Models;
using OnlineMobileServices_Models.DTOs;
using OnlineMobileServices_Models.Models;
using OnlineMobileServices_Models.Services;

namespace OnlineMobileServices_API.Controllers.Dashboard
{
    [ApiController]
    [Route("api/[controller]")]
    public class CallerTunesController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly UserService _userService;
        public CallerTunesController(DatabaseContext context, UserService userService)
        {
            _context = context;
            _userService = userService;
        }




        //get id
        [HttpGet("{id}")]
        public async Task<ActionResult<CallerTunesPackage>> GetCallerTunesPackage(int id)
        {
            var callerTunesPackage = await _context.CallerTunesPackages.FindAsync(id);

            if (callerTunesPackage == null)
            {
                return NotFound();
            }

            return callerTunesPackage;
        }

        //get otp (input: phone number, token? | output: otp[fake:1234])
        [HttpPost("otp")]
        public async Task<IActionResult> GetOTP(string MobileNumber, int PackageID)
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
            var rechargePackage = await _context.CallerTunesPackages.FindAsync(PackageID);
            if (rechargePackage == null)
            {
                rsObject = new { message = "Song not found or invalid" };
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
        [HttpPost("buy")]
        public async Task<IActionResult> Recharge(string MobileNumber, string otp, int PackageId, string token = "")
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
                var rechargePackage = await _context.CallerTunesPackages.FindAsync(PackageId);
                if (rechargePackage == null)
                {
                    rsObject = new
                    {
                        status = 0,
                        message = "Song not found or invalid"
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

                CallerTunesHistory pkgHistory = new CallerTunesHistory
                {
                    UserID = user_id == -1 ? null : user_id,
                    MobileNumber = MobileNumber,
                    PackageID = PackageId,
                    Date = DateTime.Now,
                    PaymentMethod = "Payal",
                    Status = "Pending",
                    Amount = rechargePackage.Amount,

                };
                _context.Entry(pkgHistory).State = EntityState.Added;
                _context.CallerTunesHistories.Add(pkgHistory);
                await _context.SaveChangesAsync();
                rsObject = new
                {
                    status = 1,
                    message = "Order created successfully. You will be redirected to the payment page",
                    rechargePackageHistoryID = pkgHistory.HistoryID,
                    amount = rechargePackage.Amount,
                    service = "CallerTunes"
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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CallerTunesPackage>>> GetCallerTunesPackages()
        {
            return await _context.CallerTunesPackages.ToListAsync();
        }










        #region Admin

        [HttpPost("Create")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<CallerTunesPackage>> PostRechargePackage(CallerTunesPackageDTO _pkg, string token)
        {
            if (!CheckRole(token))
            {
                return Unauthorized();
            }

            //xử lý ảnh
            string mp3Path = "";
            if (_pkg.Mp3 != null)
            {
                if (!FileController.CheckImageSize(_pkg.Mp3, 500))
                {
                    return BadRequest("File size is too large");
                }
                //check extension
                if (!FileController.CheckMp3IsValid(_pkg.Mp3))
                {
                    return BadRequest("Mp3 file is not valid");
                }
                mp3Path = FileController.UploadMp3(_pkg.Mp3);
            }
            else
            {
                return BadRequest("Mp3's file is required");
            }
            //create a new recharge package
            CallerTunesPackage newPkg = new CallerTunesPackage
            {
                PackageName = _pkg.PackageName,
                Amount = _pkg.Amount,
                Validity = _pkg.Validity,
                Icon = mp3Path,
                Status = "Active"

            };

            //_context.Entry(rechargePackage).State = EntityState.Added;

            _context.CallerTunesPackages.Add(newPkg);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCallerTunesPackages", new { id = newPkg.PackageID }, newPkg);
        }

        #endregion














        //check role 
        [NonAction]
        public bool CheckRole(string token)
        {
            Console.WriteLine("Token_: " + token);
            if (token == null | token == "" || string.IsNullOrEmpty(token))
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