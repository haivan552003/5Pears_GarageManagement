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
    public class NewsController : ControllerBase
    {
        private readonly string _connectionString;
        public NewsController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SqlConnection");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<news>>> GetNews()
        {
            var procedureName = "sp_view_news1";

            using (var connection = new SqlConnection(_connectionString))
            {
                var news = await connection.QueryAsync<news>(procedureName, commandType: CommandType.StoredProcedure);
                return Ok(news);
            }

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<news>> GetNewsID(int id)
        {
            //return Ok(product);
            var procedureName = "sp_getid_news1";
            var parameters = new DynamicParameters();
            parameters.Add("id", id, DbType.Int32, ParameterDirection.Input);

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var news = await connection.QueryFirstOrDefaultAsync<news>(
                    procedureName, parameters, commandType: CommandType.StoredProcedure);

                if (news == null)
                {
                    return NotFound();
                }

                return Ok(news);
            }
        }


        [HttpPost]
        public async Task<ActionResult<news>> PostNews(news News)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_add_news1", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@news_img", News.news_img);
                cmd.Parameters.AddWithValue("@title", News.title);
                cmd.Parameters.AddWithValue("@content", News.content);
                cmd.Parameters.AddWithValue("@id_emp", News.id_emp);
                cmd.Parameters.AddWithValue("@status", News.status);
                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }

            return CreatedAtAction(nameof(GetNews), new { id = News.id }, News);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutNews(int id, news News)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_update_news1", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@news_img", News.news_img);
                cmd.Parameters.AddWithValue("@title", News.title);
                cmd.Parameters.AddWithValue("@content", News.content);
                cmd.Parameters.AddWithValue("@id_emp", News.id_emp);
                cmd.Parameters.AddWithValue("@status", News.status);
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNews(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_delete_news1", conn);
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
