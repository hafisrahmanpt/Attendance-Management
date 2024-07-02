using Attendance_Management.Web.Data;
using Attendance_Management.Web.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Attendance_Management.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public AttendanceController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet("details")]
        public IActionResult GetAttendanceDetails()
        {

                var attendanceDetails = dbContext.Attendance
                    .Include(a => a.Employee)
                    .Select(a => new
                    {
                        Id = a.Id,
                        EmployeeName = a.Employee.Name,
                        CheckIn = a.CheckIn,
                        CheckOut = a.CheckOut
                    })
                    .ToList();

                return Ok(attendanceDetails);

        }

        [HttpPost("CheckIn")]
        public IActionResult CheckIn(int empId)
        {
            var existingAttendance = dbContext.Attendance
                .FirstOrDefault(a => a.EmpId == empId && a.CheckIn.Date == DateTime.Today);

            if (existingAttendance != null)
            {
                return BadRequest("Already checked in today");
            }
            var newAttendance = new Attendance
            {
                EmpId = empId,
                CheckIn = DateTime.Now
            };

            dbContext.Attendance.Add(newAttendance);
            dbContext.SaveChanges();

            return Ok("Checked in successfully");
        }


        [HttpPost("CheckOut")]
        public IActionResult CheckOut(int empId)
        {
            var attendance = dbContext.Attendance
                .FirstOrDefault(a => a.EmpId == empId && a.CheckIn.Date == DateTime.Today);

            if (attendance == null)
            {
                return BadRequest("No check-in available for today");
            }

            attendance.CheckOut = DateTime.Now;

            dbContext.SaveChanges();

            return Ok("Checked out successfully");
        }

        [HttpGet("{empId}")]
        public IActionResult GetAttendanceDetails(int empId)
        {

                var attendanceDetails = dbContext.Attendance
                    .Where(a => a.EmpId == empId)
                    .OrderByDescending(a => a.CheckIn)
                    .Select(a => new
                    {
                        AttendanceId = a.Id,
                        EmployeeName = a.Employee.Name,
                        CheckIn = a.CheckIn,
                        CheckOut = a.CheckOut
                    })
                    .ToList();

                return Ok(attendanceDetails);

        }

    }
}
