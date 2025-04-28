using Microsoft.AspNetCore.SignalR;

public class RoomHub : Hub
{
    public async Task JoinRoom(string roomCode, string username)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, roomCode);
        await Clients.Group(roomCode).SendAsync("UserJoined", username);
    }

    public async Task SubmitAnswer(string roomCode, int questionId, string username)
    {
        await Clients.Group(roomCode).SendAsync("UserAnswered", questionId, username);
    }
}
