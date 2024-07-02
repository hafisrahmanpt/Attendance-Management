using Attendance_Management.Web.Data;
using Attendance_Management.Web.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Attendance_Management.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public EmployeesController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetAllEmployees()
        {                     
             var allEmployees = dbContext.Employees.ToList();
             return Ok(allEmployees);           
        }

        [HttpPost]
        public IActionResult AddEmployee(Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

                var employeeEntity = new Employee
                {
                    id = employee.id,
                    Name = employee.Name,
                    Designation = employee.Designation
                };

                dbContext.Employees.Add(employeeEntity);
                dbContext.SaveChanges();

                return CreatedAtAction(nameof(GetAllEmployees), employeeEntity);
        }

    }
}
