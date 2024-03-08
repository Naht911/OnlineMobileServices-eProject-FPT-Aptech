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
    public class CallerTunesController : Controller
    {
        private readonly ILogger<CallerTunesController> _logger;

        private readonly HttpClient _client = new HttpClient();

        private readonly IMemoryCache _memoryCache;

        public CallerTunesController(ILogger<CallerTunesController> logger, IMemoryCache memoryCache)
        {
            _logger = logger;
            _memoryCache = memoryCache;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var Packages = await _client.GetAsync($"{Program.API_URL}/CallerTunes");
                if (Packages.IsSuccessStatusCode)
                {
                    var packageList = await Packages.Content.ReadFromJsonAsync<List<CallerTunesPackage>>();
                    return View(packageList);
                }
                else
                {
                    return View();
                }
            }
            catch (System.Exception)
            {
                //404
                return View();
            }
        }

        [HttpGet("CallerTunes/{id}")]
        public async Task<IActionResult> CallerTunes(int id)
        {
            try
            {
                var Package = await _client.GetFromJsonAsync<CallerTunesPackage>($"{Program.API_URL}/CallerTunes/{id}");
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


















        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}