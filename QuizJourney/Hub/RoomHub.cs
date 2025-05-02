using Microsoft.AspNetCore.SignalR;
using QuizJourney.Services.RoomTracker;

public class RoomHub : Hub
{
    private readonly IRoomTrackerService _roomTracker;

    public RoomHub(IRoomTrackerService roomTracker)
    {
        _roomTracker = roomTracker;
    }

    // Bergabung ke Room
    public async Task JoinRoom(int roomId, string username)
    {
        try
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentException("Username cannot be null or empty.");
            }

            _roomTracker.AddUserToRoom(roomId, Context.ConnectionId, username);

            var users = _roomTracker.GetUsersInRoom(roomId);

            await Clients.Group(roomId.ToString()).SendAsync("RoomUserListUpdated", users);
            await Clients.Group(roomId.ToString()).SendAsync("RoomUserCountUpdated", users.Count);
            Console.WriteLine("USER COUNT : " + users.Count);

            // Tambahkan client ke grup SignalR untuk room tersebut
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString());

            Console.WriteLine($"User {username} has joined the room {roomId}.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in JoinRoom: {ex.Message}");
            throw;
        }
    }

    public async Task LeaveRoom(int roomId, string username)
    {
        try
        {
            _roomTracker.RemoveUserFromRoom(Context.ConnectionId);

            var users = _roomTracker.GetUsersInRoom(roomId);

            await Clients.Group(roomId.ToString()).SendAsync("RoomUserListUpdated", users);
            await Clients.Group(roomId.ToString()).SendAsync("RoomUserCountUpdated", users.Count);

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId.ToString());

            Console.WriteLine($"User {username} has left the room {roomId}.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in LeaveRoom: {ex.Message}");
            throw;
        }
    }

    // Ketika pengguna terputus
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        try
        {
            var roomId = _roomTracker.GetRoomByConnectionId(Context.ConnectionId);
            var username = _roomTracker.GetUsernameByConnectionId(Context.ConnectionId);

            Console.WriteLine($"[OnDisconnectedAsync] ConnId: {Context.ConnectionId}, RoomId: {roomId}, Username: {username}");

            if (roomId == null || string.IsNullOrWhiteSpace(username))
            {
                Console.WriteLine("[OnDisconnectedAsync] Skipped: Connection has no associated room or username.");
                return;
            }

            _roomTracker.RemoveUserFromRoom(Context.ConnectionId);
            var users = _roomTracker.GetUsersInRoom((int)roomId);

            await Clients.Group(roomId.ToString()).SendAsync("RoomUserListUpdated", users);
            await Clients.Group(roomId.ToString()).SendAsync("RoomUserCountUpdated", users.Count);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[OnDisconnectedAsync] Error: {ex}");
        }

        await base.OnDisconnectedAsync(exception);
    }

    public async Task SubmitAnswer(int roomId, int questionId, string username)
    {
        try
        {
            await Clients.Group(roomId.ToString()).SendAsync("UserAnswered", questionId, username);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in SubmitAnswer: {ex.Message}");
            throw;
        }
    }
}
