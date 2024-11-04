using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System;
using BE_API.Models;


namespace BE_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordController : ControllerBase
    {
        private readonly string _connectionString;

        public PasswordController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SqlConnection");
        }

        [HttpPost("change")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePassWord request)
        {
   
            using (var connection = new SqlConnection(_connectionString))
            {
                // Kiểm tra mật khẩu cũ có đúng không
                var userExists = await connection.QueryFirstOrDefaultAsync<int>(
                    "SELECT COUNT(*) FROM customers WHERE id = @Id AND password = @OldPassword",
                    new { Id = request.id, OldPassword = request.OldPassword }
                );

                if (userExists == 0)
                {
                    return BadRequest("Mật khẩu cũ không chính xác");
                }

                // Gọi stored procedure để đổi mật khẩu
                await connection.ExecuteAsync(
                    "sp_ChangePassword",
                    new { CustomerId = request.id, OldPassword = request.OldPassword, NewPassword = request.NewPassword },
                    commandType: CommandType.StoredProcedure
                );

                return Ok(new { message = "Đổi mật khẩu thành công" });
            }
        }

    }
}
