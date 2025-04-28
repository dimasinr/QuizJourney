using System.Collections.Concurrent;

namespace QuizJourney.Services.RoomTracker;
public class RoomTrackerService : IRoomTrackerService
{
    // roomId -> set of usernames
    private readonly ConcurrentDictionary<string, HashSet<string>> _roomUsers = new();
    // connectionId -> (roomId, username)
    private readonly ConcurrentDictionary<string, (string roomId, string username)> _connections = new();

    public void AddUserToRoom(string roomId, string connectionId, string username)
    {
        _connections[connectionId] = (roomId, username);

        _roomUsers.AddOrUpdate(roomId,
            _ => new HashSet<string> { username },
            (_, set) =>
            {
                lock (set)
                {
                    set.Add(username);
                }
                return set;
            });
    }

    public void RemoveUser(string connectionId)
    {
        if (_connections.TryRemove(connectionId, out var info))
        {
            var (roomId, username) = info;
            if (_roomUsers.TryGetValue(roomId, out var set))
            {
                lock (set)
                {
                    set.Remove(username);
                }
            }
        }
    }

    public int GetRoomUserCount(string roomId)
    {
        if (_roomUsers.TryGetValue(roomId, out var set))
        {
            lock (set)
            {
                return set.Count;
            }
        }
        return 0;
    }

    public List<string> GetUsernamesInRoom(string roomId)
    {
        if (_roomUsers.TryGetValue(roomId, out var set))
        {
            lock (set)
            {
                return set.ToList();
            }
        }
        return new List<string>();
    }
}
