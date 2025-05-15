using System.Text.Json.Serialization;

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

        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        public ChoiceDTO? SelectedChoice { get; set; }
        public double Score { get; set; }
        public List<ChoiceDTO> Choices { get; set; } = new();
        public int CorrectChoiceId { get; set; }
    }
}
