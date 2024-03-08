using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using OnlineMobileServices_FE;
using OnlineMobileServices_Models.Models;

namespace OnlineMobileServices.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("[controller]")]
    public class RechargeController : Controller
    {

        private readonly IMemoryCache _memoryCache;

        private readonly HttpClient _client = new HttpClient();

        public RechargeController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;

        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var Packages = await _client.GetAsync($"{Program.API_URL}/RechargePackage");
                if (Packages.IsSuccessStatusCode)
                {
                    var packageList = await Packages.Content.ReadFromJsonAsync<List<RechargePackage>>();
                    return View(packageList);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (System.Exception)
            {
                //404
                return NotFound();
            }
        }

        [HttpGet("Recharge/{id}")]
        public async Task<IActionResult> Recharge(int id)
        {
            try
            {
                var Package = await _client.GetFromJsonAsync<RechargePackage>($"{Program.API_URL}/RechargePackage/{id}");
                if (Package == null)
                {
                    return NotFound();
                }
                return View(Package);
            }
            catch (System.Exception)
            {
                //404
                return NotFound();
            }
        }

        


    }
}