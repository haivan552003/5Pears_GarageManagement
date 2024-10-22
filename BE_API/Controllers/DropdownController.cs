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
    public class DropdownController : ControllerBase
    {
        private readonly string _connectionString;
        public DropdownController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SqlConnection");
        }

        [HttpGet("emp")]
        public async Task<ActionResult<IEnumerable<dropdown>>> DropdownUser()
        {
            var procedureName = "sp_dropdown_user";

            using (var connection = new SqlConnection(_connectionString))
            {
                var dropdown = await connection.QueryAsync<dropdown>(procedureName, commandType: CommandType.StoredProcedure);
                return Ok(dropdown);
            }

        }
        [HttpGet("customer")]
        public async Task<ActionResult<IEnumerable<dropdown>>> DropdownCustomer()
        {
            var procedureName = "sp_dropdown_customer";

            using (var connection = new SqlConnection(_connectionString))
            {
                var dropdown = await connection.QueryAsync<dropdown>(procedureName, commandType: CommandType.StoredProcedure);
                return Ok(dropdown);
            }

        }
        [HttpGet("driver")]
        public async Task<ActionResult<IEnumerable<dropdown>>> DropdownDriver()
        {
            var procedureName = "sp_dropdown_driver";

            using (var connection = new SqlConnection(_connectionString))
            {
                var dropdown = await connection.QueryAsync<dropdown>(procedureName, commandType: CommandType.StoredProcedure);
                return Ok(dropdown);
            }

        }
        [HttpGet("location")]
        public async Task<ActionResult<IEnumerable<dropdown>>> DropdownLocation()
        {
            var procedureName = "sp_dropdown_location";

            using (var connection = new SqlConnection(_connectionString))
            {
                var dropdown = await connection.QueryAsync<dropdown>(procedureName, commandType: CommandType.StoredProcedure);
                return Ok(dropdown);
            }

        }
        [HttpGet("car")]
        public async Task<ActionResult<IEnumerable<dropdown>>> DropdownCar()
        {
            var procedureName = "sp_dropdown_car";

            using (var connection = new SqlConnection(_connectionString))
            {
                var dropdown = await connection.QueryAsync<dropdown>(procedureName, commandType: CommandType.StoredProcedure);
                return Ok(dropdown);
            }

        }
        [HttpGet("trip")]
        public async Task<ActionResult<IEnumerable<dropdown>>> DropdownTrip()
        {
            var procedureName = "sp_dropdown_trip";

            using (var connection = new SqlConnection(_connectionString))
            {
                var dropdown = await connection.QueryAsync<dropdown>(procedureName, commandType: CommandType.StoredProcedure);
                return Ok(dropdown);
            }

        }
        [HttpGet("tripdetail")]
        public async Task<ActionResult<IEnumerable<dropdown>>> DropdownTripDetail()
        {
            var procedureName = "sp_dropdown_tripdetail";

            using (var connection = new SqlConnection(_connectionString))
            {
                var dropdown = await connection.QueryAsync<dropdown>(procedureName, commandType: CommandType.StoredProcedure);
                return Ok(dropdown);
            }

        }

        [HttpGet("carseat")]
        public async Task<ActionResult<IEnumerable<dropdown>>> DropdownCarseat(int car_id)
        {
            var procedureName = "sp_dropdown_carseat";
            var parameters = new DynamicParameters();
            parameters.Add("car_id", car_id, DbType.Int32, ParameterDirection.Input);

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var product = await connection.QueryFirstOrDefaultAsync<dropdown>(
                    procedureName, parameters, commandType: CommandType.StoredProcedure);

                if (product == null)
                {
                    return NotFound();
                }

                return Ok(product);
            }

        }

        [HttpGet("role")]
        public async Task<ActionResult<IEnumerable<dropdown>>> DropdownRole()
        {
            var procedureName = "sp_dropdown_role";

            using (var connection = new SqlConnection(_connectionString))
            {
                var dropdown = await connection.QueryAsync<dropdown>(procedureName, commandType: CommandType.StoredProcedure);
                return Ok(dropdown);
            }
        }
    }
}
