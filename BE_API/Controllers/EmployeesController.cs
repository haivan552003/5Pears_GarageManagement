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
        public async Task<ActionResult> AddEmployees(employees_create newEmployee)
        {
            try
            {
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(newEmployee.password);
                var parameters = new DynamicParameters(newEmployee);
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var result = await connection.ExecuteAsync("sp_add_employee", parameters, commandType: CommandType.StoredProcedure);

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


        // PUT: api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployees(int id, employees_update employee)
        {
            var parameters = new DynamicParameters(employee);
            parameters.Add("@id", id);

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var result = await connection.ExecuteAsync("sp_updateemployee", parameters, commandType: CommandType.StoredProcedure);

                if (result == 0)
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
