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

        [HttpGet("CarNumber/{carNumber}")]
        public async Task<ActionResult<ModelCustom.TripCarLocation>> ViewTripDetailByCarNumber(string carNumber)
        {
            var procedureName = "[dbo].[sp_get_by_carNumber_cars]";
            var parameters = new DynamicParameters();
            parameters.Add("@carNumber", carNumber, DbType.String, ParameterDirection.Input);

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var multi = await connection.QueryMultipleAsync(procedureName, parameters, commandType: CommandType.StoredProcedure))
                {
                    var car = multi.ReadFirstOrDefault<ModelCustom.TripCarLocation>();
                    if (car == null)
                    {
                        return NotFound();
                    }

                    car.Trip_Detail_Customs = multi.Read<ModelCustom.trip_detail_custom>().ToList();
                    car.guest_Cars_Customs = multi.Read<ModelCustom.guest_cars_custom>().ToList();
                    car.guest_Car_Driver_Customs = multi.Read<ModelCustom.guest_car_driver_custom>().ToList();

                    return Ok(car);
                }
            }
        }


        [HttpPost("PostCars")]
        public async Task<ActionResult<car_create>> PostCars(car_create cars)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var existingCarCount = await connection.QueryFirstOrDefaultAsync<int>(
                    "SELECT COUNT(*) FROM dbo.cars WHERE car_number = @CarNumber AND is_delete = 0",
                    new { CarNumber = cars.car_number }
                );

                if (existingCarCount > 0)
                {
                    return BadRequest();
                }
            }

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
                cmd.Parameters.AddWithValue("@is_retail", cars.is_retail);
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
                await CreateCarSeats(carId, cars.number_seat);
            }

            return CreatedAtAction(nameof(GetAllCars), new { id = carId }, cars);
        }
        //thêm nhiều xe
        [HttpPost("PostCarsss")]
        public async Task<ActionResult<AddCarss>> PostCarsss(AddCarss cars)
        {
            List<int> createdCarIds = new List<int>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_create_multiple_cars", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@car_id", SqlDbType.Int).Value = cars.car_id;
                cmd.Parameters.Add("@quantity", SqlDbType.Int).Value = cars.quantity;

                await conn.OpenAsync();
                var reader = await cmd.ExecuteReaderAsync();

                // Lặp qua từng xe được tạo ra và lấy ID
                while (reader.Read())
                {
                    int carId = Convert.ToInt32(reader["id"]);
                    createdCarIds.Add(carId);

                    // Thêm ghế cho mỗi chiếc xe
                    if (cars.number_seat > 0)
                    {
                        await CreateCarSeats(carId, cars.number_seat);
                    }
                }
            }

            if (createdCarIds.Count > 0)
            {
                return CreatedAtAction(nameof(GetAllCars), new { ids = createdCarIds }, cars);
            }

            return BadRequest("Could not create cars.");
        }

        private async Task CreateCarSeats(int carId, int numberOfSeats)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                await CreateCarSeat(conn, carId, "TÀI XẾ", 1, 1);

                // Create additional seats
                for (int i = 1; i <= numberOfSeats; i++)
                {
                    var (row, col) = GetSeatPosition(numberOfSeats, i);
                    await CreateCarSeat(conn, carId, GetSeatName(numberOfSeats, i), row, col);
                }
            }
        }

        private async Task CreateCarSeat(SqlConnection conn, int carId, string name, int row, int col)
        {
            SqlCommand cmd = new SqlCommand("sp_create_car_seats", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@car_id", carId);
            cmd.Parameters.AddWithValue("@row", row);
            cmd.Parameters.AddWithValue("@col", col);
            await cmd.ExecuteNonQueryAsync();
        }

        private string GetSeatName(int totalSeats, int seatNumber)
        {
            char prefix = totalSeats switch
            {
                16 => 'A',
                29 => 'B',
                44 => 'D',
                _ => 'X'
            };
            return $"{prefix}{seatNumber}";
        }

        private (int row, int col) GetSeatPosition(int totalSeats, int seatNumber)
        {
            int row = 0;
            int col = 0;
            switch (totalSeats)
            {
                case 4:
                    if (seatNumber == 1)
                    {
                        row = 1;
                        col = 1;
                    }
                    else if (seatNumber == 2)
                    {
                        row = 1;
                        col = 1;
                    }
                    else if (seatNumber == 3)
                    {
                        row = 1;
                        col = 1;
                    }
                    else if (seatNumber == 4)
                    {
                        row = 1;
                        col = 1;
                    }
                    break;
                case 7:
                    if (seatNumber == 1)
                    {
                        row = 1;
                        col = 1;
                    }
                    else if (seatNumber == 2)
                    {
                        row = 1;
                        col = 1;
                    }
                    else if (seatNumber == 3)
                    {
                        row = 1;
                        col = 1;
                    }
                    else if (seatNumber == 4)
                    {
                        row = 1;
                        col = 1;
                    } else if (seatNumber == 5)
                    {
                        row = 1;
                        col = 1;
                    }
                    else if (seatNumber == 6)
                    {
                        row = 1;
                        col = 1;
                    }
                    else if (seatNumber == 8)
                    {
                        row = 1;
                        col = 1;
                    }
                    break;
                case 16:                  
                    if (seatNumber == 1)
                    {
                        row = 1;
                        col = 2;
                    }
                    else if (seatNumber == 2)
                    {
                        row = 1;
                        col = 3;
                    }
                    else if (seatNumber == 3)
                    {
                        row = 1;
                        col = 4;
                    }
                    else if (seatNumber == 4)
                    {
                        row = 2;
                        col = 1;
                    }
                    else if (seatNumber == 5)
                    {
                        row = 2;
                        col = 2;
                    }
                    else if (seatNumber == 6)
                    {
                        row = 2;
                        col = 3;
                    }
                    else if (seatNumber == 7)
                    {
                        row = 3;
                        col = 1;
                    }
                    else if (seatNumber == 8)
                    {
                        row = 3;
                        col = 2;
                    }
                    else if (seatNumber == 9)
                    {
                        row = 3;
                        col = 3;
                    }
                    else if (seatNumber == 10)
                    {
                        row = 4;
                        col = 1;
                    }
                    else if (seatNumber == 11)
                    {
                        row = 4;
                        col = 2;
                    }
                    else if (seatNumber == 12)
                    {
                        row = 4;
                        col = 3;
                    }
                    else if (seatNumber == 13)
                    {
                        row = 5;
                        col = 1;
                    }
                    else if (seatNumber == 14)
                    {
                        row = 5;
                        col = 2;
                    }
                    else if (seatNumber == 15)
                    {
                        row = 5;
                        col = 3;
                    }
                    else if (seatNumber == 16)
                    {
                        row = 5;
                        col = 4;
                    }
                    break;
                case 29:
                    if (seatNumber == 1)
                    {
                        row = 2;
                        col = 1;
                    }
                    else if (seatNumber == 2)
                    {
                        row = 2;
                        col = 2;
                    }
                    else if (seatNumber == 3)
                    {
                        row = 2;
                        col = 4;
                    }
                    else if (seatNumber == 4)
                    {
                        row = 2;
                        col = 5;
                    }
                    else if (seatNumber == 5)
                    {
                        row = 3;
                        col = 1;
                    }
                    else if (seatNumber == 6)
                    {
                        row = 3;
                        col = 2;
                    }
                    else if (seatNumber == 7)
                    {
                        row = 3;
                        col = 4;
                    }
                    else if (seatNumber == 8)
                    {
                        row = 3;
                        col = 5;
                    }
                    else if (seatNumber == 9)
                    {
                        row = 4;
                        col = 1;
                    }
                    else if (seatNumber == 10)
                    {
                        row = 4;
                        col = 2;
                    }
                    else if (seatNumber == 11)
                    {
                        row = 4;
                        col = 4;
                    }
                    else if (seatNumber == 12)
                    {
                        row = 4;
                        col = 5;
                    }
                    else if (seatNumber == 13)
                    {
                        row = 5;
                        col = 1;
                    }
                    else if (seatNumber == 14)
                    {
                        row = 5;
                        col = 2;
                    }
                    else if (seatNumber == 15)
                    {
                        row = 5;
                        col = 4;
                    }
                    else if (seatNumber == 16) 
                    {
                        row = 5;
                        col = 5;
                    }
                    else if (seatNumber == 17)
                    {
                        row = 6;
                        col = 1;
                    }
                    else if (seatNumber == 18)
                    {
                        row = 6;
                        col = 2;
                    }
                    else if (seatNumber == 19)
                    {
                        row = 6;
                        col = 4;
                    }
                    else if (seatNumber == 20)
                    {
                        row = 6;
                        col = 5;
                    }
                    else if (seatNumber == 21)
                    {
                        row = 7;
                        col = 1;
                    }
                    else if (seatNumber == 22)
                    {
                        row = 7;
                        col = 2;
                    } else if (seatNumber == 23)
                    {
                        row = 7;
                        col = 4;
                    }
                    else if (seatNumber == 24)
                    {
                        row = 7;
                        col = 5;
                    }
                    else if (seatNumber == 25)
                    {
                        row = 8;
                        col = 1;
                    }
                    else if (seatNumber == 26)
                    {
                        row = 8;
                        col = 2;
                    }
                    else if (seatNumber == 27)
                    {
                        row = 8;
                        col = 3;
                    } else if (seatNumber == 28)
                    {
                        row = 8;
                        col = 4;
                    }
                    else if (seatNumber == 29)
                    {
                        row = 8;
                        col = 5;
                    }
                    break;
                case 44:
                    if (seatNumber == 1)
                    {
                        row = 2;
                        col = 1;
                    }
                    else if (seatNumber == 2)
                    {
                        row = 2;
                        col = 1;
                    }
                    else if (seatNumber == 3)
                    {
                        row = 2;
                        col = 3;
                    }
                    else if (seatNumber == 4)
                    {
                        row = 2;
                        col = 3;
                    }
                    else if (seatNumber == 5)
                    {
                        row = 2;
                        col = 5;
                    }
                    else if (seatNumber == 6)
                    {
                        row = 2;
                        col = 5;
                    }
                    else if (seatNumber == 7)
                    {
                        row = 3;
                        col = 1;
                    }
                    else if (seatNumber == 8)
                    {
                        row = 3;
                        col = 1;
                    }
                    else if (seatNumber == 9)
                    {
                        row = 3;
                        col = 3;
                    }
                    else if (seatNumber == 10)
                    {
                        row = 3;
                        col = 3;
                    }
                    else if (seatNumber == 11)
                    {
                        row = 3;
                        col = 5;
                    }
                    else if (seatNumber == 12)
                    {
                        row = 3;
                        col = 5;
                    }
                    else if (seatNumber == 13)
                    {
                        row = 4;
                        col = 1;
                    }
                    else if (seatNumber == 14)
                    {
                        row = 4;
                        col = 1;
                    }
                    else if (seatNumber == 15)
                    {
                        row = 4;
                        col = 3;
                    }
                    else if (seatNumber == 16)
                    {
                        row = 4;
                        col = 3;
                    }
                    else if (seatNumber == 17)
                    {
                        row = 4;
                        col = 5;
                    }
                    else if (seatNumber == 18)
                    {
                        row = 4;
                        col = 5;
                    }
                    else if (seatNumber == 19)
                    {
                        row = 5;
                        col = 1;
                    }
                    else if (seatNumber == 20)
                    {
                        row = 5;
                        col = 1;
                    }
                    else if (seatNumber == 21)
                    {
                        row = 5;
                        col = 3;
                    }
                    else if (seatNumber == 22)
                    {
                        row = 5;
                        col = 3;
                    }
                    else if (seatNumber == 23)
                    {
                        row = 5;
                        col = 5;
                    }
                    else if (seatNumber == 24)
                    {
                        row = 5;
                        col = 5;
                    }
                    else if (seatNumber == 25)
                    {
                        row = 6;
                        col = 1;
                    }
                    else if (seatNumber == 26)
                    {
                        row = 6;
                        col = 1;
                    }
                    else if (seatNumber == 27)
                    {
                        row = 6;
                        col = 3;
                    }
                    else if (seatNumber == 28)
                    {
                        row = 6;
                        col = 3;
                    }
                    else if (seatNumber == 29) 
                    {
                        row = 6;
                        col = 5;
                    }  else if (seatNumber == 30)
                    {
                        row = 6;
                        col = 5;
                    }
                    else if (seatNumber == 31)
                    {
                        row = 7;
                        col = 1;
                    }
                    else if (seatNumber == 32)
                    {
                        row = 7;
                        col = 1;
                    }
                    else if (seatNumber == 33)
                    {
                        row = 7;
                        col = 5;
                    }
                    else if (seatNumber == 34)
                    {
                        row = 7;
                        col = 5;
                    }
                    else if (seatNumber == 35)
                    {
                        row = 8;
                        col = 1;
                    }
                    else if (seatNumber == 36)
                    {
                        row = 8;
                        col = 1;
                    }
                    else if (seatNumber == 37)
                    {
                        row = 8;
                        col = 2;
                    }
                    else if (seatNumber == 38)
                    {
                        row = 8;
                        col = 2;
                    }
                    else if (seatNumber == 39)
                    {
                        row = 8;
                        col = 3;
                    }
                    else if (seatNumber == 40)
                    {
                        row = 8;
                        col = 3;
                    }
                    else if (seatNumber == 41) 
                    {
                        row = 8;
                        col = 4;
                    } else if (seatNumber == 42)
                    {
                        row = 8;
                        col = 4;
                    }
                    else if (seatNumber == 43)
                    {
                        row = 8;
                        col = 5;
                    }
                    else if (seatNumber == 44) 
                    {
                        row = 8;
                        col = 5;
                    }
                    break;
                default:
                    row = 1;
                    col = seatNumber;
                    break;
            }

            return (row, col);
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
            using (var connection = new SqlConnection(_connectionString))
            {
                var existingCarCount = await connection.QueryFirstOrDefaultAsync<int>(
                    "SELECT COUNT(*) FROM dbo.cars WHERE car_number = @CarNumber AND id != @Id AND is_delete = 0",
                    new { CarNumber = cars.car_number, Id = id }
                );
                if (existingCarCount > 0)
                {
                    return BadRequest();
                }
            }
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
                cmd.Parameters.AddWithValue("@isRetail", cars.is_retail);
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
