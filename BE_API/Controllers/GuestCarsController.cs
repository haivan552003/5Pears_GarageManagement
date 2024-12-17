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
    public class GuestCarsController : ControllerBase
    {
        private readonly string _connectionString;
        public GuestCarsController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SqlConnection");
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<guest_car>>> GetGuestCar()
        {
            var procedureName = "sp_get_guest_car";

            using (var connection = new SqlConnection(_connectionString))
            {
                var employeesList = await connection.QueryAsync<guest_car>(procedureName, commandType: CommandType.StoredProcedure);
                return Ok(employeesList);
            }
        }

        [HttpGet("GetTop")]
        public async Task<ActionResult<IEnumerable<guest_car>>> GetTopGuestCar()
        {
            var procedureName = "sp_get_top_guest_car";

            using (var connection = new SqlConnection(_connectionString))
            {
                var employeesList = await connection.QueryAsync<guest_car>(procedureName, commandType: CommandType.StoredProcedure);
                return Ok(employeesList);
            }
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<guest_car>> GetGuestCarID(int id)
        {
            var procedureName = "sp_getid_guest_car";
            var parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Int32, ParameterDirection.Input);

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var product = await connection.QueryFirstOrDefaultAsync<guest_car>(
                    procedureName, parameters, commandType: CommandType.StoredProcedure);

                if (product == null)
                {
                    return NotFound();
                }

                return Ok(product);
            }
        }

        [HttpPost]
        public async Task<ActionResult> AddGuestCar(guest_car_create request)
        {
            try
            {
                var parameters = new DynamicParameters(request);
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var result = await connection.ExecuteAsync("sp_create_guest_car", parameters, commandType: CommandType.StoredProcedure);

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

        //update -> status = 2(nếu khách đã thanh toán)
        [HttpPut("PutStatus2/{id}")]
        public async Task<IActionResult> PutStatus2(int id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@id", id);

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var result = await connection.ExecuteAsync("sp_update_status_2_guest_car", parameters, commandType: CommandType.StoredProcedure);

                if (result == 0)
                {
                    return NotFound();
                }
            }

            return NoContent();
        }

        //update -> status = 6(Hủy sau khi tạo đơn xong mà chưa thanh toán)
        [HttpPut("PutStatus6/{id}")]
        public async Task<IActionResult> PutStatus6(int id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@id", id);

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var result = await connection.ExecuteAsync("sp_update_status_6_guest_car", parameters, commandType: CommandType.StoredProcedure);

                if (result == 0)
                {
                    return NotFound();
                }
            }

            return NoContent();
        }

        //update -> status = 9(Đã hoàn tiền, hoàn tiền sẽ vào ví)
        [HttpPut("PutStatus9/{id}")]
        public async Task<IActionResult> PutStatus9(int id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@id", id);

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var result = await connection.ExecuteAsync("sp_update_status_9_guest_car", parameters, commandType: CommandType.StoredProcedure);

                if (result == 0)
                {
                    return NotFound();
                }
            }

            return NoContent();
        }

        //update -> status = 10(Đã cọc)
        [HttpPut("PutStatus10/{id}")]
        public async Task<IActionResult> PutStatus10(int id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@id", id);

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var result = await connection.ExecuteAsync("sp_update_status_10_guest_car", parameters, commandType: CommandType.StoredProcedure);

                if (result == 0)
                {
                    return NotFound();
                }
            }

            return NoContent();
        }

        //update -> status = 11(Đã giao xe)
        [HttpPut("PutStatus11/{id}")]
        public async Task<IActionResult> PutStatus11(int id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@id", id);

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var result = await connection.ExecuteAsync("sp_update_status_11_guest_car", parameters, commandType: CommandType.StoredProcedure);

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

                var result = await connection.ExecuteAsync("sp_update_status_12_guest_car", parameters, commandType: CommandType.StoredProcedure);

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

                var result = await connection.ExecuteAsync("sp_update_status_13_guest_car", parameters, commandType: CommandType.StoredProcedure);

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

                var result = await connection.ExecuteAsync("sp_update_status_14_guest_car", parameters, commandType: CommandType.StoredProcedure);

                if (result == 0)
                {
                    return NotFound();
                }
            }

            return NoContent();
        }

        // POST: api/Products/CheckDateRetailCar
        [HttpPost("CheckDateRetailCar")]
        public async Task<ActionResult<bool>> CheckDateRetailCar([FromBody] CarRentalRequest request)
        {
            var procedureName = "sp_check_date_retail_car";
            var parameters = new DynamicParameters();
            parameters.Add("@date_start", request.DateStart, DbType.Date, ParameterDirection.Input);
            parameters.Add("@car_id", request.CarId, DbType.Int32, ParameterDirection.Input);

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

        [HttpPost("AddImgCarDriver")]
        public async Task<ActionResult> AddImg(AddImgGC request)
        {
            try
            {
                var parameters = new DynamicParameters(request);

                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var result = await connection.ExecuteAsync("sp_update_gc_img", parameters, commandType: CommandType.StoredProcedure);

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
        public async Task<ActionResult<List<GetImgGC>>> GetImgByID(int id)
        {
            var procedureName = "sp_get_id_gc_img";
            var parameters = new DynamicParameters();
            parameters.Add("@guest_car_Id", id, DbType.Int32, ParameterDirection.Input);
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var images = await connection.QueryAsync<GetImgGC>(
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
