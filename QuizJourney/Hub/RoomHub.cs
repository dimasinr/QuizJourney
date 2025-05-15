using Microsoft.AspNetCore.SignalR;
using QuizJourney.Services.RoomTracker;
using QuizJourney.Data;
using QuizJourney.DTOs;
using QuizJourney.Models;            // untuk UserScore dan model lain
using Microsoft.EntityFrameworkCore; 

public class RoomHub : Hub
{
    private readonly IRoomTrackerService _roomTracker;
    private readonly ApplicationDbContext _context;

    public RoomHub(IRoomTrackerService roomTracker, ApplicationDbContext context)
    {
        _roomTracker = roomTracker;
        _context = context;
    }

    public async Task JoinRoom(int roomId, string username)
    {
        try
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentException("Username cannot be null or empty.");
            }
            // _roomTracker.RemoveUserFromRoom(Context.ConnectionId);
            // var usersRemoved = _roomTracker.GetUsersInRoom(roomId);
            // await Clients.Group(roomId.ToString()).SendAsync("RoomUserCountUpdated", usersRemoved.Count);

            await Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString());

            _roomTracker.AddUserToRoom(roomId, Context.ConnectionId, username);

            var users = _roomTracker.GetUsersInRoom(roomId);

            await Clients.Group(roomId.ToString()).SendAsync("RoomUserListUpdated", users);
            await Clients.Group(roomId.ToString()).SendAsync("RoomUserCountUpdated", users.Count);
            
            Console.WriteLine($"[DEBUG] AddUserToRoom: roomId={roomId}, connId={Context.ConnectionId}, username={username}");
            Console.WriteLine($"[DEBUG] Current users in room {roomId}: {string.Join(", ", users)}");

            Console.WriteLine($"[JoinRoom] {username} joined room {roomId} - now {users.Count} users.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[JoinRoom] Error: {ex.Message}");
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
            var scores = await _context.StudentAnswers
                .Include(sa => sa.Question)
                .Include(sa => sa.User)
                .Where(sa => sa.Question != null && sa.Question.RoomId == roomId)
                .GroupBy(sa => sa.User)
                .Select(g => new
                {
                    Username = g.Key!.Username,
                    Score = g.Sum(x => x.Score),
                    FirstAnswerTime = g.Min(x => x.CreatedAt)
                })
                .OrderByDescending(x => x.Score)
                .ThenBy(x => x.FirstAnswerTime)
                .Select(x => new UserScore
                {
                    Username = x.Username,
                    Score = (int)x.Score  
                })
                .ToListAsync();

            foreach(var score in scores)
            {
                Console.WriteLine($"Username: {score.Username}, Score: {score.Score}");
            }

            await Clients.Group(roomId.ToString()).SendAsync("ReceiveRanking", scores);
            Console.WriteLine($"[SubmitAnswer] Broadcasted 'ReceiveRanking' event to room {roomId}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[SubmitAnswer] Error: {ex.Message}");
            throw;
        }
    }
}
