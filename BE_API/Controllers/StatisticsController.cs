using BE_API.ModelCustom;
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
    public class StatisticsController : ControllerBase
    {
        private readonly string _connectionString;
        public StatisticsController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SqlConnection");
        }

        [HttpGet("Driver_Statistics")]
        public async Task<IEnumerable<DriverStatistics>> GetDriverStatisticsAsync([FromQuery] int? year = null)
        {
            using var connection = new SqlConnection(_connectionString);
            var parameters = new DynamicParameters();
            parameters.Add("@Year", year ?? DateTime.Now.Year);

            return await connection.QueryAsync<DriverStatistics>(
                "sp_statistics_driver_year",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        [HttpGet("Car_Statistics")]
        public async Task<ActionResult<IEnumerable<CarStatistics>>> GetCarStatisticsAsync([FromQuery] int? year = null)
        {
            using var connection = new SqlConnection(_connectionString);
            var parameters = new DynamicParameters();
            parameters.Add("@Year", year ?? DateTime.Now.Year);

            var result = await connection.QueryAsync<CarStatistics>(
                "sp_statistics_car_year",
                parameters,
                commandType: CommandType.StoredProcedure
            );
            return Ok(result);
        }

        [HttpGet("Trip_Statistics")]
        public async Task<ActionResult<IEnumerable<TripStatistics>>> GetTripStatisticsAsync([FromQuery] int? year = null)
        {
            using var connection = new SqlConnection(_connectionString);
            var parameters = new DynamicParameters();
            parameters.Add("@Year", year ?? DateTime.Now.Year);

            var result = await connection.QueryAsync<TripStatistics>(
                "sp_statistics_trip_per_year",
                parameters,
                commandType: CommandType.StoredProcedure
            );
            return Ok(result);
        }
        [HttpGet("Driver_by_trip")]
        public async Task<ActionResult<IEnumerable<statistics_driver_by_trip>>> statistics_driver_by_trip()
        {
            using var connection = new SqlConnection(_connectionString);
            var result = await connection.QueryAsync<statistics_driver_by_trip>(
                "sp_statistics_driver_by_trip",
                commandType: CommandType.StoredProcedure
            );
            return Ok(result);
        }
        [HttpGet("Driver_by_GCD")]
        public async Task<ActionResult<IEnumerable<statistics_driver_by_GCD>>> statistics_driver_by_gcd()
        {
            using var connection = new SqlConnection(_connectionString);
            var result = await connection.QueryAsync<statistics_driver_by_GCD>(
                "sp_statistics_driver_by_GCD",
                commandType: CommandType.StoredProcedure
            );
            return Ok(result);
        }
        [HttpGet("Driver_monthly_trip/{driverId}")]
        public async Task<ActionResult<IEnumerable<statistics_driver_by_trip_monly>>> GetDriverMonthlyTripStats(int driverId)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var result = await connection.QueryAsync<statistics_driver_by_trip_monly>(
                    "sp_statistics_driver_monthly_GCD", 
                    new { DriverId = driverId },
                    commandType: CommandType.StoredProcedure
                );
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("Driver_monthly_GCD/{driverId}")]
        public async Task<ActionResult<IEnumerable<statistics_driver_by_GCD_monly>>> GetDriverMonthlyGCDStats(int driverId)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var result = await connection.QueryAsync<statistics_driver_by_GCD_monly>(
                    "sp_statistics_driver_monthly", 
                    new { DriverId = driverId },
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
