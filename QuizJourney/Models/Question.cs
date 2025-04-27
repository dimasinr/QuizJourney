using System.Text.Json.Serialization;

namespace QuizJourney.Models
{
    public class Question
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public Room? Room { get; set; }

        public string Text { get; set; } = string.Empty;

        [JsonIgnore] 
        public ICollection<Choice>? Choices { get; set; }
        public int CorrectChoiceId { get; set; }
    }
}
