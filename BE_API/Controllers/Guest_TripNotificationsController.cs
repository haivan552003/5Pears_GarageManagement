using BE_API.ModelCustom;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Threading.Tasks;

namespace BE_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuestTripNotificationsController : ControllerBase
    {
        private readonly string _connectionString;

        public GuestTripNotificationsController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SqlConnection");
        }

        [HttpGet("GetDriverGuestTrips/{id}")]
        public async Task<IActionResult> GetDriverGuestTrips(int id)
        {
            if (string.IsNullOrEmpty(_connectionString))
            {
                return StatusCode(500, new { message = "Lỗi cấu hình: Connection string không tồn tại." });
            }

            using var connection = new SqlConnection(_connectionString);

            try
            {
                var query = "sp_get_notification_driver_guest_trip";

                var parameters = new { id };

                var result = await connection.QueryAsync<DriverGuestTripNotification>(
                    query,
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                if (result == null)
                {
                    return NotFound(new { message = "Không tìm thấy dữ liệu phù hợp." });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi lấy danh sách thông báo", error = ex.Message });
            }
        }

        //guest_driver
        [HttpGet("GetDriverGuestDriverNotifications/{id}")]
        public async Task<IActionResult> GetDriverGuestDriverNotifications(int id)
        {
            if (string.IsNullOrEmpty(_connectionString))
            {
                return StatusCode(500, new { message = "Lỗi cấu hình: Connection string không tồn tại." });
            }

            using var connection = new SqlConnection(_connectionString);

            try
            {
                var query = "sp_get_notification_driver_guest_driver";

                var parameters = new { id };

                var result = await connection.QueryAsync<DriverGuestNotification>(
                    query,
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                if (result == null)
                {
                    return NotFound(new { message = "Không tìm thấy dữ liệu phù hợp." });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi lấy danh sách thông báo", error = ex.Message });
            }
        }

        //Guest_driver_car
        [HttpGet("GetGuestDriverCarNotifications/{id}")]
        public async Task<IActionResult> GetGuestDriverCarNotifications(int id)
        {
            if (string.IsNullOrEmpty(_connectionString))
            {
                return StatusCode(500, new { message = "Lỗi cấu hình: Connection string không tồn tại." });
            }

            using var connection = new SqlConnection(_connectionString);

            try
            {
                var query = "sp_get_notification_driver_guest_car_driver";

                var parameters = new { id };

                var result = await connection.QueryAsync<DriverGuestCarNotification>(
                    query,
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                if (result == null)
                {
                    return NotFound(new { message = "Không tìm thấy dữ liệu phù hợp." });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi lấy danh sách thông báo", error = ex.Message });
            }
        }
    }
}
