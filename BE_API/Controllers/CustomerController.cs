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

       [HttpPost]
        public async Task<ActionResult> AddCustomer(customer_create newCustomer)
        {
            try
            {
                var parameters = new DynamicParameters(newCustomer);

                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var result = await connection.ExecuteAsync("sp_add_customers", parameters, commandType: CommandType.StoredProcedure);

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


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] customer_create updatedCustomer)
        {
            var parameters = new DynamicParameters(updatedCustomer);
            parameters.Add("@id", id);

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var result = await connection.ExecuteAsync("sp_update_customers", parameters, commandType: CommandType.StoredProcedure);

                if (result == 0)
                {
                    return NotFound();
                }
            }

            return NoContent();
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

    }
}
