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
        public async Task<ActionResult<driver>> AddDriver(driver driver)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_create_drivers", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@fullname", driver.fullname);
                cmd.Parameters.AddWithValue("@birthday", driver.birthday);
                cmd.Parameters.AddWithValue("@img_driver", driver.img_driver);
                cmd.Parameters.AddWithValue("@driver_license_img1", driver.driver_license_img1);  
                cmd.Parameters.AddWithValue("@driver_license_img2", driver.driver_license_img2);
                cmd.Parameters.AddWithValue("@driver_license_number", driver.driver_license_number);
                cmd.Parameters.AddWithValue("@citizen_identity_img1", driver.citizen_identity_img1);    
                cmd.Parameters.AddWithValue("@citizen_identity_img2", driver.citizen_identity_img2);
                cmd.Parameters.AddWithValue("@citizen_identity_number", driver.citizen_identity_number);
                cmd.Parameters.AddWithValue("@gender", driver.gender);
                cmd.Parameters.AddWithValue("@price", driver.price);
                cmd.Parameters.AddWithValue("@voucher", driver.voucher);
                cmd.Parameters.AddWithValue("@phonenumber", driver.phonenumber);
                cmd.Parameters.AddWithValue("@address", driver.address);
                cmd.Parameters.AddWithValue("@status", driver.status);
                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }

            return CreatedAtAction(nameof(GetAllDrivers), new { id = driver.id }, driver);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutDriver(int id, driver driver)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_update_driver", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@fullname", driver.fullname);
                cmd.Parameters.AddWithValue("@birthday", driver.birthday);
                cmd.Parameters.AddWithValue("@img_driver", driver.img_driver);
                cmd.Parameters.AddWithValue("@driver_license_img1", driver.driver_license_img1);
                cmd.Parameters.AddWithValue("@driver_license_img2", driver.driver_license_img2);
                cmd.Parameters.AddWithValue("@driver_license_number", driver.driver_license_number);
                cmd.Parameters.AddWithValue("@citizen_identity_img1", driver.citizen_identity_img1);
                cmd.Parameters.AddWithValue("@citizen_identity_img2", driver.citizen_identity_img2);
                cmd.Parameters.AddWithValue("@citizen_identity_number", driver.citizen_identity_number);
                cmd.Parameters.AddWithValue("@gender", driver.gender);
                cmd.Parameters.AddWithValue("@price", driver.price);
                cmd.Parameters.AddWithValue("@voucher", driver.voucher);
                cmd.Parameters.AddWithValue("@phonenumber", driver.phonenumber);
                cmd.Parameters.AddWithValue("@address", driver.address);
                cmd.Parameters.AddWithValue("@status", driver.status);
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
