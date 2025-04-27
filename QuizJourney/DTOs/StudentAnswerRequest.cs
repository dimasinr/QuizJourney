namespace QuizJourney.DTOs
{
    public class StudentAnswerRequest
    {
        public int QuestionId { get; set; }
        public int SelectedChoiceId { get; set; }
        public double TimeTaken { get; set; }
    }
}
