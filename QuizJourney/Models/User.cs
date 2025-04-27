using System.Text.Json.Serialization;
using QuizJourney.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    [JsonIgnore]
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = "Student"; 

    [JsonIgnore]
    public ICollection<Room>? CreatedRooms { get; set; }
    [JsonIgnore]
    public ICollection<StudentAnswer>? Answers { get; set; }
}