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
    public class HistoryController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly UserService _userService;
        public HistoryController(DatabaseContext context, UserService userService)
        {
            _context = context;
            _userService = userService;
        }

        //RechargeHistory
        [HttpGet("RechargeHistories")]
        public async Task<ActionResult<IEnumerable<RechargeHistory>>> GetRechargeHistories()
        {
            return await _context.RechargeHistories.ToListAsync();
        }

        [HttpGet("Recharge/{id}")]
        public async Task<ActionResult<RechargeHistory>> GetRechargeHistory(int id)
        {
            var rechargeHistory = await _context.RechargeHistories.FindAsync(id);

            if (rechargeHistory == null)
            {
                return NotFound();
            }

            return rechargeHistory;
        }
        //Delete RechargeHistory
        [HttpDelete("Recharge/{id}")]
        public async Task<IActionResult> DeleteRechargeHistory(int id)
        {
            var rechargeHistory = await _context.RechargeHistories.FindAsync(id);
            if (rechargeHistory == null)
            {
                return NotFound();
            }

            _context.RechargeHistories.Remove(rechargeHistory);
            await _context.SaveChangesAsync();

            return Ok();
        }

        //update status of RechargeHistory
        [HttpPut("Recharge/{id}")]
        public async Task<IActionResult> PutRechargeHistory(int id, String status)
        {
            var rechargeHistory = await _context.RechargeHistories.FindAsync(id);
            if (rechargeHistory == null)
            {
                return NotFound();
            }
            rechargeHistory.Status = status;
            _context.Entry(rechargeHistory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

            }

            return Ok();
        }


        //SpecialRechargeHistory
        [HttpGet("SpecialRechargeHistories")]
        public async Task<ActionResult<IEnumerable<RechargeHistory>>> GetSpecialRechargeHistories()
        {
            return await _context.RechargeHistories.ToListAsync();
        }

        [HttpGet("SpecialRecharge/{id}")]
        public async Task<ActionResult<RechargeHistory>> GetSpecialRechargeHistory(int id)
        {
            var rechargeHistory = await _context.RechargeHistories.FindAsync(id);

            if (rechargeHistory == null)
            {
                return NotFound();
            }

            return rechargeHistory;
        }

        //Delete SpecialRechargeHistory
        [HttpDelete("SpecialRecharge/{id}")]
        public async Task<IActionResult> DeleteSpecialRechargeHistory(int id)
        {
            var rechargeHistory = await _context.RechargeHistories.FindAsync(id);
            if (rechargeHistory == null)
            {
                return NotFound();
            }

            _context.RechargeHistories.Remove(rechargeHistory);
            await _context.SaveChangesAsync();

            return Ok();
        }

        //update status of SpecialRechargeHistory
        [HttpPut("SpecialRecharge/{id}")]
        public async Task<IActionResult> PutSpecialRechargeHistory(int id, String status)
        {
            var rechargeHistory = await _context.RechargeHistories.FindAsync(id);
            if (rechargeHistory == null)
            {
                return NotFound();
            }
            rechargeHistory.Status = status;
            _context.Entry(rechargeHistory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

            }

            return Ok();
        }



        //PostPaidHistory
        [HttpGet("PostPaidHistories")]
        public async Task<ActionResult<IEnumerable<PostPaidHistory>>> GetPostPaidHistories()
        {
            return await _context.PostPaidHistories.ToListAsync();
        }

        [HttpGet("PostPaid/{id}")]
        public async Task<ActionResult<PostPaidHistory>> GetPostPaidHistory(int id)
        {
            var postPaidHistory = await _context.PostPaidHistories.FindAsync(id);

            if (postPaidHistory == null)
            {
                return NotFound();
            }

            return postPaidHistory;
        }

        //Delete PostPaidHistory
        [HttpDelete("PostPaid/{id}")]
        public async Task<IActionResult> DeletePostPaidHistory(int id)
        {
            var postPaidHistory = await _context.PostPaidHistories.FindAsync(id);
            if (postPaidHistory == null)
            {
                return NotFound();
            }

            _context.PostPaidHistories.Remove(postPaidHistory);
            await _context.SaveChangesAsync();

            return Ok();
        }

        //update status of PostPaidHistory
        [HttpPut("PostPaid/{id}")]
        public async Task<IActionResult> PutPostPaidHistory(int id, PostPaidHistory postPaidHistory)
        {
            if (id != postPaidHistory.HistoryID)
            {
                return BadRequest();
            }

            _context.Entry(postPaidHistory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

            }

            return Ok();
        }


        //CallerTunesHistory
        [HttpGet("CallerTunesHistories")]
        public async Task<ActionResult<IEnumerable<CallerTunesHistory>>> GetCallerTunesHistories()
        {
            return await _context.CallerTunesHistories.ToListAsync();
        }

        [HttpGet("CallerTunes/{id}")]
        public async Task<ActionResult<CallerTunesHistory>> GetCallerTunesHistory(int id)
        {
            var callerTunesHistory = await _context.CallerTunesHistories.FindAsync(id);

            if (callerTunesHistory == null)
            {
                return NotFound();
            }

            return callerTunesHistory;
        }

        //Delete CallerTunesHistory
        [HttpDelete("CallerTunes/{id}")]
        public async Task<IActionResult> DeleteCallerTunesHistory(int id)
        {
            var callerTunesHistory = await _context.CallerTunesHistories.FindAsync(id);
            if (callerTunesHistory == null)
            {
                return NotFound();
            }

            _context.CallerTunesHistories.Remove(callerTunesHistory);
            await _context.SaveChangesAsync();

            return Ok();
        }

        //update status of CallerTunesHistory
        [HttpPut("CallerTunes/{id}")]
        public async Task<IActionResult> PutCallerTunesHistory(int id, CallerTunesHistory callerTunesHistory)
        {
            if (id != callerTunesHistory.HistoryID)
            {
                return BadRequest();
            }

            _context.Entry(callerTunesHistory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

            }

            return Ok();
        }



        //DoNotDisturbHistory
        [HttpGet("DoNotDisturbHistories")]
        public async Task<ActionResult<IEnumerable<DoNotDisturbHistory>>> GetDoNotDisturbHistories()
        {
            return await _context.DoNotDisturbHistories.ToListAsync();
        }

        [HttpGet("DoNotDisturb/{id}")]
        public async Task<ActionResult<DoNotDisturbHistory>> GetDoNotDisturbHistory(int id)
        {
            var doNotDisturbHistory = await _context.DoNotDisturbHistories.FindAsync(id);

            if (doNotDisturbHistory == null)
            {
                return NotFound();
            }

            return doNotDisturbHistory;
        }

        //Delete DoNotDisturbHistory
        [HttpDelete("DoNotDisturb/{id}")]
        public async Task<IActionResult> DeleteDoNotDisturbHistory(int id)
        {
            var doNotDisturbHistory = await _context.DoNotDisturbHistories.FindAsync(id);
            if (doNotDisturbHistory == null)
            {
                return NotFound();
            }

            _context.DoNotDisturbHistories.Remove(doNotDisturbHistory);
            await _context.SaveChangesAsync();

            return Ok();
        }

        //update status of DoNotDisturbHistory
        [HttpPut("DoNotDisturb/{id}")]
        public async Task<IActionResult> PutDoNotDisturbHistory(int id, DoNotDisturbHistory doNotDisturbHistory)
        {
            if (id != doNotDisturbHistory.HistoryID)
            {
                return BadRequest();
            }

            _context.Entry(doNotDisturbHistory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

            }

            return Ok();
        }




    }
}