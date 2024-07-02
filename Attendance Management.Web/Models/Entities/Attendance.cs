namespace Attendance_Management.Web.Models.Entities
{
    public class Attendance
    {
        public int Id { get; set; }
        public int EmpId { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }


        public Employee Employee { get; set; }
    }
}
