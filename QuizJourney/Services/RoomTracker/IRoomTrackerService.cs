namespace QuizJourney.Services.RoomTracker;
public interface IRoomTrackerService
{
    void AddUserToRoom(int roomId, string connectionId, string username);
    void RemoveUserFromRoom(string connectionId);
    List<string> GetUsersInRoom(int roomId);
    int? GetRoomByConnectionId(string connectionId);
    string? GetUsernameByConnectionId(string connectionId);
}

