﻿using BE_API.Models;
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
        public async Task<ActionResult> AddTrip([FromBody] trip newTrip)
        {
            if (newTrip == null || string.IsNullOrEmpty(newTrip.img_trip) ||
                string.IsNullOrEmpty(newTrip.from) || string.IsNullOrEmpty(newTrip.to) ||
                string.IsNullOrEmpty(newTrip.trip_code))
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var parameters = new DynamicParameters();
            parameters.Add("@img_trip", newTrip.img_trip);
            parameters.Add("@from", newTrip.from);
            parameters.Add("@to", newTrip.to);
            parameters.Add("@emp_create", newTrip.emp_create);
            parameters.Add("@trip_code", newTrip.trip_code);
            parameters.Add("@status", newTrip.status);
            parameters.Add("@is_return", newTrip.is_return);

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var result = await connection.ExecuteAsync("sp_add_trip", parameters, commandType: CommandType.StoredProcedure);

                if (result > 0)
                {
                    return Ok(new { message = "Chuyến đi đã được thêm thành công" });
                }

                return BadRequest(new { message = "Thêm chuyến đi không thành công" });
            }
        }


        [HttpPut("trip/{id}")]
        public async Task<IActionResult> UpdateTrip(int id, trip_create trip)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_update_trip", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@img_trip", trip.img_trip);
                cmd.Parameters.AddWithValue("@from", trip.from);
                cmd.Parameters.AddWithValue("@to", trip.to);
                cmd.Parameters.AddWithValue("@trip_code", trip.trip_code);
                cmd.Parameters.AddWithValue("@status", trip.status);
                cmd.Parameters.AddWithValue("@is_return", trip.is_return);

                await conn.OpenAsync();

                int rowsAffected = await cmd.ExecuteNonQueryAsync();

                if (rowsAffected == 0)
                {
                    return NotFound();
                }
            }

            return NoContent();
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


        // POST: api/trips
        [HttpPost("tripdetail")]
        public async Task<ActionResult> AddTripDetail([FromBody] trip_detail newTrip)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@time_start", newTrip.time_start);
            parameters.Add("@time_end", newTrip.time_end);
            parameters.Add("@price", newTrip.price);
            parameters.Add("@voucher", newTrip.voucher);
            parameters.Add("@trip_id", newTrip.trip_id);
            parameters.Add("@car_id", newTrip.car_id);
            parameters.Add("@location_from_id", newTrip.location_from_id);
            parameters.Add("@driver_id", newTrip.driver_id);
            parameters.Add("@location_to_id", newTrip.location_to_id);
            parameters.Add("@trip_detail_code", newTrip.trip_detail_code);
            parameters.Add("@distance", newTrip.distance);
            parameters.Add("@status", newTrip.status);

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
        public async Task<IActionResult> UpdateTripDetail(int id, trip_detail trip)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_update_trip_detail", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@time_start", trip.time_start);
                cmd.Parameters.AddWithValue("@time_end", trip.time_end);
                cmd.Parameters.AddWithValue("@price", trip.price);
                cmd.Parameters.AddWithValue("@voucher", trip.voucher);
                cmd.Parameters.AddWithValue("@car_id", trip.car_id);
                cmd.Parameters.AddWithValue("@location_from_id", trip.location_from_id);
                cmd.Parameters.AddWithValue("@driver_id", trip.driver_id);
                cmd.Parameters.AddWithValue("@location_to_id", trip.location_to_id);
                cmd.Parameters.AddWithValue("@trip_detail_code", trip.trip_detail_code);
                cmd.Parameters.AddWithValue("@distance", trip.distance);
                cmd.Parameters.AddWithValue("@status", trip.status);

                await conn.OpenAsync();

                int rowsAffected = await cmd.ExecuteNonQueryAsync();

                if (rowsAffected == 0)
                {
                    return NotFound();
                }
            }

            return NoContent();
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
    }
}
