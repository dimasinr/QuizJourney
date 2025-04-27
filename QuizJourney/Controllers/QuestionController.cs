using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizJourney.Data;
using QuizJourney.DTOs;
using QuizJourney.Models;

namespace QuizJourney.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public QuestionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET api/questions/{roomId}
        [HttpGet("{roomId}")]
        public async Task<IActionResult> GetQuestions(int roomId)
        {
            var questions = await _context.Questions
                .Where(q => q.RoomId == roomId)
                .Include(q => q.Choices)
                .ToListAsync();

            if (questions == null || !questions.Any())
                return NotFound("Questions not found for the specified room.");

            var result = questions.Select(q => new QuestionDTO
            {
                Id = q.Id,
                Text = q.Text,
                RoomId = q.RoomId,
                CorrectChoiceId = q.CorrectChoiceId,
                Choices = q.Choices?.Select(c => new ChoiceDTO
                {
                    Id = c.Id,
                    Text = c.Text,
                    IsCorrect = c.Id == q.CorrectChoiceId
                }).ToList() ?? new List<ChoiceDTO>()
            });

            return Ok(result);
        }

        // POST api/questions
        [HttpPost]
        public async Task<IActionResult> CreateQuestion([FromBody] Question question)
        {
            if (question == null)
                return BadRequest("Invalid question data.");

            if (question.Choices == null || !question.Choices.Any())
                return BadRequest("Question must have at least one choice.");

            // Cek apakah RoomId valid (optional)
            var roomExists = await _context.Rooms.AnyAsync(r => r.Id == question.RoomId);
            if (!roomExists)
                return BadRequest("Invalid RoomId: Room doesn't exist.");

            _context.Questions.Add(question);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetQuestions), new { roomId = question.RoomId }, new { message = "Question created successfully" });
        }

        // DELETE api/questions/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuestion(int id)
        {
            var question = await _context.Questions
                .Include(q => q.Choices)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (question == null)
                return NotFound("Question not found.");

            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Question berhasil dihapus" });
        }
    }
}
