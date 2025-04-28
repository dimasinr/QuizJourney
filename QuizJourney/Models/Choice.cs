using System.ComponentModel.DataAnnotations.Schema;

namespace QuizJourney.Models
{
    public class Choice
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public Question? Question { get; set; }
        public bool IsCorrect { get; set; }
        public string Text { get; set; } = string.Empty;
    }
}
