using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizJourney.Data;
using QuizJourney.DTOs;
using QuizJourney.Models;
using System.Security.Claims;

namespace QuizJourney.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Teacher")]
    public class RoomController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RoomController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Create Room
        [HttpPost]
        public async Task<IActionResult> CreateRoom([FromBody] RoomDTO roomDTO)
        {
            if (roomDTO == null)
                return BadRequest("Room data is required");

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("Invalid token");

            var teacherId = int.Parse(userIdClaim.Value);

            var room = new Room
            {
                Title = roomDTO.Title,
                Description = roomDTO.Description,
                TeacherId = teacherId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();

            var teacher = await _context.Users.FindAsync(teacherId);

            var createdRoomDTO = new RoomDTO
            {
                RoomId = room.Id,
                Title = room.Title,
                Description = room.Description,
                Teacher = teacher != null ? new TeacherDTO
                {
                    Id = teacher.Id,
                    Name = teacher.Username
                } : null
            };

            return CreatedAtAction(nameof(GetRoom), new { id = room.Id }, createdRoomDTO);
        }

        // Get Room by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoom(int id)
        {
            var room = await _context.Rooms
                .Include(r => r.Teacher)
                .Include(r => r.Questions)
                    .ThenInclude(q => q.Choices)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (room == null)
                return NotFound("Room not found");

            return Ok(room);
        }

        // Delete Room by ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var room = await _context.Rooms.FindAsync(id);

            if (room == null)
                return NotFound("Room not found");

            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Room berhasil dihapus" });
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetRooms([FromQuery] int? teacherId)
        {
            var user = User;
            var isAuthenticated = user.Identity?.IsAuthenticated ?? false;
            var role = user.FindFirst(ClaimTypes.Role)?.Value;
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);

            IQueryable<Room> query = _context.Rooms.Include(r => r.Teacher);

            if (isAuthenticated)
            {
                if (role == "Teacher")
                {
                    if (userIdClaim == null)
                        return Unauthorized("Invalid token");

                    var teacherIdFromToken = int.Parse(userIdClaim.Value);
                    query = query.Where(r => r.TeacherId == teacherIdFromToken);
                }
                else if (role == "Student")
                {
                    if (teacherId.HasValue)
                    {
                        query = query.Where(r => r.TeacherId == teacherId.Value);
                    }
                }
            }
            else
            {
                return Unauthorized("User not authenticated");
            }

            var rooms = await query.ToListAsync();

            var roomDTOs = rooms.Select(r => new RoomDTO
            {
                RoomId = r.Id,
                Title = r.Title,
                Description = r.Description,
                Teacher = r.Teacher != null ? new TeacherDTO
                {
                    Id = r.Teacher.Id,
                    Name = r.Teacher.Username
                } : null
            }).ToList();

            return Ok(roomDTOs);
        }
    }
}
