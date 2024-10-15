﻿using BE_API.Models;
using Dapper;
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
    public class CarsController : ControllerBase
    {
        private readonly string _connectionString;
        public CarsController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SqlConnection");
        }

        [HttpGet("getAllCars")]
        public async Task<ActionResult<IEnumerable<car>>> GetAllCars()
        {
            var procedureName = "sp_getall_cars";

            using (var connection = new SqlConnection(_connectionString))
            {
                var cars = await connection.QueryAsync<car>(procedureName, commandType: CommandType.StoredProcedure);
                return Ok(cars);
            }

        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<car_seat>>> Get_all_car_seats()
        {
            var procedureName = "sp_getall_car_seat";

            using (var connection = new SqlConnection(_connectionString))
            {
                var car_seats = await connection.QueryAsync<car_seat>(procedureName, commandType: CommandType.StoredProcedure);
                return Ok(car_seats);
            }

        }
        [HttpGet("{id}")]
        public async Task<ActionResult<car>> Get_cars_by_id(int id)
        {
            var procedureName = "sp_get_by_id_cars";
            var parameters = new DynamicParameters();
            parameters.Add("id", id, DbType.Int32, ParameterDirection.Input);

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var cars = await connection.QueryFirstOrDefaultAsync<car>(
                    procedureName, parameters, commandType: CommandType.StoredProcedure);

                if (cars == null)
                {
                    return NotFound();
                }

                return Ok(cars);
            }
        }
        [HttpGet("getCarSeats/{id}")]
        public async Task<ActionResult<car_seat>> Get_car_seats_by_id(int id)
        {
            var procedureName = "sp_get_by_id_car_seat";
            var parameters = new DynamicParameters();
            parameters.Add("id", id, DbType.Int32, ParameterDirection.Input);

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var car_seats= await connection.QueryFirstOrDefaultAsync<car_seat>(
                    procedureName, parameters, commandType: CommandType.StoredProcedure);

                if (car_seats == null)
                {
                    return NotFound();
                }

                return Ok(car_seats);
            }
        }
        [HttpPost("PostCars")]

        public async Task<ActionResult<car>> PostCars(car cars)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_create_cars", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@car_number", cars.car_number);
                cmd.Parameters.AddWithValue("@color", cars.color);
                cmd.Parameters.AddWithValue("@vehicle_registration_start", cars.vehicle_registration_start);
                cmd.Parameters.AddWithValue("@vehicle_registration_end", cars.vehicle_registration_end);
                cmd.Parameters.AddWithValue("@price", cars.price);
                cmd.Parameters.AddWithValue("@isAuto", cars.is_auto);
                cmd.Parameters.AddWithValue("@status", cars.status);
                cmd.Parameters.AddWithValue("@id_type", cars.id_type);
                cmd.Parameters.AddWithValue("@id_brand", cars.id_brand);
                cmd.Parameters.AddWithValue("@year_production", cars.year_production);
                cmd.Parameters.AddWithValue("@odo", cars.odo);
                cmd.Parameters.AddWithValue("@insurance_fee", cars.insurance_fee);
                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }

            return CreatedAtAction(nameof(GetAllCars), new { id = cars.id }, cars);
        }
        [HttpPost("PostCarSeat")]
        public async Task<ActionResult<car>> PostCarSeat(car_seat cs)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_create_car_seats", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@name", cs.name);
                cmd.Parameters.AddWithValue("@car_id", cs.id_car);
                cmd.Parameters.AddWithValue("@row", cs.row);
                cmd.Parameters.AddWithValue("@col", cs.col);
                cmd.Parameters.AddWithValue("@status", cs.status);             
                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }

            return CreatedAtAction(nameof(GetAllCars), new { id = cs.id }, cs);
        }
        [HttpPut("putCars/{id}")]
        public async Task<IActionResult> PutCars(int id, car cars)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_update_car", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@car_number", cars.car_number);
                cmd.Parameters.AddWithValue("@color", cars.color);
                cmd.Parameters.AddWithValue("@vehicle_registration_start", cars.vehicle_registration_start);
                cmd.Parameters.AddWithValue("@vehicle_registration_end", cars.vehicle_registration_end);
                cmd.Parameters.AddWithValue("@price", cars.price);
                cmd.Parameters.AddWithValue("@isAuto", cars.is_auto);
                cmd.Parameters.AddWithValue("@status", cars.status);
                cmd.Parameters.AddWithValue("@id_type", cars.id_type);
                cmd.Parameters.AddWithValue("@id_brand", cars.id_brand);
                cmd.Parameters.AddWithValue("@year_production", cars.year_production);
                cmd.Parameters.AddWithValue("@odo", cars.odo);
                cmd.Parameters.AddWithValue("@insurance_fee", cars.insurance_fee);
                await conn.OpenAsync();

                int rowsAffected = await cmd.ExecuteNonQueryAsync();

                if (rowsAffected == 0)
                {
                    return NotFound();
                }
            }

            return NoContent();
        }
        [HttpPut("putCarSeat/{id}")]
        public async Task<IActionResult> PutCarSeat(int id, car_seat cs)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_update_car_seats", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@name", cs.name);
                cmd.Parameters.AddWithValue("@car_id", cs.id_car);
                cmd.Parameters.AddWithValue("@row", cs.row);
                cmd.Parameters.AddWithValue("@col", cs.col);
                cmd.Parameters.AddWithValue("@status", cs.status);
                int rowsAffected = await cmd.ExecuteNonQueryAsync();

                if (rowsAffected == 0)
                {
                    return NotFound();
                }
            }

            return NoContent();
        }
        [HttpDelete("DeleteCars/{id}")]
        public async Task<IActionResult> DeleteCars(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_delete_car", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                await conn.OpenAsync();

                int rowsAffected = await cmd.ExecuteNonQueryAsync();

                if (rowsAffected == 0)
                {
                    return NotFound();
                }
            }

            return NoContent();
        }
        [HttpDelete("DeleteCarSeats/{id}")]
        public async Task<IActionResult> DeleteCarSeats(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_delete_car_seat", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                await conn.OpenAsync();

                int rowsAffected = await cmd.ExecuteNonQueryAsync();

                if (rowsAffected == 0)
                {
                    return NotFound();
                }
            }

            return NoContent();
        }
    }
}