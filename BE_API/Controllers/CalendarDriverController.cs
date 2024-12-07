using BE_API.ModelCustom;
using BE_API.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BE_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalendarDriverController : ControllerBase
    {
        private readonly string _connectionString;
        public CalendarDriverController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SqlConnection");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<calendarDriver>>> GetDriverCalendar(int id_driver)
        {
            try
            {
                var procedureName = "sp_calendar_driver";
                var parameters = new DynamicParameters();
                parameters.Add("id_driver", id_driver, DbType.Int32, ParameterDirection.Input);

                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var calendar = await connection.QueryAsync<calendarDriver>(
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
        [HttpGet("guest-driver-calendar")]

        public async Task<ActionResult<IEnumerable<calendarDriver>>> GetDriverCalendarGD(int id_driver)
        {
            try
            {
                var procedureName = "sp_calendar_driver_gd";
                var parameters = new DynamicParameters();
                parameters.Add("id_driver", id_driver, DbType.Int32, ParameterDirection.Input);

                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var calendar = await connection.QueryAsync<calendarDriver>(
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
        [HttpGet("guest-car-driver-calendar")]
        public async Task<ActionResult<IEnumerable<calendarDriver>>> GetDriverCalendarGCD(int id_driver)
        {
            try
            {
                var procedureName = "sp_calendar_driver_gcd";
                var parameters = new DynamicParameters();
                parameters.Add("id_driver", id_driver, DbType.Int32, ParameterDirection.Input);

                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var calendar = await connection.QueryAsync<calendarDriver>(
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
        [HttpGet("dropdown-drivers")]
        public async Task<ActionResult<IEnumerable<DriverDropdown>>> GetDriverDropdown()
        {
            try
            {
                var procedureName = "sp_dropdown_driver";

                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var drivers = await connection.QueryAsync<DriverDropdown>(
                        procedureName, commandType: CommandType.StoredProcedure);

                    if (!drivers.Any())
                    {
                        return NotFound();
                    }
                    return Ok(drivers);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

      
    }
}
