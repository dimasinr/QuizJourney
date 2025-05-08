namespace QuizJourney.DTOs
{
    public class RoomDTO
    {
        public int RoomId { get; set; }  
        public string Title { get; set; }
        public string Description { get; set; }
        public TeacherDTO? Teacher { get; set; }  // Make sure this is nullable or optional

    }

    public class TeacherDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
    }

}
