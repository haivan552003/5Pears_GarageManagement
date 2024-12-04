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

        [HttpGet("checkCarNumberExists/{carNumber}")]
        public async Task<IActionResult> CheckCarNumberExists(string carNumber)
        {
            try
            {
                var procedureName = "sp_check_car_number_exists";
                var parameters = new DynamicParameters();
                parameters.Add("carNumber", carNumber, DbType.String, ParameterDirection.Input);

                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var carExists = await connection.QueryFirstOrDefaultAsync<bool>(
                        procedureName,
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    return Ok(new { exists = carExists });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi kiểm tra biển số xe", error = ex.Message });
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
            try
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
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("carNumber/{carNumber}")]
        public async Task<ActionResult<car>> GetCarsByCarNumber(string carNumber)
        {
            try
            {
                var procedureName = "sp_get_by_carNumber_cars";
                var parameters = new DynamicParameters();
                parameters.Add("carNumber", carNumber, DbType.String, ParameterDirection.Input);

                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var car = await connection.QueryFirstOrDefaultAsync<car>(
                        procedureName, parameters, commandType: CommandType.StoredProcedure);

                    if (car == null)
                    {
                        return NotFound();
                    }
                    return Ok(car);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }


        [HttpGet("getCarSeats/{carId}")]
        public async Task<ActionResult<IEnumerable<car_seat>>> GetCarSeatsByCarId(int carId)
        {
            try
            {
                var procedureName = "sp_get_by_id_car_seat";
                var parameters = new DynamicParameters();
                parameters.Add("car_id", carId, DbType.Int32, ParameterDirection.Input);

                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var carSeats = await connection.QueryAsync<car_seat>(
                        procedureName, parameters, commandType: CommandType.StoredProcedure);

                    if (!carSeats.Any())
                    {
                        return NotFound();
                    }
                    return Ok(carSeats);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
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
                cmd.Parameters.AddWithValue("@vehical_registration_start", cars.vehicle_registration_start);
                cmd.Parameters.AddWithValue("@vehical_registration_end", cars.vehicle_registration_end);
                cmd.Parameters.AddWithValue("@price", cars.price);
                cmd.Parameters.AddWithValue("@isAuto", cars.isAuto);
                cmd.Parameters.AddWithValue("@status", cars.status);
                cmd.Parameters.AddWithValue("@id_type", cars.type_id);
                cmd.Parameters.AddWithValue("@id_brand", cars.brand_id);
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
                cmd.Parameters.AddWithValue("@car_id", cs.car_id);
                cmd.Parameters.AddWithValue("@row", cs.row);
                cmd.Parameters.AddWithValue("@col", cs.col);
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
                cmd.Parameters.AddWithValue("@vehical_registration_start", cars.vehicle_registration_start);
                cmd.Parameters.AddWithValue("@vehical_registration_end", cars.vehicle_registration_end);
                cmd.Parameters.AddWithValue("@price", cars.price);
                cmd.Parameters.AddWithValue("@isAuto", cars.isAuto);
                cmd.Parameters.AddWithValue("@status", cars.status);
                cmd.Parameters.AddWithValue("@id_type", cars.type_id);
                cmd.Parameters.AddWithValue("@id_brand", cars.brand_id);
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
                cmd.Parameters.AddWithValue("@row", cs.row);
                cmd.Parameters.AddWithValue("@col", cs.col);
                await conn.OpenAsync();
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
                cmd.Parameters.AddWithValue("@id", id);
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
                cmd.Parameters.AddWithValue("@id", id);
                await conn.OpenAsync();

                int rowsAffected = await cmd.ExecuteNonQueryAsync();

                if (rowsAffected == 0)
                {
                    return NotFound();
                }
            }

            return NoContent();
        }
        [HttpGet("getAllCarImages")]
        public async Task<ActionResult<IEnumerable<car_img>>> GetAllCarImages()
        {
            var procedureName = "sp_getAll_car_img";

            using (var connection = new SqlConnection(_connectionString))
            {
                var carImages = await connection.QueryAsync<car_img>(
                    procedureName,
                    commandType: CommandType.StoredProcedure
                );
                return Ok(carImages);
            }
        }

        [HttpGet("GetImgById/{carId}")]
        public async Task<ActionResult<car_img>> GetCarImageById(int carId)
        {
            try
            {
                var procedureName = "sp_getById_car_img";
                var parameters = new DynamicParameters();
                parameters.Add("car_id", carId, DbType.Int32, ParameterDirection.Input);

                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var carSeats = await connection.QueryAsync<car_img>(
                        procedureName, parameters, commandType: CommandType.StoredProcedure);

                    if (!carSeats.Any())
                    {
                        return NotFound();
                    }
                    return Ok(carSeats);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPost("CreateImg")]
        public async Task<ActionResult<car_img>> CreateCarImage(car_img carImage)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("sp_create_car_img", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Name", carImage.name);
                    cmd.Parameters.AddWithValue("@CarId", carImage.car_id);

                    await conn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }

                return CreatedAtAction(nameof(GetAllCarImages), new { id = carImage.id }, carImage);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPut("updateImg/{id}")]
        public async Task<IActionResult> UpdateCarImage(int id, car_img carImage)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("sp_update_car_img", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@Name", carImage.name);
                    await conn.OpenAsync();
                    int rowsAffected = await cmd.ExecuteNonQueryAsync();

                    if (rowsAffected == 0)
                    {
                        return NotFound();
                    }
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpDelete("deleteImg/{id}")]
        public async Task<IActionResult> DeleteCarImage(int id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("sp_delete_car_img", conn);
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
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [HttpGet("viewCar/{id}")]
        public async Task<ActionResult<car>> sp_GetTripsID(int id)
        {
            var procedureName = "sp_view_by_id_cars";
            var parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Int32, ParameterDirection.Input);

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var multi = await connection.QueryMultipleAsync(procedureName, parameters, commandType: CommandType.StoredProcedure))
                {
                    var car = multi.ReadFirstOrDefault<car>();
                    if (car == null)
                    {
                        return NotFound();
                    }

                    var carImages = multi.Read<car_img>().ToList();
                    car.car_img = carImages;

                    return Ok(car);
                }
            }
        }


    }
}
