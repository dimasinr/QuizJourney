namespace QuizJourney.Services.RoomTracker;
public interface IRoomTrackerService
{
    void AddUserToRoom(string roomId, string connectionId, string username);
    void RemoveUser(string connectionId);
    int GetRoomUserCount(string roomId);
    List<string> GetUsernamesInRoom(string roomId);
}
