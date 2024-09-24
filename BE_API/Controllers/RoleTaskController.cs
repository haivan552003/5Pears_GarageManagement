using BE_API.Models;
using Dapper;
using Microsoft.AspNetCore.Http;
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
    public class RoleTaskController : ControllerBase
    {
        private readonly string _connectionString;
        public RoleTaskController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SqlConnection");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<role_task>>> GetRoleTask()
        {
            var procedureName = "sp_view_role_task";

            using (var connection = new SqlConnection(_connectionString))
            {
                var role_task = await connection.QueryAsync<role_task>(procedureName, commandType: CommandType.StoredProcedure);
                return Ok(role_task);
            }

        }

        [HttpPost]
        public async Task<ActionResult<role_task>> PostRoleTask(role_task role_Task)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_add_role_task", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@name", role_Task.name);
                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }

            return CreatedAtAction(nameof(GetRoleTask), new { id = role_Task.id }, role_Task);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<role_task>> GetRoleTaskID(int id)
        {
            //return Ok(product);
            var procedureName = "sp_getid_role_task";
            var parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Int32, ParameterDirection.Input);

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var product = await connection.QueryFirstOrDefaultAsync<role_task>(
                    procedureName, parameters, commandType: CommandType.StoredProcedure);

                if (product == null)
                {
                    return NotFound();
                }

                return Ok(product);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, role_task role_Task)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_update_role_task", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@name", role_Task.name);
                await conn.OpenAsync();

                int rowsAffected = await cmd.ExecuteNonQueryAsync();

                if (rowsAffected == 0)
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
                SqlCommand cmd = new SqlCommand("sp_DeleteProduct", conn);
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
