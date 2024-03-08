using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using OnlineMobileServices.Models;
using OnlineMobileServices_Models.DTOs;
using OnlineMobileServices_Models.Models;
using PayPal.Api;
using System.Diagnostics;
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

        private IHttpContextAccessor httpContextAccessor;

        IConfiguration _configuration;

        private readonly IMemoryCache _memoryCache;


        public HomeController(ILogger<HomeController> logger, IMemoryCache memoryCache, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _logger = logger;
            _memoryCache = memoryCache;
            this.httpContextAccessor = httpContextAccessor;
            _configuration = configuration;

        }
        [HttpGet]
        public IActionResult Index()
        {
            if (!_memoryCache.TryGetValue("TelcoData", out IEnumerable<Telco>? telcoData))
            {
                telcoData = client.GetFromJsonAsync<IEnumerable<Telco>>($"{url}Telco").Result ?? new List<Telco>();
                Console.WriteLine("Get data from API");
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5)); // Adjust as needed

                _memoryCache.Set("TelcoData", telcoData, cacheEntryOptions);
            }
            ViewBag.Telco = telcoData;
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
         [HttpGet("PaymentWithPaypal")]
        public ActionResult PaymentWithPaypal(string Cancel = null, string blogId = "", string PayerID = "", string guid = "")
        {
            //getting the apiContext[^1^][1]
            var ClientID = _configuration.GetSection("PayPal:Key").Value;
            var ClientSecret = _configuration.GetSection("PayPal:Secret").Value;
            var mode = _configuration.GetSection("PayPal:Mode").Value;
            if(ClientID == null || ClientSecret == null || mode == null)
            {
                return Ok(new { status = 0, message = "Paypal is not configured" });
            }
            if(Cancel == "true")
            {
                return Ok(new { status = 0, message = "Payment cancelled" });
            }
            APIContext apiContext = PaypalConfiguration.GetAPIContext(ClientID, ClientSecret, mode);

            try
            {
                string payerId = PayerID;
                if (string.IsNullOrEmpty(payerId))
                {
                    string baseURI = this.Request.Scheme + "://" + this.Request.Host + "/PaymentWithPayPal?";
                    var guidd = Convert.ToString((new Random()).Next(100000));
                    guid = guidd;
                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid, blogId);
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;


                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            paypalRedirectUrl = lnk.href;
                        }
                    }

                    httpContextAccessor.HttpContext.Session.SetString("payment", createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    var paymentId = httpContextAccessor.HttpContext.Session.GetString("payment");
                    var executedPayment = ExecutePayment(apiContext, payerId, paymentId as string);


                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return Ok(new { status = 0, message = "Payment failed" });
                    }

                    return Ok(new { status = 1, message = "Payment success" });
                }
            }
            catch (Exception ex)
            {
                return Ok(new { status = 0, message = ex.Message });
            }
        }
        private PayPal.Api.Payment payment;
        private Payment CreatePayment(APIContext apiContext, string redirectUrl, string blogId)
        {
            // Tạo danh sách item và thêm các đối tượng item vào
            var itemList = new ItemList() { items = new List<Item>() };
            // Thêm chi tiết Item như tên, tiền tệ, giá, v.v.
            itemList.items.Add(new Item()
            {
                name = "Item Detail",
                currency = "USD",
                price = "1.00",
                quantity = "1",
                sku = "asd"
            });

            var payer = new Payer() { payment_method = "paypal" };
            // Cấu hình Redirect Urls với đối tượng RedirectUrls
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };

            // Thêm chi tiết Thuế, vận chuyển và Tổng phụ
            var amount = new Amount()
            {
                currency = "USD",
                total = "1.00" // Tổng cộng phải bằng tổng của thuế, vận chuyển và tổng phụ
            };

            var transactionList = new List<Transaction>();
            // Thêm mô tả về giao dịch
            transactionList.Add(new Transaction()
            {
                description = "Transaction description",
                invoice_number = Guid.NewGuid().ToString(), // Tạo một Số Hóa Đơn
                amount = amount,
                item_list = itemList
            });

            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };

            // Tạo một thanh toán sử dụng APIContext
            return this.payment.Create(apiContext);
        }

        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution() { payer_id = payerId };
            this.payment = new Payment() { id = paymentId };
            return this.payment.Execute(apiContext, paymentExecution);
        }




    }
}
