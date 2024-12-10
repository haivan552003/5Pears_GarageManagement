using BE_API.ModelCustom;
using BE_API.Models;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace BE_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValidateController : Controller
    {
        private readonly string _connectionString;
        public ValidateController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SqlConnection");
        }

        [HttpGet("ValidateCustomer/{id}")]
        public async Task<IActionResult> ValidateCustomer(int id)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@id", id, DbType.Int32);

                    var result = await connection.QueryFirstOrDefaultAsync<string>(
                        "sp_validate_customer",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    if (result == null)
                    {
                        return Ok(new
                        {
                            Success = true,
                        });
                    }

                    return Ok(new
                    {
                        Success = false,
                        Message = result
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Success = false,
                    Message = "Đã xãy ra lỗi khi xác thực khách hàng",
                    Error = ex.Message
                });
            }
        }


        [HttpGet("ValidateDriver/{id}")]
        public async Task<IActionResult> ValidateDriver(int id)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var parameters = new DynamicParameters();
                    parameters.Add("@id", id, DbType.Int32);

                    var result = await connection.QueryFirstOrDefaultAsync<string>(
                        "sp_validate_driver",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    if (result == null)
                    {
                        return Ok(new
                        {
                            Success = true,
                            Message = "Tài xế có thể được xóa."
                        });
                    }

                    return Ok(new
                    {
                        Success = false,
                        Message = result
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Success = false,
                    Message = "Có lỗi xảy ra.",
                    Details = ex.Message
                });
            }
        }


        [HttpGet("ValidateLocation/{id}")]
        public async Task<IActionResult> ValidateLocation(int id)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var parameters = new DynamicParameters();
                    parameters.Add("@id", id, DbType.Int32);

                    var result = await connection.QueryFirstOrDefaultAsync<string>(
                        "sp_validate_location",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    if (result == null)
                    {
                        return Ok(new
                        {
                            Success = true,
                            Message = "Địa chỉ có thể được xóa."
                        });
                    }

                    return Ok(new
                    {
                        Success = false,
                        Message = result
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Success = false,
                    Message = "Có lỗi xảy ra.",
                    Details = ex.Message
                });
            }
        }


        [HttpGet("ValidateTrip/{id}")]
        public async Task<IActionResult> ValidateTrip(int id)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var parameters = new DynamicParameters();
                    parameters.Add("@id", id, DbType.Int32);

                    var result = await connection.QueryFirstOrDefaultAsync<string>(
                        "sp_validate_trip",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    if (result == null)
                    {
                        return Ok(new
                        {
                            Success = true,
                            Message = "Chuyến xe có thể được xóa."
                        });
                    }

                    return Ok(new
                    {
                        Success = false,
                        Message = result
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Success = false,
                    Message = "Có lỗi xảy ra.",
                    Details = ex.Message
                });
            }
        }

        [HttpGet("ValidateTripDetails/{id}")]
        public async Task<IActionResult> ValidateTripDetails(int id)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var parameters = new DynamicParameters();
                    parameters.Add("@id", id, DbType.Int32);

                    var result = await connection.QueryFirstOrDefaultAsync<string>(
                        "sp_validate_trip_details",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    if (result == null)
                    {
                        return Ok(new
                        {
                            Success = true,
                            Message = "Chi tiết chuyến xe có thể được xóa."
                        });
                    }

                    return Ok(new
                    {
                        Success = false,
                        Message = result
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Success = false,
                    Message = "Có lỗi xảy ra.",
                    Details = ex.Message
                });
            }
        }

        [HttpGet("ValidateCarTypes/{id}")]
        public async Task<IActionResult> ValidateCarTypes(int id)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@id", id, DbType.Int32);

                    var result = await connection.QueryFirstOrDefaultAsync<string>(
                        "sp_validate_cartypes",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    if (result == null)
                    {
                        return Ok(new
                        {
                            Success = true,
                        });
                    }

                    return Ok(new
                    {
                        Success = false,
                        Message = result
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Success = false,
                    Message = "Đã xãy ra lỗi khi xác thực loại xe",
                    Error = ex.Message
                });
            }
        }

        [HttpGet("ValidateCarBrands/{id}")]
        public async Task<IActionResult> ValidateCarBrands(int id)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@id", id, DbType.Int32);

                    var result = await connection.QueryFirstOrDefaultAsync<string>(
                        "sp_validate_carbrands",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    if (result == null)
                    {
                        return Ok(new
                        {
                            Success = true,
                        });
                    }

                    return Ok(new
                    {
                        Success = false,
                        Message = result
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Success = false,
                    Message = "Đã xãy ra lỗi khi xác thực hãng xe",
                    Error = ex.Message
                });
            }
        }

        [HttpGet("CheckPhoneNumberCustomer/{phone_number}")]
        public async Task<IActionResult> CheckPhoneNumberCustomer(string phone_number)
        {
            var procedureName = "sp_check_phone_customer";
            var parameters = new { phone_number };

            using (var connection = new SqlConnection(_connectionString))
            {
                var customer = await connection.QuerySingleOrDefaultAsync<customers>(procedureName, parameters, commandType: CommandType.StoredProcedure);
                if (customer == null)
                {
                    return NotFound();
                }
                return Ok(customer);
            }
        }

        [HttpGet("ValidateCheckScheduleCar")]
        public async Task<IActionResult> ValidateCheckScheduleCar(int id, DateTime date_start )
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var parameters = new DynamicParameters();
                    parameters.Add("@id", id, DbType.Int32);
                    parameters.Add("@date_start", date_start, DbType.DateTime);

                    var result = await connection.QueryFirstOrDefaultAsync<string>(
                        "sp_validate_check_schedule_car",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    if (result == null)
                    {
                        return Ok(new
                        {
                            Success = true,
                            Message = "Chuyến xe có thể được xóa."
                        });
                    }

                    return Ok(new
                    {
                        Success = false,
                        Message = result
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Success = false,
                    Message = "Có lỗi xảy ra.",
                    Details = ex.Message
                });
            }
        }  
        
        [HttpGet("ValidateCheckScheduleDriver")]
        public async Task<IActionResult> ValidateCheckScheduleDriver(int id, DateTime date_start )
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var parameters = new DynamicParameters();
                    parameters.Add("@id", id, DbType.Int32);
                    parameters.Add("@date_start", date_start, DbType.DateTime);

                    var result = await connection.QueryFirstOrDefaultAsync<string>(
                        "sp_validate_check_schedule_driver",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    if (result == null)
                    {
                        return Ok(new
                        {
                            Success = true,
                            Message = "Chuyến xe có thể được xóa."
                        });
                    }

                    return Ok(new
                    {
                        Success = false,
                        Message = result
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Success = false,
                    Message = "Có lỗi xảy ra.",
                    Details = ex.Message
                });
            }
        }
    }
}
