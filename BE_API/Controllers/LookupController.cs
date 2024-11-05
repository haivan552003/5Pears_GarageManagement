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
        public async Task<ActionResult<IEnumerable<Lookup>>> GetCarTripDetails(string phoneNumber, string ticketCode)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@PhoneNumber", phoneNumber, DbType.String);
            parameters.Add("@TicketCode", ticketCode, DbType.String);
            using (var connection = new SqlConnection(_connectionString))
            {
                var result = await connection.QueryAsync<Lookup>("sp_lookup_car", parameters, commandType: CommandType.StoredProcedure);
                return Ok(result);
            }
        }
    }
}
