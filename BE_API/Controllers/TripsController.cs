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
    public class TripsController : Controller
    {
        private readonly string _connectionString;
        public TripsController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SqlConnection");
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<trip>>> sp_Get_Trips()
        {
            var procedureName = "sp_Get_Trips";

            using (var connection = new SqlConnection(_connectionString))
            {
                var employeesList = await connection.QueryAsync<trip>(procedureName, commandType: CommandType.StoredProcedure);
                return Ok(employeesList);
            }
        }
        // GET: api/Products/5

        [HttpGet("{id}")]
        public async Task<ActionResult<trip>> sp_GetTripsID(int id)
        {
            //return Ok(product);
            var procedureName = "sp_GetTripsID";
            var parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Int32, ParameterDirection.Input);

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var product = await connection.QueryFirstOrDefaultAsync<trip>(
                    procedureName, parameters, commandType: CommandType.StoredProcedure);

                if (product == null)
                {
                    return NotFound();
                }

                return Ok(product);
            }

        }
        // POST: api/trips
        [HttpPost]
        public async Task<ActionResult> AddTrip([FromBody] trip newTrip)
        {
            // Kiểm tra xem đối tượng có hợp lệ không
            if (newTrip == null || string.IsNullOrEmpty(newTrip.img_trip) ||
                string.IsNullOrEmpty(newTrip.from) || string.IsNullOrEmpty(newTrip.to) ||
                string.IsNullOrEmpty(newTrip.trip_code))
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            // Sử dụng DynamicParameters để truyền tham số
            var parameters = new DynamicParameters();
            parameters.Add("@img_trip", newTrip.img_trip);
            parameters.Add("@from", newTrip.from);
            parameters.Add("@to", newTrip.to);
            parameters.Add("@date_create", DateTime.Now); // Thời gian hiện tại
            parameters.Add("@date_update", DateTime.Now); // Thời gian hiện tại
            parameters.Add("@is_delete", 0); // Mặc định là 0 (chưa bị xóa)
            parameters.Add("@emp_create", newTrip.emp_create);
            parameters.Add("@trip_code", newTrip.trip_code);
            parameters.Add("@status", newTrip.status);
            parameters.Add("@is_return", newTrip.is_return);

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var result = await connection.ExecuteAsync("sp_add_trip", parameters, commandType: CommandType.StoredProcedure);

                if (result > 0)
                {
                    return Ok(new { message = "Chuyến đi đã được thêm thành công" });
                }

                return BadRequest(new { message = "Thêm chuyến đi không thành công" });
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTrip(int id, [FromBody] trip trip)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                // Kiểm tra xem nhân viên có tồn tại không
                var empExists = await conn.ExecuteScalarAsync<int>(
                    "SELECT COUNT(1) FROM employees WHERE id = @emp_create",
                    new { emp_create = trip.emp_create });

                if (empExists == 0)
                {
                    return BadRequest("Nhân viên không tồn tại.");
                }

                // Tiến hành cập nhật nếu nhân viên hợp lệ
                using (var cmd = new SqlCommand("sp_update_trip", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Thêm các tham số
                    cmd.Parameters.AddWithValue("@id_trip", id);
                    cmd.Parameters.AddWithValue("@img_trip", trip.img_trip ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@from", trip.from ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@to", trip.to ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@date_update", DateTime.Now);
                    cmd.Parameters.AddWithValue("@is_delete", trip.is_delete);
                    cmd.Parameters.AddWithValue("@emp_create", trip.emp_create);
                    cmd.Parameters.AddWithValue("@trip_code", trip.trip_code ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@status", trip.status);
                    cmd.Parameters.AddWithValue("@is_return", trip.is_return);

                    int rowsAffected = await cmd.ExecuteNonQueryAsync();

                    if (rowsAffected == 0)
                    {
                        return NotFound("Không tìm thấy chuyến đi để cập nhật.");
                    }
                }
            }

            return NoContent(); // Trả về NoContent nếu cập nhật thành công
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrip(int id)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var parameters = new DynamicParameters();
                    parameters.Add("@id", id, DbType.Int32, ParameterDirection.Input); // Tham số ID cho stored procedure

                    var result = await connection.ExecuteAsync(
                        "sp_delete_trips",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    if (result > 0)
                    {
                        return Ok(new { message = "Trip deleted successfully." });
                    }
                    else
                    {
                        return NotFound(new { message = "Trip not found or already deleted." });
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error occurred: {ex.Message}" });
            }
        }


    }
}
