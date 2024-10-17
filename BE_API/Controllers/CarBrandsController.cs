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
    public class CarBrandsController : ControllerBase
    {
        private readonly string _connectionString;
        public CarBrandsController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SqlConnection");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<car_brands>>> GetCarBrands()
        {
            var procedureName = "sp_view_car_brands";

            using (var connection = new SqlConnection(_connectionString))
            {
                var car_brands = await connection.QueryAsync<car_brands>(procedureName, commandType: CommandType.StoredProcedure);
                return Ok(car_brands);
            }

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<car_brands>> GetCarBrandsID(int id)
        {
            //return Ok(product);
            var procedureName = "sp_getid_car_brands";
            var parameters = new DynamicParameters();
            parameters.Add("id", id, DbType.Int32, ParameterDirection.Input);

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var car_brands = await connection.QueryFirstOrDefaultAsync<car_brands>(
                    procedureName, parameters, commandType: CommandType.StoredProcedure);

                if (car_brands == null)
                {
                    return NotFound();
                }

                return Ok(car_brands);
            }
        }


        [HttpPost]
        public async Task<ActionResult<car_brands>> PostCarBrands(car_brands CarBrands)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_add_car_brands", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@name", CarBrands.name);
                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }

            return CreatedAtAction(nameof(GetCarBrands), new { id = CarBrands.id }, CarBrands);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCarBrands(int id, car_brands CarBrands)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_update_car_brands", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@name", CarBrands.name);
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
        public async Task<IActionResult> DeleteCarBrands(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_delete_car_brands", conn);
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
