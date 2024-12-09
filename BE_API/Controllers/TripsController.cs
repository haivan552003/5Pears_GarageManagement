using BE_API.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace BE_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripsController : Controller
    {
        private readonly string _connectionString;
        public TripsController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SqlConnection");
        }
        [HttpGet("trip")]
        public async Task<ActionResult<IEnumerable<trip>>> sp_Get_Trips()
        {
            var procedureName = "sp_get_trip";

            using (var connection = new SqlConnection(_connectionString))
            {
                var employeesList = await connection.QueryAsync<trip>(procedureName, commandType: CommandType.StoredProcedure);
                return Ok(employeesList);
            }
        }
        // GET: api/Products/5

        [HttpGet("trip/{id}")]
        public async Task<ActionResult<trip>> sp_GetTripsID(int id)
        {
            var procedureName = "sp_getid_trip";
            var parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Int32, ParameterDirection.Input);

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var multi = await connection.QueryMultipleAsync(procedureName, parameters, commandType: CommandType.StoredProcedure))
                {
                    var trip = multi.ReadFirstOrDefault<trip>();

                    if (trip == null)
                    {
                        return NotFound();
                    }

                    var tripDetails = multi.Read<trip_detail>().ToList();

                    trip.TripDetails = tripDetails;

                    return Ok(trip);
                }
            }
        }


        // POST: api/trips
        [HttpPost("trip")]
        public async Task<ActionResult> AddEmployees(trip_create newEmployee)
        {
            try
            {
                var parameters = new DynamicParameters(newEmployee);
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var result = await connection.ExecuteAsync("sp_add_trip", parameters, commandType: CommandType.StoredProcedure);

                    if (result > 0)
                    {
                        return Ok();
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi: {ex.Message}");
            }
        }


        [HttpPut("trip/{id}")]
        public async Task<IActionResult> UpdateTrip(int id, trip_update trip)
        {
            var parameters = new DynamicParameters(trip);
            parameters.Add("@id", id);

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                var result = await conn.ExecuteAsync("sp_update_trip", parameters, commandType: CommandType.StoredProcedure);

                if (result > 0)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
        }
        [HttpDelete("trip/{id}")]
        public async Task<IActionResult> DeleteTrip(int id)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var parameters = new DynamicParameters();
                    parameters.Add("@id", id, DbType.Int32, ParameterDirection.Input); // Tham số ID cho stored procedure

                    var result = await connection.ExecuteAsync(
                        "sp_delete_trips",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    if (result > 0)
                    {
                        return Ok();
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error occurred: {ex.Message}" });
            }
        }

        // GET: api/Products/5


        [HttpGet("tripDetails")]
        public async Task<ActionResult<IEnumerable<trip_detail>>> sp_Get_Trips_details()
        {
            var procedureName = "sp_getall_tripdetail";

            using (var connection = new SqlConnection(_connectionString))
            {
                var employeesList = await connection.QueryAsync<trip_detail>(procedureName, commandType: CommandType.StoredProcedure);
                return Ok(employeesList);
            }
        }


        [HttpGet("tripdetail/{id}")]
        public async Task<ActionResult<trip_detail>> GetTripDetailID(int id)
        {
            var procedureName = "sp_getid_tripdetail";
            var parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Int32, ParameterDirection.Input);

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var product = await connection.QueryFirstOrDefaultAsync<trip_detail>(
                    procedureName, parameters, commandType: CommandType.StoredProcedure);

                if (product == null)
                {
                    return NotFound();
                }

                return Ok(product);
            }
        }

        [HttpGet("tripdetail_by_tripid/{id}")]
        public async Task<ActionResult<IEnumerable<trip_detail>>> GetTripDetailIDany(int id)
        {
            var procedureName = "[dbo].[sp_view_tripdetail_by_tripid]";
            var parameters = new { id };

            using (var connection = new SqlConnection(_connectionString))
            {
                var tripDetails = await connection.QueryAsync<trip_detail>(
                    procedureName,
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                if (!tripDetails.Any())
                {
                    return NotFound();
                }

                return Ok(tripDetails);
            }
        }



        // POST: api/trips
        [HttpPost("tripdetail")]
        public async Task<ActionResult> AddTripDetail(trip_detail_create newTrip)
        {
            var parameters = new DynamicParameters(newTrip);

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var result = await connection.ExecuteAsync("sp_add_trip_detail", parameters, commandType: CommandType.StoredProcedure);

                if (result > 0)
                {
                    return Ok();
                }

                return BadRequest();
            }
        }


        [HttpPut("tripdetail/{id}")]
        public async Task<IActionResult> UpdateTripDetail(int id, trip_detail_update trip)
        {
            var parameters = new DynamicParameters(trip);
            parameters.Add("@id", id);

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                var result = await conn.ExecuteAsync("sp_update_trip_detail", parameters, commandType: CommandType.StoredProcedure);

                if (result > 0)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
        }

        [HttpDelete("tripdetail/{id}")]
        public async Task<IActionResult> DeleteTripDetail(int id)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var parameters = new DynamicParameters();
                    parameters.Add("@id", id, DbType.Int32, ParameterDirection.Input);

                    var result = await connection.ExecuteAsync(
                        "sp_delete_trip_detail",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    if (result > 0)
                    {
                        return Ok();
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error occurred: {ex.Message}" });
            }
        }


        [HttpGet("view_tripdetail/{id}")]
        public async Task<ActionResult<IEnumerable<trip_detail>>> ViewTripDetailID(int id)
        {
            var procedureName = "[dbo].[sp_view_tripdetail]";
            var parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Int32, ParameterDirection.Input);

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var multi = await connection.QueryMultipleAsync(procedureName, parameters, commandType: CommandType.StoredProcedure))
                {
                    var trip_detail = multi.ReadFirstOrDefault<ModelCustom.trip_detail>();

                    if (trip_detail == null)
                    {
                        return NotFound();
                    }

                    var car_seat = multi.Read<ModelCustom.car_seat>().ToList();

                    trip_detail.car_seats = car_seat;

                    return Ok(trip_detail);
                }
            }
        }

        [HttpGet("viewTrip/{id}")]
        public async Task<ActionResult<trip_detail>> sp_GetTripsDetailsID(int id)
        {
            var procedureName = "sp_get_view_trip_deatails_by_id";
            var parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Int32, ParameterDirection.Input);

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var multi = await connection.QueryMultipleAsync(procedureName, parameters, commandType: CommandType.StoredProcedure))
                {
                    var car = multi.ReadFirstOrDefault<trip_detail>();
                    if (car == null)
                    {
                        return NotFound();
                    }

                    var carImages = multi.Read<car_seat>().ToList();
                    car.car_seat = carImages;

                    return Ok(car);
                }
            }
        }

        [HttpPost("serchtrip")]
        public async Task<ActionResult<IEnumerable<search_tripdetail>>> SearchTrips(search_trip request)
        {
            var procedureName = "[dbo].[sp_search_trip]";

            var parameters = new DynamicParameters();
            parameters.Add("@location_from", request.location_from);
            parameters.Add("@location_to", request.location_to);
            parameters.Add("@date_start", request.date_start?.Date);
            parameters.Add("@date_return", request.date_return?.Date);
            parameters.Add("@is_return", request.is_return);

            using (var connection = new SqlConnection(_connectionString))
            {
                var tripDetails = await connection.QueryAsync<search_tripdetail>(
                    procedureName,
                    parameters,  
                    commandType: CommandType.StoredProcedure
                );

                return Ok(tripDetails);
            }
        }

    }
}
