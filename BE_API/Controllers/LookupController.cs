using BE_API.Models;
using Dapper;
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
    public class LookupController : ControllerBase
    {
        private readonly string _connectionString;
        public LookupController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SqlConnection");
        }
        [HttpGet("{phoneNumber}/{ticketCode}")]
        public async Task<ActionResult<IEnumerable<lookup>>> GetCarTripDetails(string phoneNumber, int ticketCode)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@PhoneNumber", phoneNumber, DbType.String);
            parameters.Add("@TicketCode", ticketCode, DbType.Int32);
            using (var connection = new SqlConnection(_connectionString))
            {
                var result = await connection.QueryAsync<lookup>("sp_lookup_car", parameters, commandType: CommandType.StoredProcedure);
                return Ok(result);
            }
        }
    }
}
