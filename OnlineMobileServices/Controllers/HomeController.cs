using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using OnlineMobileServices.Models;
using OnlineMobileServices_Models.DTOs;
using OnlineMobileServices_Models.Models;
using PayPal.Api;
using System.Diagnostics;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
namespace OnlineMobileServices_FE.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        HttpClient _client = new HttpClient();
        private string url = $"{Program.API_URL}/";
        private string url_user = $"{Program.API_URL}/User";

        private PayPal.Api.Payment _payment;

        private IHttpContextAccessor _httpContextAccessor;

        IConfiguration _configuration;

        private readonly IMemoryCache _memoryCache;


        public HomeController(ILogger<HomeController> logger, IMemoryCache memoryCache, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _logger = logger;
            _memoryCache = memoryCache;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;

        }
        [HttpGet]
        public IActionResult Index()
        {
            if (!_memoryCache.TryGetValue("TelcoData", out IEnumerable<Telco>? telcoData))
            {
                telcoData = _client.GetFromJsonAsync<IEnumerable<Telco>>($"{url}Telco").Result ?? new List<Telco>();
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


                var response = _client.PostAsJsonAsync<UserLoginDTO>($"{url_user}/Login", userDTOLogin).Result;

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

                var response = _client.PostAsJsonAsync<UserLoginDTO>($"{url_user}/Register", userDTOLogin).Result;

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
        public async Task<IActionResult> PaymentWithPaypal(string Cancel = "", string Service = "", int BillID = -1, string guid = "", string PayerID = "")
        {
            //getting the apiContext[^1^][1]
            var ClientID = _configuration.GetSection("PayPal:Key").Value;
            var ClientSecret = _configuration.GetSection("PayPal:Secret").Value;
            var mode = _configuration.GetSection("PayPal:Mode").Value;

            if (ClientID == null || ClientSecret == null || mode == null)
            {
                return StatusCode(500, "Paypal is not configured");
            }
            if (Cancel == "true")
            {
                return View("PaymentFail");
            }

            APIContext apiContext = PaypalConfiguration.GetAPIContext(ClientID, ClientSecret, mode);

            try
            {
                //http://localhost:8000/PaymentWithPayPal?guid=83401&paymentId=PAYID-MXVNFUQ3HP99311454364311&token=EC-24X34879A13415046&PayerID=VC9G2S4KHKM8U
                string payerId = PayerID;
                if (string.IsNullOrEmpty(payerId))
                {

                    String[] Services = { "Recharge", "SpecialRecharge", "DoNotDisturb", "CallerTunes", "PostPaid" };
                    if (!Services.Contains(Service) || BillID == -1)
                    {
                        Console.WriteLine("Service not found");
                        return NotFound("Service not found");
                    }
                    var fullEndPoint = $"{Program.API_URL}/History/{Service}/{BillID}";
                    var response = await _client.GetAsync(fullEndPoint);
                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("GetFromJsonAsync.Bill not found");
                        return NotFound("GetFromJsonAsync.Bill not found");
                    }
                    var result = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("result: " + result);
                    // result{"historyID":3,"mobileNumber":"1111111111","userID":null,"user":null,"packageID":9,"originalPackage":null,"date":"2024-03-08T13:52:44.2050281","amount":123,"paymentMethod":"Payal","status":"Pending"}
                    var responseData = JsonConvert.DeserializeObject<dynamic>(result);
                    Console.WriteLine("responseData: " + responseData);
                    // var responseData = JsonConvert.DeserializeObject<dynamic>(result);
                    // // int _BillID = responseData.HistoryID;
                    // // double amount = responseData.Amount;
                    int _BillID = responseData?.historyID;
                    double amount = responseData?.amount;
                    if (_BillID != BillID || amount == -1)
                    {
                        Console.WriteLine("Bill not found 2");
                        return NotFound("Bill not found 2");
                    }
                    HttpContext.Session.SetString("PackageNameHistory", Service);
                    HttpContext.Session.SetString("BillID", _BillID.ToString());

                    string baseURI = this.Request.Scheme + "://" + this.Request.Host + "/PaymentWithPayPal?";
                    var guidd = Convert.ToString((new Random()).Next(100000));
                    guid = guidd;
                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid, payerId, amount);
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

                    _httpContextAccessor.HttpContext.Session.SetString("payment", createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    var paymentId = _httpContextAccessor.HttpContext.Session.GetString("payment");
                    if (paymentId == null)
                    {
                        return NotFound("Payment not found");
                    }
                    var executedPayment = ExecutePayment(apiContext, payerId, paymentId as string);
                    if (executedPayment == null)
                    {
                        // Liểm tra xem còn seesion của PackageNameHistory và BillID không\
                        string PackageNameHistory = HttpContext.Session.GetString("PackageNameHistory") ?? String.Empty;
                        int _BillID = int.Parse(HttpContext.Session.GetString("BillID") ?? "-1");
                        if (PackageNameHistory == String.Empty || _BillID == -1)
                        {
                            //redic to home
                            return RedirectToAction("Index", "Home");
                        }
                        //redic to fail
                        return RedirectToAction("PaymentFail");
                    }

                    if (executedPayment.state.ToLower() != "approved")
                    {
                        //
                        return RedirectToAction("PaymentFail");
                    }
                    else
                    {
                        //redic to success
                        return RedirectToAction("PaymentSuccess");
                    }
                }
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex);
            }
        }

        //pay ok
        [HttpGet("PaymentSuccess")]
        public async Task<IActionResult> PaymentSuccess()
        {
            //get package name history
            string PackageNameHistory = HttpContext.Session.GetString("PackageNameHistory") ?? "Recharge";
            int BillID = int.Parse(HttpContext.Session.GetString("BillID") ?? "3");
            if (PackageNameHistory == String.Empty || BillID == -1)
            {
                Console.WriteLine("PackageNameHistory or BillID not found");
                return NotFound();
            }
            var fullEndPoint = $"{Program.API_URL}/History/{PackageNameHistory}/{BillID}";
            var response = await _client.GetAsync(fullEndPoint);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var packageList = JsonConvert.DeserializeObject<dynamic>(result);
                //clear session
                // HttpContext.Session.Remove("PackageNameHistory");
                // HttpContext.Session.Remove("BillID");
                var responsePut = await _client.PutAsJsonAsync($"{fullEndPoint}?status=Success", "Success");
                if (!responsePut.IsSuccessStatusCode)
                {
                    // return NotFound("Update status fail");

                }
                return View(packageList);
            }
            else
            {
                Console.WriteLine("PaymentSuccess.Package not found");
                return View();
            }


        }

        //pay fail
        [HttpGet("PaymentFail")]
        public async Task<IActionResult> PaymentFail()
        {

            //get package name history
            string PackageNameHistory = HttpContext.Session.GetString("PackageNameHistory") ?? "Recharge";
            int BillID = int.Parse(HttpContext.Session.GetString("BillID") ?? "3");
            if (PackageNameHistory == String.Empty || BillID == -1)
            {
                return NotFound();
            }
            var fullEndPoint = $"{Program.API_URL}/History/{PackageNameHistory}/{BillID}";
            var response = await _client.GetAsync(fullEndPoint);
            if (response.IsSuccessStatusCode)
            {

                var result = await response.Content.ReadAsStringAsync();
                var history = JsonConvert.DeserializeObject<dynamic>(result);
                // //update status of RechargeHistory
                // [HttpPut("Recharge/{id}")]
                // public async Task<IActionResult> PutRechargeHistory(int id, String status)
                // http://localhost:8001/api/History/Recharge/3?status=ok
                var responsePut = await _client.PutAsJsonAsync($"{fullEndPoint}?status=Fail", "Fail");
                if (!responsePut.IsSuccessStatusCode)
                {
                    // return NotFound("Update status fail");

                }








                //clear session
                // HttpContext.Session.Remove("PackageNameHistory");
                // HttpContext.Session.Remove("BillID");
                return View(history);
            }
            else
            {
                return NotFound("PaymentSuccess.Package not found");
            }

        }



        //o	About Us
        [HttpGet("About")]
        public IActionResult About()
        {
            return View();
        }

        //o	Contact Us
        [HttpGet("Contact")]
        public IActionResult Contact()
        {
            return View();
        }

        //o	Customer Care
        [HttpGet("CustomerCare")]
        public IActionResult CustomerCare()
        {
            return View();
        }








        private Payment CreatePayment(APIContext apiContext, string redirectUrl, string blogId, double _amount)
        {
            try
            {
                // Tạo danh sách item và thêm các đối tượng item vào
                var itemList = new ItemList() { items = new List<Item>() };
                // Thêm chi tiết Item như tên, tiền tệ, giá, v.v.
                itemList.items.Add(new Item()
                {
                    name = "Item Detail",
                    currency = "USD",
                    price = _amount.ToString(),
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
                    total = _amount.ToString(), // Tổng số tiền
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

                this._payment = new Payment()
                {
                    intent = "sale",
                    payer = payer,
                    transactions = transactionList,
                    redirect_urls = redirUrls
                };

                // Tạo một thanh toán sử dụng APIContext
                return this._payment.Create(apiContext);
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e);
                throw e;
            }
        }

        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            if (paymentId == null || payerId == null)
            {
                return null;
            }
            try
            {
                var paymentExecution = new PaymentExecution() { payer_id = payerId };
                this._payment = new Payment() { id = paymentId };
                return this._payment.Execute(apiContext, paymentExecution);
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e);
                return null;
            }

        }
        [HttpGet("DownloadInvoice")]
        public async Task<FileResult> DownloadInvoice()
        {
            string PackageNameHistory = HttpContext.Session.GetString("PackageNameHistory") ?? "Recharge";
            int BillID = int.Parse(HttpContext.Session.GetString("BillID") ?? "3");
            if (PackageNameHistory == String.Empty || BillID == -1)
            {
                //redic to home
                return File(Encoding.UTF8.GetBytes("PackageNameHistory or BillID not found"), "text/plain", "PackageNameHistory or BillID not found.txt");
            }
            var fullEndPoint = $"{Program.API_URL}/History/{PackageNameHistory}/{BillID}";
            var response = await _client.GetAsync(fullEndPoint);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var packageList = JsonConvert.DeserializeObject<dynamic>(result);
                var pdfBytes = CreatePdf(packageList);

                if (packageList != null)
                {
                    return File(pdfBytes, "application/pdf", $"Invoice-{packageList.historyID}.pdf");
                }
                else
                {
                    return File(Encoding.UTF8.GetBytes("Package not found"), "text/plain", "Package not found.txt");
                }
            }
            else
            {
                return File(Encoding.UTF8.GetBytes("Package not found"), "text/plain", "Package not found.txt");
            }
        }
        private byte[] CreatePdf(dynamic model)
        {
            Console.WriteLine("model: " + model);
            using (var ms = new MemoryStream())
            {
                // Document setup
                var document = new Document(PageSize.A4, 50, 50, 25, 25);
                var writer = PdfWriter.GetInstance(document, ms);
                document.Open();

                // Fonts
                var normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);
                var boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);


                // Image logo = Image.GetInstance("path/to/your/logo.png");
                // logo.ScaleToFit(50f, 50f); 
                // document.Add(logo);

                // Use a suitable font for the title
                var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);

                // Add Invoice Header
                var title = new Paragraph("Invoice", titleFont);
                title.Alignment = Element.ALIGN_CENTER;
                document.Add(title);
                //add khoảng cách
                document.Add(new Paragraph(" "));
                // Invoice Details Table
                var table = new PdfPTable(2);
                table.WidthPercentage = 90;
                table.HorizontalAlignment = Element.ALIGN_CENTER;

                table.AddCell(new Phrase("Description", boldFont));
                table.AddCell(new Phrase("Value", boldFont));

                table.AddCell("Invoice ID");
                table.AddCell(model.historyID.ToString());
                table.AddCell("Date");
                table.AddCell(model.date.ToString("dd/MM/yyyy"));
                table.AddCell("Mobile Number");
                table.AddCell(model?.mobileNumber.ToString());
                table.AddCell("Package");
                table.AddCell(model?.packageID.ToString());
                table.AddCell("Amount");
                table.AddCell($"{model?.amount.ToString()}$");
                table.AddCell("Payment Method");
                table.AddCell(model?.paymentMethod.ToString());
                table.AddCell("Status");
                table.AddCell(model?.status.ToString());

                document.Add(table);

                var currentDate = DateTime.Now;
                var footerPhrase = new Phrase($"Printed on: {currentDate.ToString("dd/MM/yyyy HH:mm")}", normalFont);
                var footer = new Paragraph(footerPhrase);
                footer.Alignment = Element.ALIGN_RIGHT;
                document.Add(footer);

                // Close the document
                document.Close();

                return ms.ToArray();
            }
        }


    }
}
