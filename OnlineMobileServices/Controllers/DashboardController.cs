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
using OnlineMobileServices_Models.DTOs;
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

            if (!Admin().IsAdmin)
            {
                return RedirectToAction("Index", "Home");
            }


            return View();
        }


        #region Recharge and Special Recharge Dashboard
        [HttpGet("Recharge")]
        public async Task<IActionResult> Recharge()
        {
            if (!Admin().IsAdmin)
            {
                return RedirectToAction("Index", "Home");
            }
            var Packages = await _client.GetAsync($"{DASHBOARD_API_URL}/RechargePackage?token={Admin().Token}");
            if (Packages.IsSuccessStatusCode)
            {
                var packageList = await Packages.Content.ReadFromJsonAsync<List<RechargePackage>>();
                return View(packageList);
            }
            return View();
        }

        //CreateRechargePackages
        [HttpGet("CreateRechargePackages")]
        public async Task<IActionResult> CreateRechargePackages()
        {
            if (!Admin().IsAdmin)
            {
                return RedirectToAction("Index", "Home");
            }
            var telcos = await _client.GetAsync($"{DASHBOARD_API_URL}/Telco?token={Admin().Token}");
            ViewBag.Telcos = await telcos.Content.ReadFromJsonAsync<List<Telco>>();
            return View();
        }

        //EditRechargePackages
        [HttpGet("EditRechargePackages")]
        public async Task<IActionResult> EditRechargePackages(int id)
        {
            if (!Admin().IsAdmin)
            {
                return RedirectToAction("Index", "Home");
            }
            var Package = await _client.GetFromJsonAsync<RechargePackage>($"{DASHBOARD_API_URL}/RechargePackage/{id}?token={Admin().Token}");
            if (Package == null)
            {
                return NotFound();
            }
            var telcos = await _client.GetAsync($"{DASHBOARD_API_URL}/Telco?token={Admin().Token}");
            ViewBag.Telcos = await telcos.Content.ReadFromJsonAsync<List<Telco>>();
            return View(Package);
        }




        [HttpGet("SpecialRecharge")]
        public async Task<IActionResult> SpecialRecharge()
        {
            if (!Admin().IsAdmin)
            {
                return RedirectToAction("Index", "Home");
            }
            var Packages = await _client.GetAsync($"{DASHBOARD_API_URL}/SpecialRecharge?token={Admin().Token}");
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
            if (!Admin().IsAdmin)
            {
                return RedirectToAction("Index", "Home");
            }
            var RechargeHistory = await _client.GetAsync($"{DASHBOARD_API_URL}/RechargeHistory?token={Admin().Token}");
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
            if (!Admin().IsAdmin)
            {
                return RedirectToAction("Index", "Home");
            }
            var SpecialRechargeHistory = await _client.GetAsync($"{DASHBOARD_API_URL}/SpecialRechargeHistory?token={Admin().Token}");
            if (SpecialRechargeHistory.IsSuccessStatusCode)
            {
                var specialRechargeHistoryList = await SpecialRechargeHistory.Content.ReadFromJsonAsync<List<SpecialRechargeHistory>>();
                return View(specialRechargeHistoryList);
            }
            return View();
        }


        #endregion

        #region Telco Dashboard
        [HttpGet("Telco")]
        public async Task<IActionResult> Telco()
        {
            if (!Admin().IsAdmin)
            {
                return RedirectToAction("Index", "Home");
            }
            var Telcos = await _client.GetAsync($"{DASHBOARD_API_URL}/Telco?token={Admin().Token}");
            if (Telcos.IsSuccessStatusCode)
            {
                var telcoList = await Telcos.Content.ReadFromJsonAsync<List<Telco>>();
                return View(telcoList);
            }
            return View();
        }

        [HttpGet("CreateTelco")]
        public IActionResult CreateTelco()
        {
            if (!Admin().IsAdmin)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpGet("EditTelco")]
        public async Task<IActionResult> EditTelco(int id)
        {
            if (!Admin().IsAdmin)
            {
                return RedirectToAction("Index", "Home");
            }
            var Telco = await _client.GetFromJsonAsync<Telco>($"{DASHBOARD_API_URL}/Telco/{id}?token={Admin().Token}");
            if (Telco == null)
            {
                return NotFound();
            }
            return View(Telco);
        }

        #endregion

        #region  DoNotDisturb
        [HttpGet("DoNotDisturb")]
        public async Task<IActionResult> DoNotDisturb()
        {
            if (!Admin().IsAdmin)
            {
                return RedirectToAction("Index", "Home");
            }
            var DoNotDisturb = await _client.GetAsync($"{DASHBOARD_API_URL}/History/DoNotDisturb/?token={Admin().Token}");
            if (DoNotDisturb.IsSuccessStatusCode)
            {
                var doNotDisturbList = await DoNotDisturb.Content.ReadFromJsonAsync<List<DoNotDisturbHistory>>();
                return View(doNotDisturbList);
            }
            return View();
        }
        #endregion

        #region  CallerTunes
        [HttpGet("CallerTunes")]
        public async Task<IActionResult> CallerTunes()
        {
            if (!Admin().IsAdmin)
            {
                return RedirectToAction("Index", "Home");
            }
            var CallerTunes = await _client.GetAsync($"{DASHBOARD_API_URL}/History/CallerTunes/?token={Admin().Token}");
            if (CallerTunes.IsSuccessStatusCode)
            {
                var callerTunesList = await CallerTunes.Content.ReadFromJsonAsync<List<CallerTunesPackage>>();
                return View(callerTunesList);
            }
            return View();
        }
        //create
        [HttpGet("CreateCallerTunes")]
        public async Task<IActionResult> CreateCallerTunes()
        {
            if (!Admin().IsAdmin)
            {
                return RedirectToAction("Index", "Home");
            }
            var telcos = await _client.GetAsync($"{DASHBOARD_API_URL}/Telco?token={Admin().Token}");
            ViewBag.Telcos = await telcos.Content.ReadFromJsonAsync<List<Telco>>();
            return View();
        }
        //edit
        [HttpGet("EditCallerTunes/{id}")]
        public async Task<IActionResult> EditCallerTunes(int id)
        {
            if (!Admin().IsAdmin)
            {
                return RedirectToAction("Index", "Home");
            }
            var Package = await _client.GetFromJsonAsync<CallerTunesPackage>($"{DASHBOARD_API_URL}/CallerTunes/{id}?token={Admin().Token}");
            if (Package == null)
            {
                return NotFound();
            }
            var telcos = await _client.GetAsync($"{DASHBOARD_API_URL}/Telco?token={Admin().Token}");
            ViewBag.Telcos = await telcos.Content.ReadFromJsonAsync<List<Telco>>();
            return View(Package);
        }
        //history
        [HttpGet("CallerTunesHistory")]
        public async Task<IActionResult> CallerTunesHistory()
        {
            if (!Admin().IsAdmin)
            {
                return RedirectToAction("Index", "Home");
            }
            var CallerTunesHistory = await _client.GetAsync($"{DASHBOARD_API_URL}/History/CallerTunesHistory/?token={Admin().Token}");
            if (CallerTunesHistory.IsSuccessStatusCode)
            {
                var callerTunesHistoryList = await CallerTunesHistory.Content.ReadFromJsonAsync<List<CallerTunesHistory>>();
                return View(callerTunesHistoryList);
            }
            return View();
        }
        #endregion

        #region  Postpaid
        [HttpGet("Postpaid")]
        public async Task<IActionResult> Postpaid()
        {
            if (!Admin().IsAdmin)
            {
                return RedirectToAction("Index", "Home");
            }
            var Postpaid = await _client.GetAsync($"{DASHBOARD_API_URL}/History/Postpaid/?token={Admin().Token}");
            if (Postpaid.IsSuccessStatusCode)
            {
                var postpaidList = await Postpaid.Content.ReadFromJsonAsync<List<PostPaidHistory>>();
                return View(postpaidList);
            }
            return View();
        }
        #endregion

        private AdminUser Admin()
        {
            AdminUser au = new AdminUser();
            au.IsAdmin = false;
            au.Token = String.Empty;
            var userJson = HttpContext.Session.GetString("User");
            if (userJson == null)
            {
                return au;
            }
            var user = JsonConvert.DeserializeObject<User>(userJson);
            if (user == null || user.Role != "Admin")
            {
                return au;
            }
            // HttpContext.Session.SetString("Token", token);
            var Token = HttpContext.Session.GetString("Token");
            if (Token == null)
            {
                return au;
            }
            au.IsAdmin = true;
            au.Token = Token;
            return au;
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }


        private class AdminUser
        {
            public string? Token { get; set; }
            public bool IsAdmin { get; set; }
        }
    }
}