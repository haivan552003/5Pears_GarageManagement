using BE_API.ModelCustom;
using BE_API.Models;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
namespace BE_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Cus_AddressController : ControllerBase
    {
        private readonly string _connectionString;
        public Cus_AddressController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SqlConnection");
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<cus_address>>> GetCustomers()
        {
            var procedureName = "sp_view_customer_address";

            using (var connection = new SqlConnection(_connectionString))
            {
                var customers = await connection.QueryAsync<cus_address>(procedureName, commandType: CommandType.StoredProcedure);
                return Ok(customers);
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<cus_address>>> GetCustomerById(int id)
        {
            var procedureName = "sp_view_customer_address_with_id";
            var parameters = new { id };

            using (var connection = new SqlConnection(_connectionString))
            {
                var customers = await connection.QueryAsync<cus_address>(procedureName, parameters, commandType: CommandType.StoredProcedure);

                if (!customers.Any())
                {
                    return NotFound();  // Nếu không có bản ghi nào
                }

                return Ok(customers);  // Trả về danh sách nhiều bản ghi
            }
        }

        [HttpPost]
            public async Task<ActionResult> AddCustomer([FromBody] cus_address newCustomer)
            {
                var procedureName = "sp_add_cus_address";
                var parameters = new DynamicParameters();
                parameters.Add("@address", newCustomer.address, DbType.String);
                parameters.Add("@id_cus", newCustomer.id_cus, DbType.Int32);
                parameters.Add("@type", newCustomer.type, DbType.Byte);
                parameters.Add("@status", newCustomer.status, DbType.Byte);


                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var result = await connection.ExecuteAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);

                    if (result > 0)
                    {
                        return Ok(new { message = "Customer added successfully" });
                    }

                    return BadRequest(new { message = "Failed to add customer" });
                }
            }
        // PUT: api/Cus_Address/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, [FromBody] cus_address cus)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_update_cus_address", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@id", id);                  // ID địa chỉ
                cmd.Parameters.AddWithValue("@address", cus.address);    // Địa chỉ
                cmd.Parameters.AddWithValue("@id_cus", cus.id_cus);      // ID khách hàng
                cmd.Parameters.AddWithValue("@type", cus.type);          // Loại
                cmd.Parameters.AddWithValue("@status", cus.status);      // Trạng thái

                await conn.OpenAsync();
                int rowsAffected = await cmd.ExecuteNonQueryAsync();

                if (rowsAffected == 0)
                {
                    return NotFound(); // Nếu không tìm thấy bản ghi
                }
            }

            return NoContent(); // Trả về 204 No Content khi thành công
        }


        // DELETE: api/Cus_Address/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCustomer(int id)
        {
            var procedureName = "sp_delete_cus_address";
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id, DbType.Int32);

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var result = await connection.ExecuteAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);

                if (result > 0)
                {
                    return Ok(new { message = "Customer deleted successfully" });
                }

                return NotFound(new { message = "Customer not found" });
            }
        }

    }
}
