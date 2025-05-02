namespace QuizJourney.Services.RoomTracker
{
    public class RoomTrackerService : IRoomTrackerService
    {
        private readonly Dictionary<int, HashSet<string>> _roomUsers = new(); 
        private readonly Dictionary<string, (int RoomId, string Username)> _connections = new(); 

        public void AddUserToRoom(int roomId, string connectionId, string username)
        {
            if (roomId <= 0 || string.IsNullOrEmpty(username))
            {
                throw new ArgumentException("Room ID or Username cannot be null or empty");
            }

            if (!_roomUsers.ContainsKey(roomId))
            {
                _roomUsers[roomId] = new HashSet<string>();
            }

            _roomUsers[roomId].Add(username);
            _connections[connectionId] = (roomId, username);
        }

        public void RemoveUserFromRoom(string connectionId)
        {
            if (_connections.TryGetValue(connectionId, out var info))
            {
                int roomId = info.RoomId;
                string username = info.Username;

                Console.WriteLine($"[RemoveUserFromRoom] Removing '{username}' from room '{roomId}'");

                if (_roomUsers.TryGetValue(roomId, out var users))
                {
                    users.Remove(username);

                    if (users.Count == 0)
                    {
                        _roomUsers.Remove(roomId);
                        Console.WriteLine($"[RemoveUserFromRoom] Room '{roomId}' is now empty and removed.");
                    }
                }
                _connections.Remove(connectionId);
            }
        }

        public List<string> GetUsersInRoom(int roomId)
        {
            if (_roomUsers.TryGetValue(roomId, out var users))
            {
                return users.ToList();
            }

            return new List<string>();
        }

        public int? GetRoomByConnectionId(string connectionId)
        {
            return _connections.TryGetValue(connectionId, out var info) ? info.RoomId : (int?)null;
        }

        public string? GetUsernameByConnectionId(string connectionId)
        {
            return _connections.TryGetValue(connectionId, out var info) ? info.Username : null;
        }

        public bool RoomExists(int roomId)
        {
            return _roomUsers.ContainsKey(roomId);
        }
    }
}
