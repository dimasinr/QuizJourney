using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizJourney.Data;
using QuizJourney.DTOs;
using QuizJourney.Models;
using System.Security.Claims;

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

        [HttpGet("{roomId}")]
        [Authorize]
        public async Task<IActionResult> GetQuestions(int roomId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized();

            var userId = int.Parse(userIdClaim.Value);

            var questions = await _context.Questions
                .Where(q => q.RoomId == roomId)
                .Include(q => q.Choices)
                .ToListAsync();

            if (questions == null || !questions.Any())
                return NotFound("Questions not found for the specified room.");
            
            var today = DateTime.UtcNow.Date;
            var answeredQuestions = await _context.StudentAnswers
                .Where(sa => sa.UserId == userId && sa.Question.RoomId == roomId && sa.CreatedAt.Date == today)
                .ToListAsync();

            var result = questions.Select(q => 
            {
                double score = 0;
                var answer = answeredQuestions.FirstOrDefault(a => a.QuestionId == q.Id);
                ChoiceDTO? selectedChoiceDto = null;
                if(answer != null){
                    var selectedChoiceEntity = q.Choices.FirstOrDefault(c => c.Id == answer.SelectedChoiceId);
                    if (selectedChoiceEntity != null)
                    {
                        selectedChoiceDto = new ChoiceDTO
                        {
                            Id = selectedChoiceEntity.Id,
                            Text = selectedChoiceEntity.Text,
                            IsCorrect = selectedChoiceEntity.Id == q.CorrectChoiceId
                        };
                        score = answer.Score;                
                    }
                }


                return new QuestionDTO
                {
                    Id = q.Id,
                    Text = q.Text,
                    RoomId = q.RoomId,
                    SelectedChoice = selectedChoiceDto, 
                    Score = score,
                    CorrectChoiceId = q.CorrectChoiceId,
                    Choices = q.Choices?.Select(c => new ChoiceDTO
                    {
                        Id = c.Id,
                        Text = c.Text,
                        IsCorrect = c.Id == q.CorrectChoiceId
                    }).ToList() ?? new List<ChoiceDTO>()
                }; 
            });

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateQuestion([FromBody] QuestionDTO questionDTO)
        {
            if (questionDTO == null)
                return BadRequest("Invalid question data.");

            if (questionDTO.Choices == null || !questionDTO.Choices.Any())
                return BadRequest("Question must have at least one choice.");

            var roomExists = await _context.Rooms.AnyAsync(r => r.Id == questionDTO.RoomId);
            if (!roomExists)
                return BadRequest("Invalid RoomId: Room doesn't exist.");

            var question = new Question
            {
                RoomId = questionDTO.RoomId,
                Text = questionDTO.Text,
                Choices = new List<Choice>()
            };

            foreach (var choiceDTO in questionDTO.Choices)
            {
                var choice = new Choice
                {
                    Text = choiceDTO.Text,
                    IsCorrect = choiceDTO.IsCorrect
                };

                question.Choices.Add(choice);

                if (choiceDTO.IsCorrect)
                {
                    question.CorrectChoiceId = 0;
                }
            }

            _context.Questions.Add(question);
            await _context.SaveChangesAsync();

            var correctChoice = question.Choices.FirstOrDefault(c => c.IsCorrect);
            if (correctChoice != null)
            {
                question.CorrectChoiceId = correctChoice.Id;
                _context.Questions.Update(question);
                await _context.SaveChangesAsync(); // Save the updated CorrectChoiceId
            }

            return CreatedAtAction(nameof(GetQuestions), new { roomId = question.RoomId }, new { message = "Question created successfully" });
        }

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
