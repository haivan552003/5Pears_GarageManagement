using BE_API.Models;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace BE_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleTaskController : ControllerBase
    {
        private readonly string _connectionString;
        public RoleTaskController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SqlConnection");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<role_task>>> GetRoleTask()
        {
            var procedureName = "sp_view_role_task";

            using (var connection = new SqlConnection(_connectionString))
            {
                var products = await connection.QueryAsync<role_task>(procedureName, commandType: CommandType.StoredProcedure);
                return Ok(products);
            }

        }
    }
}
