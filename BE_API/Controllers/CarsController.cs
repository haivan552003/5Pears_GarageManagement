using BE_API.Models;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
        public async Task<ActionResult<car_create>> PostCars(car_create cars)
        {
            int carId;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_create_cars", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                // Existing car parameters
                cmd.Parameters.AddWithValue("@car_number", cars.car_number);
                cmd.Parameters.AddWithValue("@color", cars.color);
                cmd.Parameters.AddWithValue("@v_registration_start", cars.vehicle_registration_start);
                cmd.Parameters.AddWithValue("@v_registration_end", cars.vehicle_registration_end);
                cmd.Parameters.AddWithValue("@price", cars.price);
                cmd.Parameters.AddWithValue("@isAuto", cars.isAuto);
                cmd.Parameters.AddWithValue("@is_retail", cars.isRetail);
                cmd.Parameters.AddWithValue("@type_id", cars.type_id);
                cmd.Parameters.AddWithValue("@brand_id", cars.brand_id);
                cmd.Parameters.AddWithValue("@year_production", cars.year_production);
                cmd.Parameters.AddWithValue("@odo", cars.odo);
                cmd.Parameters.AddWithValue("@insurance_fee", cars.insurance_fee);
                cmd.Parameters.AddWithValue("@fuel", cars.fuel);
                cmd.Parameters.AddWithValue("@description", cars.description);
                cmd.Parameters.AddWithValue("@car_name", cars.car_name);
                cmd.Parameters.AddWithValue("@voucher", cars.voucher);
                cmd.Parameters.AddWithValue("@number_seat", cars.number_seat);

                SqlParameter carIdParam = new SqlParameter("@car_id", SqlDbType.Int);
                carIdParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(carIdParam);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                carId = Convert.ToInt32(carIdParam.Value);
            }

            if (cars.number_seat > 0)
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    // Create driver's seat
                    SqlCommand driverSeatCmd = new SqlCommand("sp_create_car_seats", conn);
                    driverSeatCmd.CommandType = CommandType.StoredProcedure;
                    driverSeatCmd.Parameters.AddWithValue("@name", "TÀI XẾ");
                    driverSeatCmd.Parameters.AddWithValue("@car_id", carId);
                    driverSeatCmd.Parameters.AddWithValue("@row", 1);
                    driverSeatCmd.Parameters.AddWithValue("@col", 1);
                    await driverSeatCmd.ExecuteNonQueryAsync();

                    // Create additional seats
                    for (int i = 1; i <= cars.number_seat - 1; i++)
                    {
                        SqlCommand seatCmd = new SqlCommand("sp_create_car_seats", conn);
                        seatCmd.CommandType = CommandType.StoredProcedure;

                        string seatName = GetSeatName(cars.number_seat, i);

                        seatCmd.Parameters.AddWithValue("@name", seatName);
                        seatCmd.Parameters.AddWithValue("@car_id", carId);
                        seatCmd.Parameters.AddWithValue("@row",  1);
                        seatCmd.Parameters.AddWithValue("@col", 1);
                        await seatCmd.ExecuteNonQueryAsync();
                    }
                }
            }

            return CreatedAtAction(nameof(GetAllCars), new { id = carId }, cars);
        }

        // Helper method to generate seat names
        private string GetSeatName(int totalSeats, int seatNumber)
        {
            char prefix = totalSeats switch
            {
                16 => 'A',
                24 => 'B',
                36 => 'C',
                45 => 'D',
                _ => 'X'
            };
            return $"{prefix}{seatNumber}";
        }

      
        [HttpPost("PostCarSeat")]
        public async Task<ActionResult<IEnumerable<car_seat>>> PostCarSeat(List<car_seat> seats)
        {
            List<car_seat> createdSeats = new List<car_seat>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                foreach (var seat in seats)
                {
                    SqlCommand cmd = new SqlCommand("sp_create_car_seats", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@name", seat.name);
                    cmd.Parameters.AddWithValue("@car_id", seat.car_id);
                    cmd.Parameters.AddWithValue("@row", seat.row);
                    cmd.Parameters.AddWithValue("@col", seat.col);

                    int seatId = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                    seat.id = seatId;

                    createdSeats.Add(seat);
                }
            }

            return CreatedAtAction(nameof(GetAllCars), createdSeats);
        }
        [HttpPut("putCars/{id}")]
        public async Task<IActionResult> PutCars(int id, car_create cars)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_update_car", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@car_number", cars.car_number);
                cmd.Parameters.AddWithValue("@color", cars.color);
                cmd.Parameters.AddWithValue("@v_registration_start", cars.vehicle_registration_start);
                cmd.Parameters.AddWithValue("@v_registration_end", cars.vehicle_registration_end);
                cmd.Parameters.AddWithValue("@price", cars.price);
                cmd.Parameters.AddWithValue("@isAuto", cars.isAuto);
                cmd.Parameters.AddWithValue("@isRetail", cars.isRetail);
                cmd.Parameters.AddWithValue("@type_id", cars.type_id);
                cmd.Parameters.AddWithValue("@brand_id", cars.brand_id);
                cmd.Parameters.AddWithValue("@year_production", cars.year_production);
                cmd.Parameters.AddWithValue("@odo", cars.odo);
                cmd.Parameters.AddWithValue("@insurance_fee", cars.insurance_fee);
                cmd.Parameters.AddWithValue("@fuel", cars.fuel);
                //cmd.Parameters.AddWithValue("@location_car", cars.location_car);
                cmd.Parameters.AddWithValue("@description", cars.description);
                cmd.Parameters.AddWithValue("@car_name", cars.car_name);
                cmd.Parameters.AddWithValue("@voucher", cars.voucher);
                cmd.Parameters.AddWithValue("@number_seat", cars.number_seat);

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
