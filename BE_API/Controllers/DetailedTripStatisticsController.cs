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
    public class DetailedTripStatisticsController : ControllerBase
    {
        private readonly string _connectionString;

        public DetailedTripStatisticsController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SqlConnection");
        }

        [HttpGet("TripMonthly")]
        public async Task<ActionResult<IEnumerable<DetailedTripStatistics>>> statistics_car()
        {
            using var connection = new SqlConnection(_connectionString);
            var result = await connection.QueryAsync<DetailedTripStatistics>(
                "sp_GetDetailedTripStatistics",
                commandType: CommandType.StoredProcedure
            );
            return Ok(result);
        }

        [HttpGet("Trip_monthly/{tripId}")]
        public async Task<ActionResult<IEnumerable<MonthlyTripStatistics>>> GetCarMonthlyStats(int tripId)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var result = await connection.QueryAsync<MonthlyTripStatistics>(
                    "sp_GetMonthlyTripStatistics",
                    new { TripID = tripId },
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
