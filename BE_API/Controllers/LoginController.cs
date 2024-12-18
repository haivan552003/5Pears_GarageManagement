using BE_API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System;
using BE_API.ModelCustom;
using Dapper;

namespace BE_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        public IConfiguration Configuration { get; set; }
        public readonly AppDbContext _context;

        public LoginController(IConfiguration configuration, AppDbContext context)
        {
            Configuration = configuration;
            _context = context;
        }

        [HttpPost("gentoken-user")]
        public async Task<IActionResult> GenerateTokenUser(login user)
        {
            if (user == null || string.IsNullOrEmpty(user.email) || string.IsNullOrEmpty(user.password))
            {
                return BadRequest("Invalid request.");
            }

            var userData = await GetUserInfor(user.email, user.password);

            if (userData == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            var jwt = Configuration.GetSection("Jwt").Get<Jwt>();

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim("email", userData.email),
                new Claim("id", userData.id.ToString()),
                new Claim("fullname", userData.fullname),
                new Claim(ClaimTypes.Role, userData.role_id.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                jwt.Issuer,
                jwt.Audience,
                claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: signIn
            );

            return Ok(new JwtSecurityTokenHandler().WriteToken(token));
        }

        [HttpPost("gentoken-admin")]
        public async Task<IActionResult> GenerateTokenAdmin(login user)
        {
            if (user == null || string.IsNullOrEmpty(user.email) || string.IsNullOrEmpty(user.password))
            {
                return BadRequest("Invalid request.");
            }

            var userData = await GetAdminInfor(user.email, user.password);

            if (userData == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            var jwt = Configuration.GetSection("Jwt").Get<Jwt>();

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim("email", userData.email),
                new Claim("emp_id", userData.id.ToString()),
                new Claim("fullname", userData.fullname),
                new Claim(ClaimTypes.Role, userData.role_id.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                jwt.Issuer,
                jwt.Audience,
                claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: signIn
            );

            return Ok(new JwtSecurityTokenHandler().WriteToken(token));
        }
        [HttpPost("gentoken-driver")]
        public async Task<IActionResult> GenerateTokenDriver(login user)
        {
            if (user == null || string.IsNullOrEmpty(user.email) || string.IsNullOrEmpty(user.password))
            {
                return BadRequest("Invalid request.");
            }

            var userData = await GetDriverInfor(user.email, user.password);

            if (userData == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            var jwt = Configuration.GetSection("Jwt").Get<Jwt>();

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim("email", userData.email),
                new Claim("emp_id", userData.id.ToString()),
                new Claim("fullname", userData.fullname),
                new Claim(ClaimTypes.Role, userData.role_id.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                jwt.Issuer,
                jwt.Audience,
                claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: signIn
            );

            return Ok(new JwtSecurityTokenHandler().WriteToken(token));
        }

        [HttpGet("user-login")]
        public async Task<login> GetUserInfor(string email, string password)
        {
            using (IDbConnection db = new SqlConnection(Configuration.GetConnectionString("SqlConnection")))
            {
                var parameters = new { Email = email, Password = password };
                var user = await db.QueryFirstOrDefaultAsync<login>(
                    "sp_user_login",
                    parameters,
                    commandType: CommandType.StoredProcedure);
                if (user != null && BCrypt.Net.BCrypt.Verify(password, user.password))
                {
                    user.password = null;

                    return user;

                }

                return null;
            }
        }

        //kiểm tra người dùng 
        [HttpGet("admin-login")]
        public async Task<login> GetAdminInfor(string email, string password)
        {
            using (IDbConnection db = new SqlConnection(Configuration.GetConnectionString("SqlConnection")))
            {
                var parameters = new { Email = email, Password = password };
                var user = await db.QueryFirstOrDefaultAsync<login>(
                    "sp_admin_login",
                    parameters,
                    commandType: CommandType.StoredProcedure);
                if (user != null && BCrypt.Net.BCrypt.Verify(password, user.password))
                {
                    user.password = null;
                    return user;

                }
                return user;
            }
        }
        [HttpGet("admin-login-driver")]
        public async Task<login> GetDriverInfor(string email, string password)
        {
            using (IDbConnection db = new SqlConnection(Configuration.GetConnectionString("SqlConnection")))
            {
                var parameters = new { Email = email, Password = password };
                var user = await db.QueryFirstOrDefaultAsync<login>(
                    "sp_admin_login_driver",
                    parameters,
                    commandType: CommandType.StoredProcedure);
                if (user != null && BCrypt.Net.BCrypt.Verify(password, user.password))
                {
                    user.password = null;
                    return user;

                }
                return user;
            }
        }

        [HttpGet("user-login_phone")]
        public async Task<login_phone> GetUserPhone(string phone, string password)
        {
            using (IDbConnection db = new SqlConnection(Configuration.GetConnectionString("SqlConnection")))
            {
                var parameters = new { Phone = phone, Password = password };
                var user = await db.QueryFirstOrDefaultAsync<login_phone>(
                    "sp_user_login_phone",
                    parameters,
                    commandType: CommandType.StoredProcedure);
                //if (user != null && BCrypt.Net.BCrypt.Verify(password, user.password))
                if (user.password == password)
                {
                    user.password = null;
                    return user;

                }
                return user;
            }
        }

        [HttpPost("gentoken-user_phone")]
        public async Task<IActionResult> GenerateTokenPhone(login_phone user)
        {
            if (user == null || string.IsNullOrEmpty(user.phone_number) || string.IsNullOrEmpty(user.password))
            {
                return BadRequest("Invalid request.");
            }

            var userDataPhone = await GetUserPhone(user.phone_number, user.password);

            if (userDataPhone == null)
            {
                return Unauthorized("User not found or password incorrect.");
            }

            var jwt = Configuration.GetSection("Jwt").Get<Jwt>();

            var claims = new[] {
        new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
        new Claim("phone", userDataPhone.phone_number),
        new Claim("id", userDataPhone.id.ToString()),
        new Claim("fullname", userDataPhone.fullname),
        new Claim(ClaimTypes.Role, userDataPhone.role_id.ToString())
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                jwt.Issuer,
                jwt.Audience,
                claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: signIn
            );

            return Ok(new JwtSecurityTokenHandler().WriteToken(token));
        }



        //[HttpPost("user_login_phone")]
        //public async Task<IActionResult> Login([FromBody] login_phone_user request)

        //{
        //    if (string.IsNullOrEmpty(request.PhoneNumber) || string.IsNullOrEmpty(request.Password))
        //    {
        //        return BadRequest(new { Message = "Số điện thoại và mật khẩu không được để trống." });
        //    }

        //    using (IDbConnection db = new SqlConnection(Configuration.GetConnectionString("SqlConnection")))
        //    {
        //        var parameters = new DynamicParameters();
        //        parameters.Add("@phone_number", request.PhoneNumber);
        //        parameters.Add("@password", request.Password);

        //        var user = await db.QueryFirstOrDefaultAsync<LoginResponseDto>(
        //            "sp_login_phone_number",
        //            parameters,
        //            commandType: CommandType.StoredProcedure);

        //        if (user != null)
        //        {
        //            return Ok(new LoginResponseDto
        //            {
        //                PhoneNumber = user.PhoneNumber,
        //                fullname = user.fullname,
        //                Status = user.Status,
        //                Message = "Đăng nhập thành công."
        //            });
        //        }
        //        else
        //        {
        //            return Unauthorized(new { Message = "Số điện thoại hoặc mật khẩu không đúng." });
        //        }
        //    }
        //}

        //[HttpPost("user_login_phone")]
        //public async Task<IActionResult> Login([FromBody] login_phone request)

        //{
        //    if (string.IsNullOrEmpty(request.PhoneNumber) || string.IsNullOrEmpty(request.Password))
        //    {
        //        return BadRequest(new { Message = "Số điện thoại và mật khẩu không được để trống." });
        //    }

        //    using (IDbConnection db = new SqlConnection(Configuration.GetConnectionString("SqlConnection")))
        //    {
        //        var parameters = new DynamicParameters();
        //        parameters.Add("@phone_number", request.PhoneNumber);
        //        parameters.Add("@password", request.Password);

        //        var user = await db.QueryFirstOrDefaultAsync<customers>(
        //            "sp_login_phone_number",
        //            parameters,
        //            commandType: CommandType.StoredProcedure);

        //        //if (user != null)
        //        //{
        //        //    //var token = GenerateTokenPhone(user);
        //        //    //return Ok(token);
        //        //    //    PhoneNumber = user.phone_number,
        //        //    //    FullName = user.fullname,
        //        //    //    Status = user.status,
        //        //    //    Message = "Đăng nhập thành công."
        //        //}
        //        if (user != null)
        //        {
        //            return Ok(new LoginResponseDto
        //            {
        //                PhoneNumber = user.phone_number,
        //                FullName = user.fullname,
        //                Status = user.status,
        //                Message = "Đăng nhập thành công."
        //            });
        //            //var token = GenerateTokenPhone();
        //            //return Ok(token);
        //        }
        //        else
        //        {
        //            return Unauthorized(new { Message = "Số điện thoại hoặc mật khẩu không đúng." });
        //        }
        //    }
        //}
    }
}
