using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OnlineMobileServices_FE;
using OnlineMobileServices_Models.Models;
using System.Management;
namespace OnlineMobileServices.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("[controller]")]
    public class DashboardController : Controller
    {
        private readonly ILogger<DashboardController> _logger;

        private readonly HttpClient _client = new HttpClient();

        private readonly string DASHBOARD_API_URL = $"{Program.API_URL}/Dashboard";

        public DashboardController(ILogger<DashboardController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        public IActionResult Index()
        {
           

            var userJson = HttpContext.Session.GetString("User");
            if (userJson == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var user = JsonConvert.DeserializeObject<User>(userJson);
            if (user == null || user.Role != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }


            return View();
        }


        #region Recharge and Special Recharge Dashboard
        [HttpGet("Recharge")]
        public async Task<IActionResult> Recharge()
        {
            var Packages = await _client.GetAsync($"{DASHBOARD_API_URL}/CallerTunes");
            if (Packages.IsSuccessStatusCode)
            {
                var packageList = await Packages.Content.ReadFromJsonAsync<List<CallerTunesPackage>>();
                return View(packageList);
            }
            return View();
        }
        [HttpGet("SpecialRecharge")]
        public async Task<IActionResult> SpecialRecharge()
        {
            var Packages = await _client.GetAsync($"{DASHBOARD_API_URL}/SpecialRecharge");
            if (Packages.IsSuccessStatusCode)
            {
                var packageList = await Packages.Content.ReadFromJsonAsync<List<SpecialRechargePackage>>();
                return View(packageList);
            }
            return View();
        }

        [HttpGet("RechargeHistory")]
        public async Task<IActionResult> RechargeHistory()
        {
            var RechargeHistory = await _client.GetAsync($"{DASHBOARD_API_URL}/RechargeHistory");
            if (RechargeHistory.IsSuccessStatusCode)
            {
                var rechargeHistoryList = await RechargeHistory.Content.ReadFromJsonAsync<List<RechargeHistory>>();
                return View(rechargeHistoryList);
            }
            return View();
        }

        [HttpGet("SpecialRechargeHistory")]
        public async Task<IActionResult> SpecialRechargeHistory()
        {
            var SpecialRechargeHistory = await _client.GetAsync($"{DASHBOARD_API_URL}/SpecialRechargeHistory");
            if (SpecialRechargeHistory.IsSuccessStatusCode)
            {
                var specialRechargeHistoryList = await SpecialRechargeHistory.Content.ReadFromJsonAsync<List<SpecialRechargeHistory>>();
                return View(specialRechargeHistoryList);
            }
            return View();
        }










































































        #endregion
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}