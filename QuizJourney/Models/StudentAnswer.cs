namespace QuizJourney.Models
{
    public class StudentAnswer
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public int QuestionId { get; set; }
        public Question? Question { get; set; }
        public int SelectedChoiceId { get; set; }
        public double TimeTaken { get; set; } 
        public double Score { get; set; }
        public string StudentCode { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
