using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OnlineMobileServices_API.Models;
using OnlineMobileServices_Models.Models;

using OnlineMobileServices_Models.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using OnlineMobileServices_Models.Services;
namespace OnlineMobileServices_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly UserService _userService;
        private readonly DatabaseContext _context;

        public UserController(DatabaseContext context, UserService userService)
        {
            _context = context;
            _userService = userService;

        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDTO userDTOLogin)
        {
            try
            {
                object errorObject;
                var errorJson = "";
                // 1. Find user by mobile number
                var user = await _context.Users.FirstOrDefaultAsync(x => x.MobileNumber == userDTOLogin.MobileNumber);

                // 2. Check if user exists
                if (user == null)
                {
                    errorObject = new { message = $"Incorrect phone number or password" };
                    errorJson = JsonConvert.SerializeObject(errorObject);
                    return StatusCode(401, errorJson);
                }

                // 3. Validate password
                string hashedPassword = _userService.HashPassword(userDTOLogin.Password);
                if (!user.Password.Equals(hashedPassword))
                {
                    errorObject = new { message = $"Incorrect phone number or password" };
                    errorJson = JsonConvert.SerializeObject(errorObject);
                    return StatusCode(401, errorJson);
                }


                var tokenString = UserService.GenerateToken(user);


                // 5. Return user data and token
                return Ok(new { user, token = tokenString });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); // Log the error
                return StatusCode(500, "Something went wrong, please try again later");
            }
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(UserLoginDTO newUser)
        {
            try
            {
                var errorObject = new { message = "Something went wrong" };
                var errorJson = JsonConvert.SerializeObject(errorObject);
                //check 10 digit phone number
                if (newUser.MobileNumber.Length != 10)
                {
                    errorObject = new { message = "Phone number must be 10 digits" };
                    errorJson = JsonConvert.SerializeObject(errorObject);
                    return StatusCode(400, errorJson);
                }
                // Kiểm tra xem người dùng đã tồn tại trong cơ sở dữ liệu chưa
                var existingUser = await _context.Users.FirstOrDefaultAsync(x => x.MobileNumber == newUser.MobileNumber);
                if (existingUser != null)
                {
                    // Trả về mã lỗi 400 Bad Request nếu người dùng đã tồn tại
                    errorObject = new { message = "User already exists." };
                    errorJson = JsonConvert.SerializeObject(errorObject);
                    return StatusCode(409, errorJson);
                }

                // Hash mật khẩu trước khi lưu vào cơ sở dữ liệu
                newUser.Password = _userService.HashPassword(newUser.Password);
                //tạo user mới
                User user = new User
                {
                    MobileNumber = newUser.MobileNumber,
                    Password = newUser.Password,
                    Role = "user",
                    RegisterDate = DateTime.Now,
                    Email = "user@user"
                };

                // Thêm người dùng mới vào cơ sở dữ liệu
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                var tokenString = UserService.GenerateToken(user);


                // 5. Return user data and token
                return StatusCode(201, new { user, token = tokenString });
            }
            catch (Exception e)
            {
                // Trả về mã lỗi 500 Internal Server Error nếu có lỗi xảy ra trong quá trình xử lý
                var errorObject = new { message = "Something went wrong", error = e.Message };
                var errorJson = JsonConvert.SerializeObject(errorObject);
                return StatusCode(500, errorJson);
            }
        }

        [HttpGet("{id}")] //chỉ dùng nội bộ
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);

                if (user == null)
                {
                    return NotFound(); // Trả về mã lỗi 404 Not Found nếu không tìm thấy người dùng với ID cung cấp
                }

                return Ok(user); // Trả về mã thành công 200 và thông tin người dùng nếu tìm thấy
            }
            catch (Exception)
            {
                // Trả về mã lỗi 500 Internal Server Error nếu có lỗi xảy ra trong quá trình xử lý
                var errorObject = new { message = "Something went wrong" };
                var errorJson = JsonConvert.SerializeObject(errorObject);
                return StatusCode(500, errorJson);
            }
        }

        [HttpGet("token")]
        public async Task<ActionResult<User>> GetUserByToken(string token)
        {
            try
            {
                var uid = _userService.GetUserIdFromToken(token);
                if (uid == -1)
                {
                    return Unauthorized();
                }
                var user = await _context.Users.FirstOrDefaultAsync(x => x.UserID == uid);

                if (user == null)
                {
                    return Unauthorized();
                }
                // Console.WriteLine("Authorized");
                return Ok(user);
            }
            catch (Exception)
            {
                var errorObject = new { message = "Something went wrong" };
                var errorJson = JsonConvert.SerializeObject(errorObject);
                return StatusCode(500, errorJson);
            }
        }






    }
}
