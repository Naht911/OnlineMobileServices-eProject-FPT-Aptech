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

        //SpecialRechargePackage
        [HttpGet("Special/{id}")]
        public async Task<IActionResult> SpecialRecharge(int id)
        {
            try
            {
                var Package = await _client.GetFromJsonAsync<SpecialRechargePackage>($"{Program.API_URL}/RechargePackage/Special/{id}");
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

        //SpecialRechargePackage index
        [HttpGet("Special")]
        public async Task<IActionResult> SpecialRechargePackage()
        {
            try
            {
                var Packages = await _client.GetAsync($"{Program.API_URL}/RechargePackage/Special");
                Console.WriteLine($"{Program.API_URL}/RechargePackage/Special");
                if (Packages.IsSuccessStatusCode)
                {
                    Console.WriteLine("SpecialRechargePackage ok");
                    var packageList = await Packages.Content.ReadFromJsonAsync<List<SpecialRechargePackage>>();
                    return View(packageList);
                }
                else
                {
                    return NotFound("SpecialRechargePackage not found or invalid");
                }
            }
            catch (System.Exception e)
            {
                //404
                throw e;
                // return NotFound();
            }
        }




    }
}