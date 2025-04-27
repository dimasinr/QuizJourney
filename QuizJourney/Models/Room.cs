namespace QuizJourney.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string Code { get; set; } = Guid.NewGuid().ToString().Substring(0, 6).ToUpper();
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int TeacherId { get; set; }
        public User? Teacher { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public ICollection<Question> Questions { get; set; } = new List<Question>();
    }
}
