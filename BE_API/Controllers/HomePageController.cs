using BE_API.ModelCustom;
using BE_API.Models;
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
    public class HomePageController : ControllerBase
    {
        private readonly string _connectionString;
        public HomePageController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SqlConnection");
        }

        [HttpGet("Get3News1")]
        public async Task<ActionResult<IEnumerable<news>>> Get3News_1()
        {
            var procedureName = "sp_view_3_news_1";

            using (var connection = new SqlConnection(_connectionString))
            {
                var products = await connection.QueryAsync<news>(procedureName, commandType: CommandType.StoredProcedure);
                return Ok(products);
            }

        }

        [HttpGet("Get3News2")]
        public async Task<ActionResult<IEnumerable<news>>> Get3News_2()
        {
            var procedureName = "sp_view_3_news_2";

            using (var connection = new SqlConnection(_connectionString))
            {
                var products = await connection.QueryAsync<news>(procedureName, commandType: CommandType.StoredProcedure);
                return Ok(products);
            }

        }

        [HttpGet("Get3News3")]
        public async Task<ActionResult<IEnumerable<news>>> Get3News_3()
        {
            var procedureName = "sp_view_3_news_3";

            using (var connection = new SqlConnection(_connectionString))
            {
                var products = await connection.QueryAsync<news>(procedureName, commandType: CommandType.StoredProcedure);
                return Ok(products);
            }

        }

        [HttpGet("TripHot1")]
        public async Task<ActionResult<IEnumerable<trip_hot_home>>> TripHot1()
        {
            var procedureName = "sp_view_trip_hot_1";

            using (var connection = new SqlConnection(_connectionString))
            {
                var products = await connection.QueryAsync<trip_hot_home>(procedureName, commandType: CommandType.StoredProcedure);
                return Ok(products);
            }
        }

        [HttpGet("TripHot2")]
        public async Task<ActionResult<IEnumerable<trip_hot_home>>> TripHot2()
        {
            var procedureName = "sp_view_trip_hot_2";

            using (var connection = new SqlConnection(_connectionString))
            {
                var products = await connection.QueryAsync<trip_hot_home>(procedureName, commandType: CommandType.StoredProcedure);
                return Ok(products);
            }
        }

        [HttpGet("TripHot3")]
        public async Task<ActionResult<IEnumerable<trip_hot_home>>> TripHot3()
        {
            var procedureName = "sp_view_trip_hot_3";

            using (var connection = new SqlConnection(_connectionString))
            {
                var products = await connection.QueryAsync<trip_hot_home>(procedureName, commandType: CommandType.StoredProcedure);
                return Ok(products);
            }
        }

        [HttpGet("VoucherHome")]
        public async Task<ActionResult<IEnumerable<banner>>> VoucherHome()
        {
            var procedureName = "sp_view_voucher_home";

            using (var connection = new SqlConnection(_connectionString))
            {
                var products = await connection.QueryAsync<banner>(procedureName, commandType: CommandType.StoredProcedure);
                return Ok(products);
            }
        }

        [HttpGet("DriverHome")]
        public async Task<ActionResult<IEnumerable<driver_home>>> DriverHome()
        {
            var procedureName = "sp_view_driver_home";

            using (var connection = new SqlConnection(_connectionString))
            {
                var products = await connection.QueryAsync<driver_home>(procedureName, commandType: CommandType.StoredProcedure);
                return Ok(products);
            }
        }
    }
}
