using BE_API.ModelCustom;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System;
using System.Linq;
using BE_API.Models;

namespace BE_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalendarCarsController : ControllerBase
    {
        private readonly string _connectionString;
        public CalendarCarsController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SqlConnection");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<carlendarCar>>> GetDriverCalendar(int id_car)
        {
            try
            {
                var procedureName = "sp_calendar_car";
                var parameters = new DynamicParameters();
                parameters.Add("id_car", id_car, DbType.Int32, ParameterDirection.Input);

                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var calendar = await connection.QueryAsync<carlendarCar>(
                        procedureName, parameters, commandType: CommandType.StoredProcedure);

                    if (!calendar.Any())
                    {
                        return NotFound();
                    }
                    return Ok(calendar);
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [HttpGet("calendar_gc")]
        public async Task<ActionResult<IEnumerable<carlendarCar>>> GetDriverCalendar_gc(int id_car)
        {
            try
            {
                var procedureName = "sp_calendar_car_gc";
                var parameters = new DynamicParameters();
                parameters.Add("id_car", id_car, DbType.Int32, ParameterDirection.Input);

                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var calendar = await connection.QueryAsync<carlendarCar>(
                        procedureName, parameters, commandType: CommandType.StoredProcedure);

                    if (!calendar.Any())
                    {
                        return NotFound();
                    }
                    return Ok(calendar);
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [HttpGet("calendar_gcd")]
        public async Task<ActionResult<IEnumerable<carlendarCar>>> GetDriverCalendar_gcd(int id_car)
        {
            try
            {
                var procedureName = "sp_calendar_car_gcd";
                var parameters = new DynamicParameters();
                parameters.Add("id_car", id_car, DbType.Int32, ParameterDirection.Input);

                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var calendar = await connection.QueryAsync<carlendarCar>(
                        procedureName, parameters, commandType: CommandType.StoredProcedure);

                    if (!calendar.Any())
                    {
                        return NotFound();
                    }
                    return Ok(calendar);
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
