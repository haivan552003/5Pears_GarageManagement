﻿using BE_API.Data;
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

        //tạo token
        [HttpPost("gentoken-user")]
        public async Task<IActionResult> GenerateTokenUser(login user)
        {
            if (user != null && !string.IsNullOrEmpty(user.email) && !string.IsNullOrEmpty(user.password))
            {
                //lấy dữ liệu nhập vào 
                var userData = await GetUserInfor(user.email, user.password);

                if (userData == null)
                {
                    return Unauthorized("Invalid username or password.");
                }

                //tạo chuỗi token
                var jwt = Configuration.GetSection("Jwt").Get<Jwt>();

                var claims = new[]
                {
            new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
            new Claim("email", userData.email.ToString()),
            new Claim("password", userData.password.ToString()),
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
            else
            {
                return BadRequest("Invalid request.");
            }
        }
         //tạo token
        [HttpPost("gentoken-admin")]
        public async Task<IActionResult> GenerateTokenAdmin(login user)
        {
            if (user != null && !string.IsNullOrEmpty(user.email) && !string.IsNullOrEmpty(user.password))
            {
                //lấy dữ liệu nhập vào 
                var userData = await GetAdminInfor(user.email, user.password);

                if (userData == null)
                {
                    return Unauthorized("Invalid username or password.");
                }

                //tạo chuỗi token
                var jwt = Configuration.GetSection("Jwt").Get<Jwt>();

                var claims = new[]
                {
            new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
            new Claim("email", userData.email.ToString()),
            new Claim("password", userData.password.ToString()),
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
            else
            {
                return BadRequest("Invalid request.");
            }
        }

        //kiểm tra người dùng 
        [HttpGet("user-login")]
        public async Task<login> GetUserInfor(string username, string password)
        {
            using (IDbConnection db = new SqlConnection(Configuration.GetConnectionString("SqlConnection")))
            {
                var parameters = new { Username = username, Password = password };
                var user = await db.QueryFirstOrDefaultAsync<login>(
                    "sp_user_login",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                return user;
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

                return user;
            }
        }
    }
}