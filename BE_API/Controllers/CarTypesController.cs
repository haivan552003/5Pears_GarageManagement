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
    public class CarTypesController : ControllerBase
    {
        private readonly string _connectionString;
        public CarTypesController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SqlConnection");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<car_types>>> GetCarTypes()
        {
            var procedureName = "sp_view_car_types";

            using (var connection = new SqlConnection(_connectionString))
            {
                var car_types = await connection.QueryAsync<car_types>(procedureName, commandType: CommandType.StoredProcedure);
                return Ok(car_types);
            }

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<car_types>> GetCarTypesID(int id)
        {
            //return Ok(product);
            var procedureName = "sp_getid_car_types";
            var parameters = new DynamicParameters();
            parameters.Add("id", id, DbType.Int32, ParameterDirection.Input);

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var car_types = await connection.QueryFirstOrDefaultAsync<car_types>(
                    procedureName, parameters, commandType: CommandType.StoredProcedure);

                if (car_types == null)
                {
                    return NotFound();
                }

                return Ok(car_types);
            }
        }


        [HttpPost]
        public async Task<ActionResult<car_types>> PostCarTypes(car_types CarTypes)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_add_car_types", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@name", CarTypes.name);
                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }

            return CreatedAtAction(nameof(GetCarTypes), new { id = CarTypes.id }, CarTypes);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCarTypes(int id, car_types CarTypes)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_update_car_types", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@name", CarTypes.name);
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
        public async Task<IActionResult> DeleteCarTypes(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_delete_car_types", conn);
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
