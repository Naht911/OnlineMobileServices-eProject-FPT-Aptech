using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
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
    [Area("Dashboard")]
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly UserService _userService;
        public DashboardController(DatabaseContext context, UserService userService)
        {
            _context = context;
            _userService = userService;
        }

        #region RECHARGE PACKAGE
        //get all recharge packages
        [HttpGet("RechargePackage")]
        public async Task<ActionResult<IEnumerable<RechargePackage>>> GetRechargePackages(string token)
        {
            if (!CheckRole(token))
            {
                return Unauthorized();
            }
            //get data remove object cycle 
            var data = await _context.RechargePackages.Include(rp => rp.Telco).ToListAsync();
            foreach (var item in data)
            {
                item.RechargePackageHistories = null;
                item.Telco.RechargePackages = null;

            }
            return data;
        }

        //create a new recharge package
        [HttpPost("RechargePackage/Create")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<RechargePackage>> PostRechargePackage(RechargePackageDTO _rechargePackage, string token)
        {
            object result_0;
            string RsJson_0 = String.Empty;
            if (!CheckRole(token))
            {
                result_0 = new
                {
                    status = 0,
                    message = "Permission denie., Please login with admin account or try to login again",
                };
                RsJson_0 = JsonConvert.SerializeObject(result_0);
                return StatusCode(401, RsJson_0);
            }
            Telco? telco = _context.Telcos.Find(_rechargePackage.TelcoID);
            if (telco == null)
            {
                result_0 = new
                {
                    status = 0,
                    message = "TelcoID is not exists",
                };
                RsJson_0 = JsonConvert.SerializeObject(result_0);
                return StatusCode(400, RsJson_0);
            }
            //check Price not negative
            if (_rechargePackage.Price < 0)
            {
                result_0 = new
                {
                    status = 0,
                    message = "Price must be greater than 0",
                };
                RsJson_0 = JsonConvert.SerializeObject(result_0);
                return StatusCode(400, RsJson_0);
            }
            // Validity
            if (_rechargePackage.Validity < 0)
            {
                result_0 = new
                {
                    status = 0,
                    message = "Validity must be greater than 0",
                };
                RsJson_0 = JsonConvert.SerializeObject(result_0);
                return StatusCode(400, RsJson_0);
            }
            // DataVolume
            if (_rechargePackage.DataVolume < 0)
            {
                result_0 = new
                {
                    status = 0,
                    message = "DataVolume must be greater than 0",
                };
                RsJson_0 = JsonConvert.SerializeObject(result_0);
                return StatusCode(400, RsJson_0);
            }
            // VoiceCall
            if (_rechargePackage.VoiceCall < 0)
            {
                result_0 = new
                {
                    status = 0,
                    message = "VoiceCall must be greater than 0",
                };
                RsJson_0 = JsonConvert.SerializeObject(result_0);
                return StatusCode(400, RsJson_0);
            }
            //xử lý ảnh
            string image_path = "";
            if (_rechargePackage.Image != null)
            {
                if (!FileController.CheckImageSize(_rechargePackage.Image, 100))
                {
                    result_0 = new
                    {
                        status = 0,
                        message = "File size is too large",
                    };
                    RsJson_0 = JsonConvert.SerializeObject(result_0);
                    return StatusCode(400, RsJson_0);
                }
                //check extension
                if (!FileController.CheckImageIsValid(_rechargePackage.Image))
                {
                    result_0 = new
                    {
                        status = 0,
                        message = "Image is not valid",
                    };
                    RsJson_0 = JsonConvert.SerializeObject(result_0);
                    return StatusCode(400, RsJson_0);
                }
                image_path = FileController.UploadImage(_rechargePackage.Image);
            }
            else
            {
                result_0 = new
                {
                    status = 0,
                    message = "Image is required",
                };
                RsJson_0 = JsonConvert.SerializeObject(result_0);
            }
            //check trùng SubscriptionCode và TelcoID
            if (_context.RechargePackages.Any(x => x.SubscriptionCode == _rechargePackage.SubscriptionCode && x.TelcoID == _rechargePackage.TelcoID))
            {
                result_0 = new
                {
                    status = 0,
                    message = "SubscriptionCode is already exists",
                };
                RsJson_0 = JsonConvert.SerializeObject(result_0);
                return StatusCode(400, RsJson_0);
            }
            //create a new recharge package
            RechargePackage rechargePackage = new RechargePackage
            {
                PackageName = _rechargePackage.PackageName,
                SubscriptionCode = _rechargePackage.SubscriptionCode,
                Description = _rechargePackage.Description,
                Price = _rechargePackage.Price,
                Validity = _rechargePackage.Validity,
                DataVolume = _rechargePackage.DataVolume,
                VoiceCall = _rechargePackage.VoiceCall,
                SMS = _rechargePackage.SMS,
                Image = image_path,
                TelcoID = _rechargePackage.TelcoID,
                Telco = telco
            };

            //_context.Entry(rechargePackage).State = EntityState.Added;

            _context.RechargePackages.Add(rechargePackage);
            await _context.SaveChangesAsync();
            result_0 = new
            {
                status = 1,
                message = "Create Recharge Package successfully",
            };
            RsJson_0 = JsonConvert.SerializeObject(result_0);
            return Ok(RsJson_0);
        }

        //update a recharge package
        [HttpPut("RechargePackage/{id}")]
        public async Task<IActionResult> PutRechargePackage(int id, RechargePackageDTO _rechargePackage, string token)
        {
            object result_0;
            string RsJson_0 = String.Empty;
            if (!CheckRole(token))
            {
                result_0 = new
                {
                    status = 0,
                    message = "Permission denie., Please login with admin account or try to login again",
                };
                RsJson_0 = JsonConvert.SerializeObject(result_0);
                return StatusCode(401, RsJson_0);
            }
            var rechargePackage = _context.RechargePackages.Find(id) ?? null;
            if (rechargePackage == null)
            {
                result_0 = new
                {
                    status = 0,
                    message = "RechargePackageID is not exists",
                };
                RsJson_0 = JsonConvert.SerializeObject(result_0);
                return StatusCode(400, RsJson_0);
            }
            Telco? telco = _context.Telcos.Find(_rechargePackage.TelcoID);
            if (telco == null)
            {
                result_0 = new
                {
                    status = 0,
                    message = "TelcoID is not exists",
                };
                RsJson_0 = JsonConvert.SerializeObject(result_0);
                return StatusCode(400, RsJson_0);
            }
            //check Price not negative
            if (_rechargePackage.Price < 0)
            {
                result_0 = new
                {
                    status = 0,
                    message = "Price must be greater than 0",
                };
                RsJson_0 = JsonConvert.SerializeObject(result_0);
                return StatusCode(400, RsJson_0);
            }
            // Validity
            if (_rechargePackage.Validity < 0)
            {
                result_0 = new
                {
                    status = 0,
                    message = "Validity must be greater than 0",
                };
                RsJson_0 = JsonConvert.SerializeObject(result_0);
                return StatusCode(400, RsJson_0);
            }
            // DataVolume
            if (_rechargePackage.DataVolume < 0)
            {
                result_0 = new
                {
                    status = 0,
                    message = "DataVolume must be greater than 0",
                };
                RsJson_0 = JsonConvert.SerializeObject(result_0);
                return StatusCode(400, RsJson_0);
            }
            // VoiceCall
            if (_rechargePackage.VoiceCall < 0)
            {
                result_0 = new { status = 0, message = "VoiceCall must be greater than 0" };
                RsJson_0 = JsonConvert.SerializeObject(result_0);
                return StatusCode(400, RsJson_0);
            }
            //xử lý ảnh
            string image_path = "";
            string old_image = rechargePackage.Image;
            if (_rechargePackage.Image != null)
            {
                if (!FileController.CheckImageSize(_rechargePackage.Image, 100))
                {
                    result_0 = new { status = 0, message = "File size is too large" };
                    RsJson_0 = JsonConvert.SerializeObject(result_0);
                    return StatusCode(400, RsJson_0);
                }
                //check extension
                if (!FileController.CheckImageIsValid(_rechargePackage.Image))
                {
                    result_0 = new { status = 0, message = "Image is not valid" };
                    RsJson_0 = JsonConvert.SerializeObject(result_0);
                    return StatusCode(400, RsJson_0);
                }
                image_path = FileController.UploadImage(_rechargePackage.Image);
            }
            else
            {
                image_path = old_image;
            }
            //check trùng SubscriptionCode và TelcoID nhưng không trùng với chính nó
            if (_context.RechargePackages.Any(x => x.SubscriptionCode == _rechargePackage.SubscriptionCode && x.TelcoID == _rechargePackage.TelcoID && x.RechargePackageID != id))
            {
                result_0 = new { status = 0, message = "SubscriptionCode is already exists" };
                RsJson_0 = JsonConvert.SerializeObject(result_0);
                return StatusCode(400, RsJson_0);
            }
            rechargePackage.PackageName = _rechargePackage.PackageName;
            rechargePackage.SubscriptionCode = _rechargePackage.SubscriptionCode;
            rechargePackage.Description = _rechargePackage.Description;
            rechargePackage.Price = _rechargePackage.Price;
            rechargePackage.Validity = _rechargePackage.Validity;
            rechargePackage.DataVolume = _rechargePackage.DataVolume;
            rechargePackage.VoiceCall = _rechargePackage.VoiceCall;
            rechargePackage.SMS = _rechargePackage.SMS;
            rechargePackage.Image = image_path;
            rechargePackage.TelcoID = _rechargePackage.TelcoID;
            rechargePackage.Telco = telco;

            _context.Entry(rechargePackage).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RechargePackageExists(id))
                {
                    result_0 = new { status = 0, message = "RechargePackageID is not exists" };
                    RsJson_0 = JsonConvert.SerializeObject(result_0);
                    return StatusCode(400, RsJson_0);
                }
                else
                {
                    throw;
                }
            }
            //kiểm tra nếu ảnh mới khác ảnh cũ thì xóa ảnh cũ
            if (image_path != old_image)
            {
                FileController.DeleteFile(old_image);
            }
            result_0 = new { status = 1, message = "Update Recharge Package successfully" };
            RsJson_0 = JsonConvert.SerializeObject(result_0);
            return Ok(RsJson_0);

        }

        //delete a recharge package
        [HttpDelete("RechargePackage/{id}")]
        public async Task<IActionResult> DeleteRechargePackage(int id, string token)
        {
            object result_0;
            string RsJson_0 = String.Empty;
            if (!CheckRole(token))
            {
                result_0 = new { status = 0, message = "Permission denie., Please login with admin account or try to login again" };
                RsJson_0 = JsonConvert.SerializeObject(result_0);
                return StatusCode(401, RsJson_0);
            }


            var rechargePackage = await _context.RechargePackages.FindAsync(id);
            if (rechargePackage == null)
            {
                result_0 = new { status = 0, message = "RechargePackageID is not exists" };
                RsJson_0 = JsonConvert.SerializeObject(result_0);
                return StatusCode(400, RsJson_0);
            }
            _context.RechargePackages.Remove(rechargePackage);
            await _context.SaveChangesAsync();
            result_0 = new { status = 1, message = "Delete Recharge Package successfully" };
            RsJson_0 = JsonConvert.SerializeObject(result_0);
            return Ok(RsJson_0);
        }


        // RechargePackageExists
        [HttpGet("RechargePackage/RechargePackageExists")]
        private bool RechargePackageExists(int id)
        {
            return _context.RechargePackages.Any(e => e.RechargePackageID == id);
        }

        //GetRechargePackage
        [HttpGet("RechargePackage/{id}")]
        public async Task<ActionResult<RechargePackage>> GetRechargePackage(int id)
        {
            var rechargePackage = await _context.RechargePackages.FindAsync(id);
            if (rechargePackage == null)
            {
                return NotFound();
            }
            rechargePackage.RechargePackageHistories = null;
            rechargePackage.Telco = null;
            return rechargePackage;
        }

        #endregion



        #region SPECIAL RECHARGE PACKAGE
        //create a new special recharge package
        [HttpPost("RechargePackage/special")]
        public async Task<ActionResult<RechargePackage>> PostSpecialRechargePackage(RechargePackage rechargePackage, string token)
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

        [HttpPut("RechargePackage/special/{id}")]
        public async Task<IActionResult> PutSpecialRechargePackage(int id, RechargePackage rechargePackage, string token)
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
        [HttpDelete("RechargePackage/special/{id}")]
        public async Task<IActionResult> DeleteSpecialRechargePackage(int id, string token)
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
        [HttpGet("RechargePackage/special/{id}")]
        public async Task<ActionResult<RechargePackage>> GetSpecialRechargePackage(int id)
        {
            var rechargePackage = await _context.RechargePackages.FindAsync(id);
            if (rechargePackage == null)
            {
                return NotFound();
            }
            return rechargePackage;
        }

        #endregion

        #region TELCO
        //get all telcos
        [HttpGet("Telco")]
        public async Task<ActionResult<IEnumerable<Telco>>> GetTelcos(string token)
        {
            if (!CheckRole(token))
            {
                return Unauthorized();
            }
            return await _context.Telcos.ToListAsync();
        }

        //create a new telco
        [HttpPost("Telco/Create")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<Telco>> PostTelco(TelcoDTO _telco, string token)
        {
            object result_0;
            string RsJson_0 = String.Empty;
            if (!CheckRole(token))
            {
                result_0 = new { status = 0, message = "Permission denie., Please login with admin account or try to login again" };
                RsJson_0 = JsonConvert.SerializeObject(result_0);
                return StatusCode(401, RsJson_0);
            }
            //xử lý ảnh
            string image_path = "";
            if (_telco.Logo != null)
            {
                if (!FileController.CheckImageSize(_telco.Logo, 100))
                {
                    result_0 = new { status = 0, message = "File size is too large" };
                    RsJson_0 = JsonConvert.SerializeObject(result_0);
                    return StatusCode(400, RsJson_0);
                }
                //check extension
                if (!FileController.CheckImageIsValid(_telco.Logo))
                {
                    result_0 = new { status = 0, message = "Image is not valid" };
                    RsJson_0 = JsonConvert.SerializeObject(result_0);
                    return StatusCode(400, RsJson_0);
                }
                image_path = FileController.UploadImage(_telco.Logo);
            }
            else
            {
                result_0 = new { status = 0, message = "Logo is required" };
                RsJson_0 = JsonConvert.SerializeObject(result_0);
                return StatusCode(400, RsJson_0);
            }
            //create a new telco
            Telco telco = new Telco
            {
                TelcoName = _telco.TelcoName,
                Description = _telco.Description,
                Logo = image_path
            };

            _context.Telcos.Add(telco);
            await _context.SaveChangesAsync();

            result_0 = new { status = 1, message = "Create Telco successfully" };
            RsJson_0 = JsonConvert.SerializeObject(result_0);
            return Ok(RsJson_0);
        }

        //update a telco
        [HttpPut("Telco/{id}")]
        public async Task<IActionResult> PutTelco(int id, TelcoDTO _telco, string token)
        {
            object result_0;
            string RsJson_0 = String.Empty;
            if (!CheckRole(token))
            {
                result_0 = new { status = 0, message = "Permission denie., Please login with admin account or try to login again" };
                RsJson_0 = JsonConvert.SerializeObject(result_0);
                return StatusCode(401, RsJson_0);
            }

            var telco = _context.Telcos.Find(id) ?? null;
            if (telco == null)
            {
                result_0 = new { status = 0, message = "TelcoID is not exists" };
                RsJson_0 = JsonConvert.SerializeObject(result_0);
                return StatusCode(400, RsJson_0);
            }
            //xử lý ảnh
            string image_path = "";
            string old_image = telco.Logo;
            if (_telco.Logo != null)
            {
                if (!FileController.CheckImageSize(_telco.Logo, 100))
                {
                    result_0 = new { status = 0, message = "File size is too large" };
                    RsJson_0 = JsonConvert.SerializeObject(result_0);
                    return StatusCode(400, RsJson_0);
                }
                //check extension
                if (!FileController.CheckImageIsValid(_telco.Logo))
                {
                    result_0 = new { status = 0, message = "Image is not valid" };
                    RsJson_0 = JsonConvert.SerializeObject(result_0);
                    return StatusCode(400, RsJson_0);
                }
                image_path = FileController.UploadImage(_telco.Logo);
            }
            else
            {
                image_path = old_image;
            }
            telco.TelcoName = _telco.TelcoName;
            telco.Description = _telco.Description; 
            telco.Logo = image_path;
            _context.Entry(telco).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TelcoExists(id))
                {
                    result_0 = new { status = 0, message = "TelcoID is not exists" };
                    RsJson_0 = JsonConvert.SerializeObject(result_0);
                    return StatusCode(400, RsJson_0);
                }
                else
                {
                    throw;
                }
            }
            //kiểm tra nếu ảnh mới khác ảnh cũ thì xóa ảnh cũ
            if (image_path != old_image)
            {
                FileController.DeleteFile(old_image);
            }
            result_0 = new { status = 1, message = "Update Telco successfully" };
            RsJson_0 = JsonConvert.SerializeObject(result_0);
            return Ok(RsJson_0);
        }

        //delete a telco
        [HttpDelete("Telco/{id}")]
        public async Task<IActionResult> DeleteTelco(int id, string token)
        {
            object result_0;
            string RsJson_0 = String.Empty;
            if (!CheckRole(token))
            {
                result_0 = new { status = 0, message = "Permission denie., Please login with admin account or try to login again" };
                RsJson_0 = JsonConvert.SerializeObject(result_0);
                return StatusCode(401, RsJson_0);
            }

            var telco = await _context.Telcos.FindAsync(id);
            if (telco == null)
            {
                result_0 = new { status = 0, message = "TelcoID is not exists" };
                RsJson_0 = JsonConvert.SerializeObject(result_0);
                return StatusCode(400, RsJson_0);
            }
            _context.Telcos.Remove(telco);
            await _context.SaveChangesAsync();
            result_0 = new { status = 1, message = "Delete Telco successfully" };
            RsJson_0 = JsonConvert.SerializeObject(result_0);
            return Ok(RsJson_0);
        }

        //TelcoExists
        [HttpGet("Telco/TelcoExists")]
        private bool TelcoExists(int id)
        {
            return _context.Telcos.Any(e => e.TelcoID == id);
        }

        //GetTelco
        [HttpGet("Telco/{id}")]
        public async Task<ActionResult<Telco>> GetTelco(int id)
        {
            var telco = await _context.Telcos.FindAsync(id);
            if (telco == null)
            {
                return NotFound();
            }
            return telco;
        }

        #endregion

        #region USER
        //get all users
        [HttpGet("User")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers(string token)
        {
            if (!CheckRole(token))
            {
                return Unauthorized();
            }
            return await _context.Users.ToListAsync();
        }

        #endregion

        #region CALLER TUNES PACKAGE
        //get all caller tunes packages
        [HttpGet("CallerTunesPackage")]
        public async Task<ActionResult<IEnumerable<CallerTunesPackage>>> GetCallerTunesPackages(string token)
        {
            if (!CheckRole(token))
            {
                return Unauthorized();
            }
            return await _context.CallerTunesPackages.ToListAsync();
        }

        //create a new caller tunes package
        [HttpPost("CallerTunesPackage/Create")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<CallerTunesPackage>> PostCallerTunesPackage(CallerTunesPackageDTO _callerTunesPackage, string token)
        {
            object result_0;
            string RsJson_0 = String.Empty;
            if (!CheckRole(token))
            {
                result_0 = new { status = 0, message = "Permission denie., Please login with admin account or try to login again" };
                RsJson_0 = JsonConvert.SerializeObject(result_0);
                return StatusCode(401, RsJson_0);
            }
            //xử lý ảnh
            string mp3_path = "";
            if (_callerTunesPackage.Mp3 != null)
            {
                if (!FileController.CheckImageSize(_callerTunesPackage.Mp3, 100))
                {
                    result_0 = new { status = 0, message = "File size is too large" };
                    RsJson_0 = JsonConvert.SerializeObject(result_0);
                    return StatusCode(400, RsJson_0);
                }
                //check extension
                if (!FileController.CheckMp3IsValid(_callerTunesPackage.Mp3))
                {
                    result_0 = new { status = 0, message = "Mp3 is not valid" };
                    RsJson_0 = JsonConvert.SerializeObject(result_0);
                    return StatusCode(400, RsJson_0);
                }
                mp3_path = FileController.UploadImage(_callerTunesPackage.Mp3);
            }
            else
            {
                result_0 = new { status = 0, message = "Mp3 is required" };
                RsJson_0 = JsonConvert.SerializeObject(result_0);
                return StatusCode(400, RsJson_0);
            }
            //create a new caller tunes package
            CallerTunesPackage callerTunesPackage = new CallerTunesPackage
            {
                PackageName = _callerTunesPackage.PackageName,
                Amount = _callerTunesPackage.Amount,
                Validity = _callerTunesPackage.Validity,
                Status = _callerTunesPackage.Status,
                Icon = mp3_path
            };

            _context.CallerTunesPackages.Add(callerTunesPackage);
            await _context.SaveChangesAsync();

            result_0 = new { status = 1, message = "Create Caller Tunes Package successfully" };
            RsJson_0 = JsonConvert.SerializeObject(result_0);
            return Ok(RsJson_0);
        }

        //update a caller tunes package
        [HttpPut("CallerTunesPackage/{id}")]
        public async Task<IActionResult> PutCallerTunesPackage(int id, CallerTunesPackageDTO _callerTunesPackage, string token)
        {
            object result_0;
            string RsJson_0 = String.Empty;
            if (!CheckRole(token))
            {
                result_0 = new { status = 0, message = "Permission denie., Please login with admin account or try to login again" };
                RsJson_0 = JsonConvert.SerializeObject(result_0);
                return StatusCode(401, RsJson_0);
            }
            var callerTunesPackage = _context.CallerTunesPackages.Find(id) ?? null;

            if (callerTunesPackage == null)
            {
                result_0 = new { status = 0, message = "CallerTunesPackageID is not exists" };
                RsJson_0 = JsonConvert.SerializeObject(result_0);
                return StatusCode(400, RsJson_0);
            }
            //xử lý ảnh
            string mp3 = "";
            if (_callerTunesPackage.Mp3 != null)
            {
                mp3 = FileController.UploadImage(_callerTunesPackage.Mp3);
            }
            else
            {
                mp3 =  callerTunesPackage.Icon;
            }

            callerTunesPackage.PackageName = _callerTunesPackage.PackageName;
            callerTunesPackage.Amount = _callerTunesPackage.Amount;
            callerTunesPackage.Validity = _callerTunesPackage.Validity;
            callerTunesPackage.Status = _callerTunesPackage.Status;
            callerTunesPackage.Icon = mp3;

            _context.Entry(callerTunesPackage).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CallerTunesPackageExists(id))
                {
                    result_0 = new { status = 0, message = "CallerTunesPackageID is not exists" };
                    RsJson_0 = JsonConvert.SerializeObject(result_0);
                    return StatusCode(400, RsJson_0);
                }
                else
                {
                    result_0 = new { status = 0, message = "Update Caller Tunes Package failed" };
                    RsJson_0 = JsonConvert.SerializeObject(result_0);
                    return StatusCode(500, RsJson_0);
                }
            }
            //kiểm tra nếu ảnh mới khác ảnh cũ thì xóa ảnh cũ
            if (mp3 != callerTunesPackage.Icon)
            {
                FileController.DeleteFile(callerTunesPackage.Icon);
            }
            result_0 = new { status = 1, message = "Update Caller Tunes Package successfully" };
            RsJson_0 = JsonConvert.SerializeObject(result_0);
            return Ok(RsJson_0);
        }

        //delete a caller tunes package
        [HttpDelete("CallerTunesPackage/{id}")]
        public async Task<IActionResult> DeleteCallerTunesPackage(int id, string token)
        {
            object result_0;
            string RsJson_0 = String.Empty;
            if (!CheckRole(token))
            {
                result_0 = new { status = 0, message = "Permission denie., Please login with admin account or try to login again" };
                RsJson_0 = JsonConvert.SerializeObject(result_0);
                return StatusCode(401, RsJson_0);
            }

            var callerTunesPackage = await _context.CallerTunesPackages.FindAsync(id);
            if (callerTunesPackage == null)
            {
                result_0 = new { status = 0, message = "CallerTunesPackageID is not exists" };
                RsJson_0 = JsonConvert.SerializeObject(result_0);
                return StatusCode(400, RsJson_0);
            }
            _context.CallerTunesPackages.Remove(callerTunesPackage);
            await _context.SaveChangesAsync();
            result_0 = new { status = 1, message = "Delete Caller Tunes Package successfully" };
            RsJson_0 = JsonConvert.SerializeObject(result_0);
            return Ok(RsJson_0);
        }

        //CallerTunesPackageExists
        [HttpGet("CallerTunesPackage/CallerTunesPackageExists")]
        private bool CallerTunesPackageExists(int id)
        {
            return _context.CallerTunesPackages.Any(e => e.PackageID == id);
        }

        //GetCallerTunesPackage
        [HttpGet("CallerTunesPackage/{id}")]
        public async Task<ActionResult<CallerTunesPackage>> GetCallerTunesPackage(int id)
        {
            var callerTunesPackage = await _context.CallerTunesPackages.FindAsync(id);
            if (callerTunesPackage == null)
            {
                return NotFound();
            }
            return callerTunesPackage;
        }

        #endregion


        #region WEB SETTINGS
        //get all web settings
        [HttpGet("WebSetting")]
        public async Task<ActionResult<IEnumerable<WebsiteSettings>>> GetWebSettings(string token)
        {
            if (!CheckRole(token))
            {
                return Unauthorized();
            }
            return await _context.WebsiteSettings.ToListAsync();
        }

        //create a new web setting
        [HttpPost("WebSetting/Create")]
        public async Task<ActionResult<WebsiteSettings>> PostWebSetting(WebsiteSettingDTO _websiteSettings, string token)
        {
            if (!CheckRole(token))
            {
                return Unauthorized();
            }
            //create a new web setting
            WebsiteSettings websiteSettings = new WebsiteSettings
            {
                Name = _websiteSettings.Name,
                Value = _websiteSettings.Value
            };

            _context.WebsiteSettings.Add(websiteSettings);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWebSetting", new { id = websiteSettings.ID }, _websiteSettings);
        }

        //update a web setting
        [HttpPut("WebSetting/{id}")]
        public async Task<IActionResult> PutWebSetting(int id, WebsiteSettingDTO _websiteSettings, string token)
        {
            if (!CheckRole(token))
            {
                return Unauthorized();
            }
            var websiteSettings = _context.WebsiteSettings.Find(id) ?? null;

            if (websiteSettings == null)
            {
                return NotFound();
            }

            websiteSettings.Name = _websiteSettings.Name;
            websiteSettings.Value = _websiteSettings.Value;

            _context.Entry(websiteSettings).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WebsiteSettingsExists(id))
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

        //delete a web setting
        [HttpDelete("WebSetting/{id}")]
        public async Task<IActionResult> DeleteWebSetting(int id, string token)
        {
            if (!CheckRole(token))
            {
                return Unauthorized();
            }

            var websiteSettings = await _context.WebsiteSettings.FindAsync(id);
            if (websiteSettings == null)
            {
                return NotFound();
            }
            _context.WebsiteSettings.Remove(websiteSettings);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        //WebsiteSettingsExists
        [HttpGet("WebSetting/WebsiteSettingsExists")]
        private bool WebsiteSettingsExists(int id)
        {
            return _context.WebsiteSettings.Any(e => e.ID == id);
        }

        //GetWebSetting
        [HttpGet("WebSetting/{id}")]
        public async Task<ActionResult<WebsiteSettings>> GetWebSetting(int id)
        {
            var websiteSettings = await _context.WebsiteSettings.FindAsync(id);
            if (websiteSettings == null)
            {
                return NotFound();
            }
            return websiteSettings;
        }

        #endregion

        #region  History

        //GetCallerTunesHistory
        [HttpGet("History/CallerTunes")]
        public async Task<ActionResult<IEnumerable<CallerTunesHistory>>> GetCallerTunesHistory(string token, string Date = null)
        {
            if (!CheckRole(token))
            {
                return Unauthorized();
            }
            if (Date == null)
            {
                return await _context.CallerTunesHistories.ToListAsync();
            }
            else
            {
                return await _context.CallerTunesHistories.Where(x => x.Date.ToString().Contains(Date)).ToListAsync();
            }
        }

        //GetRechargeHistory
        [HttpGet("History/Recharge")]
        public async Task<ActionResult<IEnumerable<RechargeHistory>>> GetRechargeHistory(string token, string Date = null)
        {
            if (!CheckRole(token))
            {
                return Unauthorized();
            }
            if (Date == null)
            {
                return await _context.RechargeHistories.ToListAsync();
            }
            else
            {
                return await _context.RechargeHistories.Where(x => x.Date.ToString().Contains(Date)).ToListAsync();
            }
        }
        //special
        [HttpGet("History/Recharge/special")]
        public async Task<ActionResult<IEnumerable<RechargeHistory>>> GetSpecialRechargeHistory(string token, string Date = null)
        {
            if (!CheckRole(token))
            {
                return Unauthorized();
            }
            if (Date == null)
            {
                return await _context.RechargeHistories.ToListAsync();
            }
            else
            {
                return await _context.RechargeHistories.Where(x => x.Date.ToString().Contains(Date)).ToListAsync();
            }
        }

        //GetDoNotDisturbHistory
        [HttpGet("History/DoNotDisturb")]
        public async Task<ActionResult<IEnumerable<DoNotDisturbHistory>>> GetDoNotDisturbHistory(string token, string Date = null)
        {
            if (!CheckRole(token))
            {
                return Unauthorized();
            }
            if (Date == null)
            {
                return await _context.DoNotDisturbHistories.ToListAsync();
            }
            else
            {
                return await _context.DoNotDisturbHistories.Where(x => x.Date.ToString().Contains(Date)).ToListAsync();
            }
        }

        //Get PostPaidHistory
        [HttpGet("History/PostPaid")]
        public async Task<ActionResult<IEnumerable<PostPaidHistory>>> GetPostPaidHistory(string token, string Date = null)
        {
            if (!CheckRole(token))
            {
                return Unauthorized();
            }
            if (Date == null)
            {
                return await _context.PostPaidHistories.ToListAsync();
            }
            else
            {
                return await _context.PostPaidHistories.Where(x => x.Date.ToString().Contains(Date)).ToListAsync();
            }
        }

        #endregion

































        //check role 
        [NonAction]
        private bool CheckRole(string token)
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