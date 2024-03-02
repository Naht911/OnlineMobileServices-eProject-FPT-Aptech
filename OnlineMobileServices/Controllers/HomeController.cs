using Microsoft.AspNetCore.Mvc;
using OnlineMobileServices_FE.Models;
using System.Diagnostics;
using Newtonsoft.Json;
using OnlineMobileServices_Models.Models;
using OnlineMobileServices_Models.DTOs;
using System.Text;

namespace OnlineMobileServices_FE.Controllers
{
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        HttpClient client = new HttpClient();
        private string url = $"{Program.ApiUrl}/";
        private string url_user = $"{Program.ApiUrl}/User";


        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            Console.WriteLine("Error");
            return View(new Models.ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult<User>> Login(UserLoginDTO userDTOLogin)
        {
            try
            {
                //kiểm tra xem có session chưa
                var userJson = HttpContext.Session.GetString("User");
                if (userJson != null)
                {
                    var user = JsonConvert.DeserializeObject<User>(userJson);
                    var obj = new
                    {
                        status = 1,
                        message = $"Already login",
                        data = user
                    };
                    var json = JsonConvert.SerializeObject(obj);
                    return StatusCode(200, json);
                }


                var response = client.PostAsJsonAsync<UserLoginDTO>($"{url_user}/Login", userDTOLogin).Result;

                if (response.IsSuccessStatusCode)
                {
                    //return json to client
                    var result = response.Content.ReadAsStringAsync().Result;
                    var _user = JsonConvert.DeserializeObject<User>(result);
                    // save _user to session
                    HttpContext.Session.SetString("User", JsonConvert.SerializeObject(_user));
                    var obj = new
                    {
                        status = 1,
                        message = $"Login sucsses",
                        data = _user
                    };
                    var json = JsonConvert.SerializeObject(obj);
                    return StatusCode(200, json);

                }
                var errorObject = new { message = $"Incorrect phone number or password {userDTOLogin.MobileNumber} - {userDTOLogin.MobileNumber}" };
                var errorJson = JsonConvert.SerializeObject(response);
                return StatusCode(401, errorJson);
            }
            catch (System.Exception)
            {

                var errorObject = new { message = "Something went wrong" };
                var errorJson = JsonConvert.SerializeObject(errorObject);
                return StatusCode(500, errorJson); // Trả về mã lỗi 500 Internal Server Error với thông báo JSON tùy chỉnh
            }

        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            var data = JsonConvert.SerializeObject(user);
            var content = new StringContent(data, Encoding.UTF8, "application/json");
            var response = client.PostAsync(url_user, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Login");
            }
            return View();
        }



    }
}
