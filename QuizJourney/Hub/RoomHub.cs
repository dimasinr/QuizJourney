using Microsoft.AspNetCore.SignalR;
using QuizJourney.Services.RoomTracker;

public class RoomHub : Hub
{
    private readonly IRoomTrackerService _roomTracker;

    public RoomHub(IRoomTrackerService roomTracker)
    {
        _roomTracker = roomTracker;
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        string connId = Context.ConnectionId;
        _roomTracker.RemoveUser(connId);
        await base.OnDisconnectedAsync(exception);
    }

    public async Task JoinRoom(int roomId, string username)
    {
        string connId = Context.ConnectionId;
        string roomKey = roomId.ToString();

        await Groups.AddToGroupAsync(connId, roomKey);
        _roomTracker.AddUserToRoom(roomKey, connId, username);

        var usernames = _roomTracker.GetUsernamesInRoom(roomKey);

        await Clients.Group(roomKey).SendAsync("UserJoined", username);
        await Clients.Group(roomKey).SendAsync("RoomUserListUpdated", usernames);
    }

    public async Task SubmitAnswer(int roomId, int questionId, string username)
    {
        await Clients.Group(roomId.ToString()).SendAsync("UserAnswered", questionId, username);
    }
}
