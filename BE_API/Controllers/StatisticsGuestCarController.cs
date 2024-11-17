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
using System.Threading.Tasks;

namespace BE_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsGuestCarController : ControllerBase
    {
        private readonly string _connectionString;
        public StatisticsGuestCarController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SqlConnection");
        }

        [HttpGet("Car_Month_Statistics")]
        public async Task<ActionResult<CarMonthStatistics>> GetCarMonthStatisticsAsync([FromQuery] int carId, [FromQuery] int month, [FromQuery] int year)
        {
            if (carId <= 0 || month < 1 || month > 12 || year <= 0)
                return BadRequest("Invalid parameters.");

            using var connection = new SqlConnection(_connectionString);
            var parameters = new DynamicParameters();
            parameters.Add("@CarId", carId);
            parameters.Add("@Month", month);
            parameters.Add("@Year", year);

            var result = await connection.QueryFirstOrDefaultAsync<CarMonthStatistics>(
                "sp_statistics_car_month1",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            if (result == null)
                return NotFound("No data found for the specified parameters.");

            return Ok(result);
        }

        [HttpGet("CarMonthly")]
        public async Task<ActionResult<IEnumerable<statistics_car>>> statistics_car()
        {
            using var connection = new SqlConnection(_connectionString);
            var result = await connection.QueryAsync<statistics_car>(
                "sp_statistics_car_year",
                commandType: CommandType.StoredProcedure
            );
            return Ok(result);
        }

        [HttpGet("Car_monthly/{carId}")]
        public async Task<ActionResult<IEnumerable<statistics_car_monthly>>> GetCarMonthlyStats(int carId)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var result = await connection.QueryAsync<statistics_car_monthly>(
                    "sp_statistics_car_year_monthly",
                    new { CarId = carId },
                    commandType: CommandType.StoredProcedure
                );
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



    }
}
