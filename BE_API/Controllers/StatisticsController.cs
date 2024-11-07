using BE_API.ModelCustom;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
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
        public async Task<ActionResult<IEnumerable<DriverStatistics>>> GetDriver()
        {
            var procedureName = "sp_statistics_driver_year";

            using (var connection = new SqlConnection(_connectionString))
            {
                var banners = await connection.QueryAsync<DriverStatistics>(procedureName, commandType: CommandType.StoredProcedure);
                return Ok(banners);
            }

        }

        [HttpGet("Car_Statistics")]
        public async Task<ActionResult<IEnumerable<CarStatistics>>> GetCar()
        {
            var procedureName = "sp_statistics_car_year";

            using (var connection = new SqlConnection(_connectionString))
            {
                var banners = await connection.QueryAsync<CarStatistics>(procedureName, commandType: CommandType.StoredProcedure);
                return Ok(banners);
            }

        }

        [HttpGet("Trip_Statistics")]
        public async Task<ActionResult<IEnumerable<TripStatistics>>> GetTripDetails()
        {
            var procedureName = "sp_statistics_trip_per_year";

            using (var connection = new SqlConnection(_connectionString))
            {
                var banners = await connection.QueryAsync<TripStatistics>(procedureName, commandType: CommandType.StoredProcedure);
                return Ok(banners);
            }

        }


    }
}
