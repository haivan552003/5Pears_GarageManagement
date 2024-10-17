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
    public class BannerController : ControllerBase
    {
        private readonly string _connectionString;
        public BannerController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SqlConnection");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<banner>>> GetBanner()
        {
            var procedureName = "sp_view_banner";

            using (var connection = new SqlConnection(_connectionString))
            {
                var banners = await connection.QueryAsync<banner>(procedureName, commandType: CommandType.StoredProcedure);
                return Ok(banners);
            }

        }

        [HttpPost]
        public async Task<ActionResult<banner>> PostBanner(banner banner)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_add_banner", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@img_banner", banner.img_banner);
                cmd.Parameters.AddWithValue("@title", banner.title);
                cmd.Parameters.AddWithValue("@emp_id", banner.emp_id);
                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }

            return CreatedAtAction(nameof(GetBanner), new { id = banner.id }, banner);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<banner>> GetBannerID(int id)
        {
            //return Ok(product);
            var procedureName = "sp_getid_banner";
            var parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Int32, ParameterDirection.Input);

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var product = await connection.QueryFirstOrDefaultAsync<banner>(
                    procedureName, parameters, commandType: CommandType.StoredProcedure);

                if (product == null)
                {
                    return NotFound();
                }

                return Ok(product);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutBanner(int id, banner banner)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_update_banner", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@title", banner.title);
                cmd.Parameters.AddWithValue("@img_banner", banner.img_banner);

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
        public async Task<IActionResult> DeleteBanner(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_delete_banner", conn);
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
