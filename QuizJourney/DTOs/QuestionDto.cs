namespace QuizJourney.DTOs
{
    public class ChoiceDTO
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
    }

    public class QuestionDTO
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public int RoomId { get; set; }
        public int CorrectChoiceId { get; set; }
        public List<ChoiceDTO> Choices { get; set; } = new();
    }
}
