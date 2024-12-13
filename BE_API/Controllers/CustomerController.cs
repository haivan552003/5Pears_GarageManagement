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
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;


namespace BE_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly string _connectionString;
        public CustomerController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SqlConnection");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<customers>>> GetCustomers()
        {
            var procedureName = "sp_view_customer";

            using (var connection = new SqlConnection(_connectionString))
            {
                var customers = await connection.QueryAsync<customers>(procedureName, commandType: CommandType.StoredProcedure);
                return Ok(customers);
            }
        }

        //[HttpGet("{id}")]
        //public async Task<ActionResult<customers>> GetCustomerWithId(int id)
        //{
        //    using (IDbConnection db = new SqlConnection(_connectionString))
        //    {
        //        // Gọi thủ tục lưu trữ để lấy thông tin khách hàng theo ID
        //        var customer = await db.QueryFirstOrDefaultAsync<customers>(
        //            "sp_view_customer_with_id",
        //            new { id },
        //            commandType: CommandType.StoredProcedure);

        //        // Kiểm tra xem khách hàng có tồn tại không
        //        if (customer == null)
        //        {
        //            return NotFound();
        //        }

        //        // Truy vấn địa chỉ của khách hàng
        //        var addresses = await db.QueryAsync<customers>(
        //            "SELECT * FROM customer_addresses WHERE is_delete = 'False' AND id_cus = @Id",
        //            new { Id = id });

        //        // Tạo một đối tượng ẩn danh để chứa thông tin khách hàng và địa chỉ
        //        var result = new
        //        {
        //            Customer = customer,
        //            Addresses = addresses.ToList()
        //        };

        //        return Ok(result);
        //    }
        //}

        [HttpGet("{id}")]
        public async Task<ActionResult<customers>> GetCustomerById(int id)
        {
            var procedureName = "sp_view_customer_with_id";
            var parameters = new { id };

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

        //[HttpPost]
        //public async Task<ActionResult> AddCustomer([FromBody] customers newCustomer)
        //{
        //    try
        //    {
        //        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(newCustomer.password);
        //        var parameters = new DynamicParameters(newCustomer);
        //        using (var connection = new SqlConnection(_connectionString))
        //        {
        //            await connection.OpenAsync();
        //            var result = await connection.ExecuteAsync("sp_add_customers", parameters, commandType: CommandType.StoredProcedure);
        //            if (result > 0)
        //            {
        //                return Ok();
        //            }
        //            else
        //            {
        //                return BadRequest();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Lỗi: {ex.Message}");
        //    }
        //}

        //POST: api/Customer
       [HttpPost]
        public async Task<ActionResult> AddCustomer([FromBody] customers newCustomer)
        {
            var procedureName = "sp_add_customers";
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(newCustomer.password);
            var parameters = new DynamicParameters();
            parameters.Add("@password", hashedPassword, DbType.String);
            parameters.Add("@fullname", newCustomer.fullname, DbType.String);
            parameters.Add("@birthday", newCustomer.birthday, DbType.Date);
            parameters.Add("@gender", newCustomer.gender, DbType.String);
            parameters.Add("@phone_number", newCustomer.phone_number, DbType.String);
            parameters.Add("@citizen_identity_img1", newCustomer.citizen_identity_img1, DbType.String);
            parameters.Add("@citizen_identity_number", newCustomer.citizen_identity_number, DbType.String);
            parameters.Add("@driver_license_img1", newCustomer.driver_license_img1, DbType.String);
            parameters.Add("@driver_license_number", newCustomer.driver_license_number, DbType.String);
            parameters.Add("@status", newCustomer.status, DbType.String);
            parameters.Add("@email", newCustomer.email, DbType.String); 
            parameters.Add("@img_cus", newCustomer.img_cus, DbType.String);

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var result = await connection.ExecuteAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);

                if (result > 0)
                {
                    return Ok();
                }

                return BadRequest();
            }
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] customers updatedCustomer)
        {
            var procedureName = "sp_update_customers";
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(updatedCustomer.password);

            var parameters = new DynamicParameters();
            parameters.Add("@cus_id", id, DbType.Int32);
            parameters.Add("@cus_code", updatedCustomer.cus_code, DbType.String);
            parameters.Add("@email", updatedCustomer.email, DbType.String);
            parameters.Add("@password", hashedPassword, DbType.String);
            parameters.Add("@fullname", updatedCustomer.fullname, DbType.String);
            parameters.Add("@birthday", updatedCustomer.birthday, DbType.Date);
            parameters.Add("@gender", updatedCustomer.gender, DbType.String);
            parameters.Add("@phone_number", updatedCustomer.phone_number, DbType.String);
            parameters.Add("@citizen_identity_img1", updatedCustomer.citizen_identity_img1, DbType.String);
            parameters.Add("@citizen_identity_number", updatedCustomer.citizen_identity_number, DbType.String);
            parameters.Add("@driver_license_img1", updatedCustomer.driver_license_img1, DbType.String);
            parameters.Add("@driver_license_number", updatedCustomer.driver_license_number, DbType.String);
            parameters.Add("@status", updatedCustomer.status, DbType.String);
            parameters.Add("@img_cus", updatedCustomer.img_cus, DbType.String);


            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var result = await connection.ExecuteAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);

                if (result > 0)
                {
                    return Ok(new { message = "Customer updated successfully" });
                }

                return BadRequest(new { message = "Failed to update customer" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_delete_customer", conn);
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

        [HttpGet("GetTop")]
        public async Task<ActionResult<IEnumerable<customers>>> GetTop()
        {
            var procedureName = "sp_get_top_customer";

            using (var connection = new SqlConnection(_connectionString))
            {
                var customers = await connection.QueryAsync<customers>(procedureName, commandType: CommandType.StoredProcedure);
                return Ok(customers);
            }
        }

    }
}
