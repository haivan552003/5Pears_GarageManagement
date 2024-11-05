using BE_API.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace BE_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverController : ControllerBase
    {
        private readonly string _connectionString;
        public DriverController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SqlConnection");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<driver>>> GetAllDrivers()
        {
            var procedureName = "sp_getall_drivers";

            using (var connection = new SqlConnection(_connectionString))
            {
                var role_task = await connection.QueryAsync<driver>(procedureName, commandType: CommandType.StoredProcedure);
                return Ok(role_task);
            }

        }
        [HttpGet("{id}")]
        public async Task<ActionResult<driver>> GetDriverByID(int id)
        {
            try
            {
                var procedureName = "sp_get_driver_by_id";
                var parameters = new DynamicParameters();
                parameters.Add("Id", id, DbType.Int32, ParameterDirection.Input);

                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var driver = await connection.QueryFirstOrDefaultAsync<driver>(
                        procedureName, parameters, commandType: CommandType.StoredProcedure);

                    if (driver == null)
                    {
                        return NotFound();
                    }

                    return Ok(driver);
                }
            }
            catch (Exception ex)
            {
                // Log the exception details here
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }


        [HttpPost]
        public async Task<ActionResult<driver>> AddDriver(driver_create driver)
        {
            try
            {
                var parameters = new DynamicParameters(driver);

                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var result = await connection.ExecuteAsync("sp_create_drivers", parameters, commandType: CommandType.StoredProcedure);

                    if (result > 0)
                    {
                        return Ok();
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi: {ex.Message}");
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutDriver(int id, driver_create driver)
        {
            var parameters = new DynamicParameters(driver);
            parameters.Add("@id", id);

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var result = await connection.ExecuteAsync("sp_update_driver", parameters, commandType: CommandType.StoredProcedure);

                if (result == 0)
                {
                    return NotFound();
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDriver(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_delete_driver", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
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
