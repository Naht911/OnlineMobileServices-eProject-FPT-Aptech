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

        private static readonly String _jwtSecret = "03140c405e133408a70ceac8f263058feceecc29d0b5d079238637be5dd2879cf";

        public static string GenerateToken(User user)
        {

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(GetJwtSecret());
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

        //Verify token
        public ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSecret);

            try
            {
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = true,
                    ValidateLifetime = true
                };

                SecurityToken securityToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out securityToken);

                if (!(securityToken is JwtSecurityToken jwtSecurityToken) ||
                    !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                    throw new SecurityTokenException("Invalid token");

                return principal;
            }
            catch
            {
                return null;
            }
        }

        //get user id from token
        public int GetUserIdFromToken(string token)
        {
            var principal = GetPrincipalFromToken(token);
            if (principal == null)
                return -1;
            var user_id = principal.FindFirst("User_id")?.Value ?? "-1";
            return int.Parse(user_id);
        }

        public bool IsTokenValid(string token)
        {
            var principal = GetPrincipalFromToken(token);

            if (principal == null)
                return false;

            return true;
        }


        public static string GetJwtSecret()
        {
            return _jwtSecret;
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
