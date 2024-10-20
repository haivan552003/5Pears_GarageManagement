using BE_API.Models;
using Dapper;
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
    public class RolesController : Controller
    {      
        private readonly string _connectionString;
        public RolesController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SqlConnection");
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<roles>>> GetRoles()
        {
            var procedureName = "sp_roles";

            using (var connection = new SqlConnection(_connectionString))
            {
                var roles = await connection.QueryAsync<roles>(procedureName, commandType: CommandType.StoredProcedure);
                return Ok(roles);
            }

        }
        [HttpGet("{id}")]
        public async Task<ActionResult<roles>> GetRoleTaskID(int id)
        {
            //return Ok(product);
            var procedureName = "sp_getid_roles";
            var parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Int32, ParameterDirection.Input);

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var product = await connection.QueryFirstOrDefaultAsync<roles>(
                    procedureName, parameters, commandType: CommandType.StoredProcedure);

                if (product == null)
                {
                    return NotFound();
                }

                return Ok(product);
            }
        }
        [HttpPost]
        public async Task<ActionResult<roles>> PostRoles(roles role_Task)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_add_roles", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@name", role_Task.name);             
                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }

            return CreatedAtAction(nameof(PostRoles), new { id = role_Task.id }, role_Task);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRoles(int id, roles role_Task)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_update_role", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@id_role", id); 
                cmd.Parameters.AddWithValue("@name", role_Task.name);  
                cmd.Parameters.AddWithValue("@date_update", DateTime.Now);

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
        public async Task<IActionResult> DeleteRoles(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_delete_Roles", conn);
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
    }
}
