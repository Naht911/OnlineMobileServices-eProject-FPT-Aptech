using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OnlineMobileServices_Models.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace OnlineMobileServices_Models.Services
{
    public class UserService
    {

        private static readonly String Jwt_Secret = "03140c405e133408a70ceac8f263058feceecc29d0b5d079238637be5dd2879cf";

        public static string GenerateToken(User user)
        {
            // 4. Generate JWT token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(UserService.GetJwtSecret());
            //Lưu thông tin user vào token
            var User_id = new Claim("User_id", user.UserID.ToString());
            // var MobileNumber = new Claim("MobileNumber", user.MobileNumber);
            var Role = new Claim("Role", user.Role);
            // var Email = new Claim("Email", user.Email);
            // var FirstName = new Claim("FirstName", user.FirstName);
            // var LastName = new Claim("LastName", user.LastName);
            // var Address = new Claim("Address", user.Address);
            // var claims = new Claim[] { User_id, MobileNumber, Role, Email, FirstName, LastName, Address };
            var claims = new Claim[] { User_id, Role };
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(30), // Set token expiration time
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };


            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }

        public static string GetJwtSecret()
        {
            return Jwt_Secret;
        }
        public string HashPassword(string password)
        {

            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Hash mật khẩu
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Chuyển đổi byte array thành một chuỗi hex
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        // Hàm này kiểm tra xem mật khẩu đã hash có khớp với mật khẩu gốc không
        public bool VerifyPassword(string password, string hashedPassword)
        {
            // Hash lại mật khẩu đầu vào
            string hashOfInput = HashPassword(password);

            // So sánh hash mật khẩu đầu vào với hash mật khẩu đã được lưu trữ
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            return comparer.Compare(hashOfInput, hashedPassword) == 0;
        }
    }
}
