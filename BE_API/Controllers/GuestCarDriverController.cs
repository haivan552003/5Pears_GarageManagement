using BE_API.ModelCustom;
using BE_API.Models;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BE_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuestCarDriverController : ControllerBase
    {
        private readonly string _connectionString;
        public GuestCarDriverController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SqlConnection");
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<GuestCarDriver>>> GetGuestCar()
        {
            var procedureName = "sp_get_guest_car_driver";

            using (var connection = new SqlConnection(_connectionString))
            {
                var employeesList = await connection.QueryAsync<GuestCarDriver>(procedureName, commandType: CommandType.StoredProcedure);
                return Ok(employeesList);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GuestCarDriver_checkout>> GetGuestCarID(int id)
        {
            var procedureName = "sp_getid_guest_car_driver";
            var parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Int32, ParameterDirection.Input);

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var product = await connection.QueryFirstOrDefaultAsync<GuestCarDriver_checkout>(
                    procedureName, parameters, commandType: CommandType.StoredProcedure);

                if (product == null)
                {
                    return NotFound();
                }

                return Ok(product);
            }
        }
        [HttpGet("GetTop")]
        public async Task<ActionResult<IEnumerable<GuestCarDriver>>> GetTopGuestCar()
        {
            var procedureName = "sp_get_top_guest_car_driver";

            using (var connection = new SqlConnection(_connectionString))
            {
                var employeesList = await connection.QueryAsync<GuestCarDriver>(procedureName, commandType: CommandType.StoredProcedure);
                return Ok(employeesList);
            }
        }

        [HttpPost]
        public async Task<ActionResult> AddGuestCar(GuestCarDriver_create request)
        {
            try
            {
                var parameters = new DynamicParameters(request);
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var result = await connection.ExecuteAsync("sp_create_guest_car_driver", parameters, commandType: CommandType.StoredProcedure);

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
        [HttpPost("CheckDateRetailDriver")]
        public async Task<ActionResult<bool>> CheckDateRetailCar([FromBody] DriverRentalRequest request)
        {
            var procedureName = "sp_check_date_retail_car_driver";
            var parameters = new DynamicParameters();
            parameters.Add("@date_start", request.DateStart, DbType.Date, ParameterDirection.Input);
            parameters.Add("@driver_id", request.DriverId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@car_id", request.CarID, DbType.Int32, ParameterDirection.Input);

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var result = await connection.ExecuteScalarAsync<int>(
                    procedureName, parameters, commandType: CommandType.StoredProcedure);

                if (result == 1)
                {
                    return Ok(true);
                }
                else
                {
                    return Ok(false);
                }
            }
        }

        //update -> status = 10(Đã cọc)
        [HttpPut("PutStatus10/{id}")]
        public async Task<IActionResult> PutStatus9(int id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@id", id);

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var result = await connection.ExecuteAsync("sp_update_status_10_guest_car_driver", parameters, commandType: CommandType.StoredProcedure);

                if (result == 0)
                {
                    return NotFound();
                }
            }

            return NoContent();
        }
        [HttpPut("PutStatus11/{id}")]
        public async Task<IActionResult> PutStatus11(int id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@id", id);

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var result = await connection.ExecuteAsync("sp_update_status_11_guest_car_driver", parameters, commandType: CommandType.StoredProcedure);

                if (result == 0)
                {
                    return NotFound();
                }
            }

            return NoContent();
        }
        [HttpPut("PutStatus12/{id}")]
        public async Task<IActionResult> PutStatus12(int id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@id", id);

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var result = await connection.ExecuteAsync("sp_update_status_12_guest_car_driver", parameters, commandType: CommandType.StoredProcedure);

                if (result == 0)
                {
                    return NotFound();
                }
            }

            return NoContent();
        }

        [HttpPut("PutStatus13/{id}")]
        public async Task<IActionResult> PutStatus13(int id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@id", id);

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var result = await connection.ExecuteAsync("sp_update_status_13_guest_car_driver", parameters, commandType: CommandType.StoredProcedure);

                if (result == 0)
                {
                    return NotFound();
                }
            }

            return NoContent();
        }

        //update -> status = 14(Đã hủy)
        [HttpPut("PutStatus14/{id}")]
        public async Task<IActionResult> PutStatus14(int id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@id", id);

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var result = await connection.ExecuteAsync("sp_update_status_14_guest_car_driver", parameters, commandType: CommandType.StoredProcedure);

                if (result == 0)
                {
                    return NotFound();
                }
            }

            return NoContent();
        }

        [HttpPost("AddImgCarDriver")]
        public async Task<ActionResult> AddImg(AddImgCarDriver request)
        {
            try
            {
                var parameters = new DynamicParameters(request);

                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var result = await connection.ExecuteAsync("sp_update_gcd_img", parameters, commandType: CommandType.StoredProcedure);

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
        [HttpGet("GetImg/{id}")]
        public async Task<ActionResult<List<GetImgGcd>>> GetImgByID(int id)
        {
            var procedureName = "sp_get_id_gcd_img";
            var parameters = new DynamicParameters();
            parameters.Add("@guest_car_driver_Id", id, DbType.Int32, ParameterDirection.Input);
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var images = await connection.QueryAsync<GetImgGcd>(
                    procedureName, parameters, commandType: CommandType.StoredProcedure);

                if (images == null || !images.Any())
                {
                    return NotFound();
                }
                return Ok(images.ToList());
            }
        }
    }
}
