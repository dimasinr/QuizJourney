using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizJourney.Data;
using QuizJourney.DTOs;
using QuizJourney.Models;

namespace QuizJourney.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Student")]
    public class StudentAnswerController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StudentAnswerController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize] // Pastikan user harus login
        [HttpPost]
        public async Task<IActionResult> SubmitAnswer([FromBody] StudentAnswerRequest answerRequest)
        {
            if (answerRequest == null)
                return BadRequest("Answer data is required");

            // Ambil UserId dari JWT
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("User ID not found in token");

            int userId = int.Parse(userIdClaim.Value);

            var question = await _context.Questions.FindAsync(answerRequest.QuestionId);
            if (question == null)
                return NotFound("Question not found");

            var selectedChoice = await _context.Choices.FindAsync(answerRequest.SelectedChoiceId);
            if (selectedChoice == null)
                return NotFound("Selected choice not found");

            double score = (selectedChoice.Id == question.CorrectChoiceId) ? 100.0 : 0.0;

            var studentAnswer = new StudentAnswer
            {
                QuestionId = answerRequest.QuestionId,
                SelectedChoiceId = answerRequest.SelectedChoiceId,
                TimeTaken = answerRequest.TimeTaken,
                Score = score,
                UserId = userId
            };

            _context.StudentAnswers.Add(studentAnswer);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetStudentAnswer), new { id = studentAnswer.Id }, studentAnswer);
        }

        // 2. Get Student Answer Details
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudentAnswer(int id)
        {
            var studentAnswer = await _context.StudentAnswers.Include(sa => sa.Question)
                                                              .ThenInclude(q => q.Choices)
                                                              .FirstOrDefaultAsync(sa => sa.Id == id);

            if (studentAnswer == null)
                return NotFound();

            return Ok(studentAnswer);
        }
    }
}
