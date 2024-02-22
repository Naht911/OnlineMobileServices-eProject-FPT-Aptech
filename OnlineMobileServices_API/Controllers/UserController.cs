using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OnlineMobileServices_API.Models;
using OnlineMobileServices_Models.Models;
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
        public async Task<ActionResult<User>> Login(string MobileNumber, string password)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.MobileNumber == MobileNumber);

                if (user == null)
                {
                    return Unauthorized(); // Trả về mã lỗi 404 Not Found nếu không tìm thấy người dùng
                }

                string hashedPassword = _userService.HashPassword(password);

                // Kiểm tra xem mật khẩu đã hash có khớp với mật khẩu gốc không
                bool isMatch = user.Password.Equals(hashedPassword);
                if (isMatch)
                {
                    return Ok(user); // Trả về mã thành công 200 và thông tin người dùng nếu mật khẩu khớp
                }

                return Unauthorized(); // Trả về mã lỗi 401 Unauthorized nếu mật khẩu không khớp
            }
            catch (Exception e)
            {
                var errorObject = new { message = "Something went wrong" };
                var errorJson = JsonConvert.SerializeObject(errorObject);
                return StatusCode(500, errorJson); // Trả về mã lỗi 500 Internal Server Error với thông báo JSON tùy chỉnh
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(User newUser)
        {
            try
            {
                // Kiểm tra xem người dùng đã tồn tại trong cơ sở dữ liệu chưa
                var existingUser = await _context.Users.FirstOrDefaultAsync(x => x.MobileNumber == newUser.MobileNumber);
                if (existingUser != null)
                {
                    return Conflict(); // Trả về mã lỗi 409 Conflict nếu người dùng đã tồn tại
                }

                // Hash mật khẩu trước khi lưu vào cơ sở dữ liệu
                newUser.Password = _userService.HashPassword(newUser.Password);

                // Thêm người dùng mới vào cơ sở dữ liệu
                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                // Trả về mã thành công 201 Created và thông tin người dùng mới
                return CreatedAtAction(nameof(GetUserById), new { id = newUser.UserID }, newUser);
            }
            catch (Exception e)
            {
                // Trả về mã lỗi 500 Internal Server Error nếu có lỗi xảy ra trong quá trình xử lý
                var errorObject = new { message = "Something went wrong" };
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
            catch (Exception e)
            {
                // Trả về mã lỗi 500 Internal Server Error nếu có lỗi xảy ra trong quá trình xử lý
                var errorObject = new { message = "Something went wrong" };
                var errorJson = JsonConvert.SerializeObject(errorObject);
                return StatusCode(500, errorJson);
            }
        }


    }
}
