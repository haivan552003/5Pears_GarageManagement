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
    public class LocationController : ControllerBase
    {
        private readonly string _connectionString;
        public LocationController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SqlConnection");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<location>>> GetLocation()
        {
            var procedureName = "sp_view_location";

            using (var connection = new SqlConnection(_connectionString))
            {
                var location = await connection.QueryAsync<location>(procedureName, commandType: CommandType.StoredProcedure);
                return Ok(location);
            }

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<location>> GetLocationID(int id)
        {
            //return Ok(product);
            var procedureName = "sp_getid_location";
            var parameters = new DynamicParameters();
            parameters.Add("id", id, DbType.Int32, ParameterDirection.Input);

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var location = await connection.QueryFirstOrDefaultAsync<location>(
                    procedureName, parameters, commandType: CommandType.StoredProcedure);

                if (location == null)
                {
                    return NotFound();
                }

                return Ok(location);
            }
        }


        [HttpPost]
        public async Task<ActionResult<location>> PostLocation(location Location)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_add_location", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@name", Location.name);
                cmd.Parameters.AddWithValue("@address", Location.address);
                cmd.Parameters.AddWithValue("@phone_number", Location.phone_number);
                cmd.Parameters.AddWithValue("@status", Location.status);
                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }

            return CreatedAtAction(nameof(GetLocation), new { id = Location.id }, Location);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutLocation(int id, location Location)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_update_location", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@name", Location.name);
                cmd.Parameters.AddWithValue("@address", Location.address);
                cmd.Parameters.AddWithValue("@phone_number", Location.phone_number);
                cmd.Parameters.AddWithValue("@location_code", Location.location_code);
                cmd.Parameters.AddWithValue("@status", Location.status);
                cmd.Parameters.AddWithValue("@id", id);
                await conn.OpenAsync();

                int rowsAffected = await cmd.ExecuteNonQueryAsync();

                if (rowsAffected == 0)
                {
                    return NotFound();
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocation(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_delete_location", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                await conn.OpenAsync();

                int rowsAffected = await cmd.ExecuteNonQueryAsync();

                if (rowsAffected == 0)
                {
                    return NotFound();
                }
            }

            return NoContent();
        }
    }
}
