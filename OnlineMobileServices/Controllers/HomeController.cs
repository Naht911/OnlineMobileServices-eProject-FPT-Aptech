using Microsoft.AspNetCore.Mvc;
using OnlineMobileServices_FE.Models;
using System.Diagnostics;
using Newtonsoft.Json;
using OnlineMobileServices_Models.Models;
using OnlineMobileServices_Models.DTOs;
using System.Text;

namespace OnlineMobileServices_FE.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        HttpClient client = new HttpClient();
        private string url = $"{Program.API_URL}/";
        private string url_user = $"{Program.API_URL}/User";


        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var Telco = client.GetFromJsonAsync<IEnumerable<Telco>>($"{url}Telco").Result;
            ViewBag.Telco = Telco;
            return View();
        }
        [HttpGet("Privacy")]
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
        [HttpGet("Login")]
        public IActionResult Login()
        {
            //check have session or not
            var userJson = HttpContext.Session.GetString("User");
            if (userJson != null)
            {
                var user = JsonConvert.DeserializeObject<User>(userJson);
                if (user != null)
                {
                    return RedirectToAction("Index", "Home");
                }

            }
            return View();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginDTO userDTOLogin)
        {
            try
            {
                Object errorObject;
                var errorJson = "";
                //check null
                if (userDTOLogin.MobileNumber == null || userDTOLogin.Password == null)
                {
                    errorObject = new { message = "Please enter phone number and password" };
                    errorJson = JsonConvert.SerializeObject(errorObject);
                    return StatusCode(400, errorJson);
                }

                //check 10 digit phone number
                if (userDTOLogin.MobileNumber.Length != 10)
                {
                    errorObject = new { message = "Phone number must be 10 digits" };
                    errorJson = JsonConvert.SerializeObject(errorObject);
                    return StatusCode(400, errorJson);
                }


                // Console.WriteLine($"Incorrect phone number or password {userDTOLogin.MobileNumber} - {userDTOLogin.MobileNumber}");
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
                    var result = await response.Content.ReadAsStringAsync();
                    var responseData = JsonConvert.DeserializeObject<dynamic>(result);

                    // Lấy token từ dữ liệu trả về
                    string token = responseData?.token;

                    // Lấy thông tin người dùng từ dữ liệu trả về
                    User _user = responseData.user.ToObject<User>();

                    if (token != null && _user != null)
                    {
                        var obj = new
                        {
                            status = 1,
                            message = $"Login sucsses, welcome back",
                            token
                        };
                        HttpContext.Session.SetString("User", JsonConvert.SerializeObject(_user));
                        HttpContext.Session.SetString("Token", token);

                        var json = JsonConvert.SerializeObject(obj);
                        return StatusCode(200, json);
                    }

                }

                errorObject = new { message = $"Incorrect phone number or password" };
                errorJson = JsonConvert.SerializeObject(response);
                return StatusCode(401, errorJson);
            }
            catch (System.Exception e)
            {

                var errorObject = new { message = "Something went wrong", error = e.Message };
                var errorJson = JsonConvert.SerializeObject(errorObject);
                return StatusCode(500, errorJson); // Trả về mã lỗi 500 Internal Server Error với thông báo JSON tùy chỉnh
            }

        }

        [HttpGet("Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserLoginDTO userDTOLogin)
        {
            try
            {
                Object errorObject;
                var errorJson = "";
                //check null
                if (userDTOLogin.MobileNumber == null || userDTOLogin.Password == null)
                {
                    errorObject = new { message = "Please enter phone number and password" };
                    errorJson = JsonConvert.SerializeObject(errorObject);
                    return StatusCode(400, errorJson);
                }
                //check phone number is number
                if (!int.TryParse(userDTOLogin.MobileNumber, out int n))
                {
                    errorObject = new { message = "Phone number must be number" };
                    errorJson = JsonConvert.SerializeObject(errorObject);
                    return StatusCode(400, errorJson);
                }
                //check 10 digit phone number
                if (userDTOLogin.MobileNumber.Length != 10)
                {
                    errorObject = new { message = "Phone number must be 10 digits" };
                    errorJson = JsonConvert.SerializeObject(errorObject);
                    return StatusCode(400, errorJson);
                }

                var response = client.PostAsJsonAsync<UserLoginDTO>($"{url_user}/Register", userDTOLogin).Result;

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var responseData = JsonConvert.DeserializeObject<dynamic>(result);

                    // Lấy token từ dữ liệu trả về
                    string token = responseData?.token;

                    // Lấy thông tin người dùng từ dữ liệu trả về
                    User _user = responseData.user.ToObject<User>();

                    if (token != null && _user != null)
                    {
                        var obj = new
                        {
                            status = 1,
                            message = $"Register sucsses, welcome to our service",
                            token
                        };
                        HttpContext.Session.SetString("User", JsonConvert.SerializeObject(_user));
                        HttpContext.Session.SetString("Token", token);

                        var json = JsonConvert.SerializeObject(obj);
                        return StatusCode(200, json);
                    }

                }
                //check 409
                if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                {
                    errorObject = new { message = $"User already exists" };
                    errorJson = JsonConvert.SerializeObject(errorObject);
                    return StatusCode(409, errorJson);
                }

                errorObject = new { message = $"Something went wrong" };
                errorJson = JsonConvert.SerializeObject(response);
                return StatusCode(500, errorJson);
            }
            catch (System.Exception)
            {

                var errorObject = new { message = $"Something went wrong" };
                var errorJson = JsonConvert.SerializeObject(errorObject);
                return StatusCode(500, errorJson);
            }

        }

        //logout
        [HttpGet("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("User");
            HttpContext.Session.Remove("Token");
            return RedirectToAction("Login");
        }



    }
}
