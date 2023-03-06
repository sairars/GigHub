using GigHub.Dtos;
using GigHub.Models;
using Microsoft.AspNet.Identity;
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

        // DELETE /api/attendances
        [HttpDelete]
        public IHttpActionResult DeleteAttendance(int id)
        {
            var userId = User.Identity.GetUserId();
            var attendance = _context.Attendances
                                        .SingleOrDefault(a => a.GigId == id &&
                                                              a.AttendeeId == userId);

            if (attendance == null)
                return NotFound();

            _context.Attendances.Remove(attendance);
            _context.SaveChanges();

            return Ok(id);
        }
    }
}
