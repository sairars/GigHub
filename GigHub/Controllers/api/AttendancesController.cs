using GigHub.Dtos;
using GigHub.Models;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace GigHub.Controllers.api
{
    [Authorize]
    public class AttendancesController : ApiController
    {
        private readonly ApplicationDbContext _context;

        public AttendancesController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        public IEnumerable<Attendance> GetAttendance()
        {


            var attendances = _context.Attendances.ToList();
            return attendances;
        }

        // POST /api/attendances
        [HttpPost]
        public IHttpActionResult Attend(AttendanceDto attendanceDto)
        {
            var attendeeId = User.Identity.GetUserId();

            if (_context.Attendances.Any(a => a.AttendeeId == attendeeId &&
                                              a.GigId == attendanceDto.GigId))
                return BadRequest("The attendance already exists");

            var attendance = new Attendance
            {
                GigId = attendanceDto.GigId,
                AttendeeId = attendeeId
            };

            _context.Attendances.Add(attendance);
            _context.SaveChanges();
            return Ok();
        }
    }
}
