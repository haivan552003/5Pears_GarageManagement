using BE_API.Models;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System;
using System.Linq;
using BE_API.ModelCustom;
using Microsoft.AspNetCore.SignalR;

namespace BE_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuestTripController : ControllerBase
    {
        private readonly string _connectionString;
        public GuestTripController(IConfiguration configuration )
        {
            _connectionString = configuration.GetConnectionString("SqlConnection");
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<guest_trip>>> GetGuestTrip()
        {
            var procedureName = "sp_get_guest_trip";

            using (var connection = new SqlConnection(_connectionString))
            {
                var employeesList = await connection.QueryAsync<guest_trip>(procedureName, commandType: CommandType.StoredProcedure);
                return Ok(employeesList);
            }
        }
        [HttpGet("GetTop")]
        public async Task<ActionResult<IEnumerable<guest_trip>>> GetTopGuestTrip()
        {
            var procedureName = "sp_get_top_guest_trip";

            using (var connection = new SqlConnection(_connectionString))
            {
                var employeesList = await connection.QueryAsync<guest_trip>(procedureName, commandType: CommandType.StoredProcedure);
                return Ok(employeesList);
            }
        }


        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<guest_trip>> GetGuestTripID(int id)
        {
            var procedureName = "sp_getid_guest_trip_admin";
            var parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Int32, ParameterDirection.Input);

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var multi = await connection.QueryMultipleAsync(procedureName, parameters, commandType: CommandType.StoredProcedure))
                {
                    var guest_trip = multi.ReadFirstOrDefault<guest_trip>();

                    if (guest_trip == null)
                    {
                        return NotFound();
                    }

                    var guest_trip_childs = multi.Read<guest_trip_child>().ToList();

                    guest_trip.guest_trip_child = guest_trip_childs;

                    return Ok(guest_trip);
                }
            }
        }
        // POST: api/employees
        [HttpPost]
        public async Task<ActionResult> AddGuestTrip(guest_trip_create request)
        {
            try
            {
                var parameters = new DynamicParameters(request);

                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var result = await connection.ExecuteAsync("sp_create_guest_trip", parameters, commandType: CommandType.StoredProcedure);

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

                var result = await connection.ExecuteAsync("sp_update_status_2_guest_trip", parameters, commandType: CommandType.StoredProcedure);

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

                var result = await connection.ExecuteAsync("sp_update_status_6_guest_trip", parameters, commandType: CommandType.StoredProcedure);

                if (result == 0)
                {
                    return NotFound();
                }
            }

            return NoContent();
        }

        //update -> status = 8(Khách hang đã thanh toán nhưng muốn hủy đơn, hoàn tiền sẽ vào ví)
        [HttpPut("PutStatus8/{id}")]
        public async Task<IActionResult> PutStatus8(int id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@id", id);

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var result = await connection.ExecuteAsync("sp_update_status_8_guest_trip", parameters, commandType: CommandType.StoredProcedure);

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

                var result = await connection.ExecuteAsync("sp_update_status_9_guest_trip", parameters, commandType: CommandType.StoredProcedure);

                if (result == 0)
                {
                    return NotFound();
                }
            }

            return NoContent();
        }//update -> status = 9(Đã hoàn tiền, hoàn tiền sẽ vào ví)

        //check seat not null
        [HttpGet("GetSeatNotNull/{id}")]
        public async Task<IActionResult> GetSeatNotNull(int id)
        {
            var procedureName = "sp_get_seat_not_null";
            var parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Int32, ParameterDirection.Input);

            using (var connection = new SqlConnection(_connectionString))
            {
                var seatsList = await connection.QueryAsync<car_seat_not_null>(
                    procedureName,
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                if (seatsList == null)
                {
                    return NotFound();
                }


                return Ok(seatsList);
            }
        }
    }
}
