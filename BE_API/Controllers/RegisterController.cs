using BE_API.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Threading.Tasks;

namespace BE_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly string _connectionString;
        public RegisterController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SqlConnection");
        }
        [HttpPost]
        public async Task<ActionResult> Register([FromBody] add_model_customers model)
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.pass_word);

            var parameters = new DynamicParameters();
            parameters.Add("@Username", model.user_name, DbType.String);
            parameters.Add("@Password", hashedPassword, DbType.String);
            parameters.Add("@Fullname", model.full_name, DbType.String);
            parameters.Add("@Birthday", model.birthday, DbType.DateTime);
            parameters.Add("@Gender", model.gender, DbType.Byte);
            parameters.Add("@PhoneNumber", model.phone_number, DbType.String);
            parameters.Add("@IDRole", 2, DbType.Int16);


            using (var connection = new SqlConnection(_connectionString))
            {            
                var result = await connection.ExecuteAsync("sp_register", parameters, commandType: CommandType.StoredProcedure);
                return Ok(result);
            }
        }
    }
}
