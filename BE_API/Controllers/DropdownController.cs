using BE_API.ModelCustom;
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
    public class DropdownController : ControllerBase
    {
        private readonly string _connectionString;
        public DropdownController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SqlConnection");
        }

        [HttpGet("emp")]
        public async Task<ActionResult<IEnumerable<dropdown>>> DropdownUser()
        {
            var procedureName = "sp_dropdown_user";

            using (var connection = new SqlConnection(_connectionString))
            {
                var dropdown = await connection.QueryAsync<dropdown>(procedureName, commandType: CommandType.StoredProcedure);
                return Ok(dropdown);
            }

        }
    }
}
