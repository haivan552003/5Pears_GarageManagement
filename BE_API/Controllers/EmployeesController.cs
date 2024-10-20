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
using System.Threading.Tasks;

namespace BE_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly string _connectionString;
        public EmployeesController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SqlConnection");
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<employees>>> sp_view_Employees()
        {
            var procedureName = "sp_view_Employees";

            using (var connection = new SqlConnection(_connectionString))
            {
                var employeesList = await connection.QueryAsync<employees>(procedureName, commandType: CommandType.StoredProcedure);
                return Ok(employeesList);
            }
        }


        // GET: api/Products/5

        [HttpGet("{id}")]
        public async Task<ActionResult<employees>> sp_view_EmployeesID(int id)
        {
            //return Ok(product);
            var procedureName = "sp_view_EmployeesID";
            var parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Int32, ParameterDirection.Input);

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var product = await connection.QueryFirstOrDefaultAsync<employees>(
                    procedureName, parameters, commandType: CommandType.StoredProcedure);

                if (product == null)
                {
                    return NotFound();
                }

                return Ok(product);
            }
        }
        // POST: api/employees
        [HttpPost]
        public async Task<ActionResult> AddEmployees([FromBody] employees newEmployee)
        {
            // Kiểm tra xem đối tượng có hợp lệ không
            if (newEmployee == null || string.IsNullOrEmpty(newEmployee.email) || string.IsNullOrEmpty(newEmployee.password))
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            // Mã hóa mật khẩu
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(newEmployee.password);

            // Sử dụng DynamicParameters để truyền tham số
            var parameters = new DynamicParameters();
            parameters.Add("@user_name", newEmployee.email, DbType.String);
            parameters.Add("@emp_code", newEmployee.emp_code, DbType.String);
            parameters.Add("@pass_word", hashedPassword, DbType.String);
            parameters.Add("@full_name", newEmployee.fullname, DbType.String);
            parameters.Add("@birthday", newEmployee.birthday, DbType.Date);
            parameters.Add("@citizen_identity_img", newEmployee.citizen_identity_img, DbType.String);
            parameters.Add("@citizen_identity_number", newEmployee.citizen_identity_number, DbType.String);
            parameters.Add("@status", newEmployee.status, DbType.String);
            parameters.Add("@gender", newEmployee.gender, DbType.String);
            parameters.Add("@id_role", newEmployee.role_id, DbType.Int32);

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var result = await connection.ExecuteAsync("sp_add_employee", parameters, commandType: CommandType.StoredProcedure);

                if (result > 0)
                {
                    return Ok(new { message = "Nhân viên đã được thêm thành công" });
                }

                return BadRequest(new { message = "Thêm nhân viên không thành công" });
            }
        }

        // PUT: api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployees(int id, employees employees)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_updateemployee", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@id_emp", id); 
                cmd.Parameters.AddWithValue("@full_name", employees.fullname);
                cmd.Parameters.AddWithValue("@birthday", employees.birthday);
                cmd.Parameters.AddWithValue("@citizen_identity_img", employees.citizen_identity_img);
                cmd.Parameters.AddWithValue("@citizen_identity_number", employees.citizen_identity_number);
                cmd.Parameters.AddWithValue("@status", employees.status); 
                cmd.Parameters.AddWithValue("@gender", employees.gender);
                cmd.Parameters.AddWithValue("@id_role", employees.role_id);

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
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString)) 
                {
                    await connection.OpenAsync();

                    var parameters = new DynamicParameters();
                    parameters.Add("@id_emp", id, DbType.Int32, ParameterDirection.Input);

                    var result = await connection.ExecuteAsync("sp_delete_employee", parameters, commandType: CommandType.StoredProcedure);

                    if (result > 0)
                    {
                        return Ok();
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

    }
}
