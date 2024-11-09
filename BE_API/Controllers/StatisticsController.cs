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
    }
}
