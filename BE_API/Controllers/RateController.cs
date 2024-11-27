using BE_API.ModelCustom;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BE_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RateController : Controller
    {
        private readonly string _connectionString;
        public RateController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SqlConnection");
        }


        [HttpPut("RateGuestDriver/{id}")]
        public async Task<IActionResult> RateGuestDriver(int id, [FromBody] RateModel request)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@id", id);
                parameters.Add("@rate", request.rate);
                parameters.Add("@rate_content", request.rate_content);

                await connection.ExecuteAsync("sp_rate_guest_driver", parameters, commandType: CommandType.StoredProcedure);
            }

            return NoContent();
        }
          [HttpPut("RateGuestCars/{id}")]
        public async Task<IActionResult> RateGuestCars(int id, [FromBody] RateModel request)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@id", id);
                parameters.Add("@rate", request.rate);
                parameters.Add("@rate_content", request.rate_content);

                await connection.ExecuteAsync("sp_rate_guest_cars", parameters, commandType: CommandType.StoredProcedure);
            }

            return NoContent();
        }
          [HttpPut("RateGuestCarsDriver/{id}")]
        public async Task<IActionResult> RateGuestCarsDriver(int id, [FromBody] RateModel request)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@id", id);
                parameters.Add("@rate", request.rate);
                parameters.Add("@rate_content", request.rate_content);

                await connection.ExecuteAsync("sp_rate_guest_car_driver", parameters, commandType: CommandType.StoredProcedure);
            }

            return NoContent();
        }
        [HttpGet("GetCustomerGuestCarDriverHistory/{cusId}")]
        public async Task<ActionResult<List<GuestCarDriverHistory>>> GetCustomerGuestCarDriverHistory(int cusId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var results = await connection.QueryAsync<GuestCarDriverHistory>("sp_cus_history_gcd",
                    new { cus_id = cusId },
                    commandType: CommandType.StoredProcedure);
                return Ok(results.ToList());
            }
        }

        [HttpGet("GetCustomerGuestCarHistory/{cusId}")]
        public async Task<ActionResult<List<GuestCarHistory>>> GetCustomerGuestCarHistory(int cusId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var results = await connection.QueryAsync<GuestCarHistory>("sp_cus_history_gc",
                    new { cus_id = cusId },
                    commandType: CommandType.StoredProcedure);
                return Ok(results.ToList());
            }
        }

        [HttpGet("GetCustomerGuestDriverHistory/{cusId}")]
        public async Task<ActionResult<List<GuestDriverHistory>>> GetCustomerGuestDriverHistory(int cusId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var results = await connection.QueryAsync<GuestDriverHistory>("sp_cus_history_gd",
                    new { cus_id = cusId },
                    commandType: CommandType.StoredProcedure);
                return Ok(results.ToList());
            }
        }

        [HttpGet("GetCustomerGuestTripHistory/{cusId}")]
        public async Task<ActionResult<List<GuestTripHistory>>> GetCustomerGuestTripHistory(int cusId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var results = await connection.QueryAsync<GuestTripHistory>("sp_cus_history_gt",
                    new { cus_id = cusId },
                    commandType: CommandType.StoredProcedure);
                return Ok(results.ToList());
            }
        }




        [HttpPut("Status6_gt/{id}")]
        public async Task<IActionResult> Status6_gt(int id)
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


        [HttpPut("Status6_gc/{id}")]
        public async Task<IActionResult> Status6_gc(int id)
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
        [HttpPut("Status6_gd/{id}")]
        public async Task<IActionResult> Status6_gd(int id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@id", id);

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var result = await connection.ExecuteAsync("sp_update_status_6_guest_driver", parameters, commandType: CommandType.StoredProcedure);

                if (result == 0)
                {
                    return NotFound();
                }
            }

            return NoContent();
        }
        [HttpPut("Status6_gcd/{id}")]
        public async Task<IActionResult> Status6_gcd(int id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@id", id);

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var result = await connection.ExecuteAsync("sp_update_status_6_guest_car_driver", parameters, commandType: CommandType.StoredProcedure);

                if (result == 0)
                {
                    return NotFound();
                }
            }

            return NoContent();
        }
    }
}
