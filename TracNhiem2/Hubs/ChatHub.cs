using Microsoft.AspNetCore.SignalR;
using TracNhiem2.Controllers;
using TracNhiem2.Data;
using TracNhiem2.Models;
using static System.Formats.Asn1.AsnWriter;

namespace TracNhiem2.Hubs
{
    public class ChatHub : Hub
    {
        private readonly TracNhiem2Context _context;
        private readonly static Dictionary<string, int> IdConnect_IdPlayer = new Dictionary<string, int>();
        private readonly static Dictionary<string, string> IdConnect_IdRoom = new Dictionary<string, string>();
        public ChatHub(TracNhiem2Context context)
        {
            _context = context;
        }
     

        public async Task GameStart(string room)
        {            
            await Clients.Group(room).SendAsync("StartGame").ConfigureAwait(true);

        }
        public async Task ReplayQuiz(string room)
        {
            await Clients.Group(room).SendAsync("replayGame").ConfigureAwait(true);

        }
        public Task JoinRoom(string roomName)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, roomName);
        }

        public Task LeaveRoom(string roomName)
        {
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
        }
        public async Task PlayerConnected(string idRoom, int idPlayer, string name)
        {
            var connectionId = Context.ConnectionId;
            IdConnect_IdPlayer.Add(connectionId, idPlayer);
            IdConnect_IdRoom.Add(connectionId, idRoom);


            var player_room = _context.RoomQuizView.FirstOrDefault(p => p.RoomId == int.Parse(idRoom) && p.PlayerId == idPlayer);
            if (player_room == null)
            {
                player_room = new RoomQuizView();
                player_room.RoomId = int.Parse(idRoom);
                player_room.PlayerId = idPlayer;
                _context.RoomQuizView.Add(player_room);
                _context.SaveChanges();
            }
            else
            {
                player_room.score = 0;
                _context.RoomQuizView.Update(player_room);
                _context.SaveChanges();

            }
            await JoinRoom(idRoom).ConfigureAwait(false);
            await Clients.Group(idRoom).SendAsync("NewUser", name).ConfigureAwait(true);
        }
        public async Task UpdateScore(string idRoom)
        {
            await Clients.Group(idRoom).SendAsync("update_sc").ConfigureAwait(true);
        }

        
        public override Task OnDisconnectedAsync(Exception exception)
        {
            try
            {
                var connectionId = Context.ConnectionId;
                var idPlayer = IdConnect_IdPlayer[connectionId];
                var idRoom = IdConnect_IdRoom[connectionId];
                IdConnect_IdPlayer.Remove(connectionId);
                IdConnect_IdRoom.Remove(connectionId);

                if (_context.RoomQuiz.Any(r => r.RoomId == int.Parse(idRoom) && r.AdminId == idPlayer))
                {
                    Clients.Group(idRoom).SendAsync("AdminLeave").ConfigureAwait(true);
                    RoomQuizController roomctl = new RoomQuizController(_context);
                    roomctl.DeleteRoom(int.Parse(idRoom));
                }
                else
                {
                    var player = _context.RoomQuizView.Where(r => r.PlayerId == idPlayer).ToList();
                    _context.RoomQuizView.RemoveRange(player);

                    int numberOfRecordsChanged = _context.SaveChanges();
                    // Tell other users to remove you from their list
                    Clients.Group(idRoom).SendAsync("RemoveUser").ConfigureAwait(true);
                }


            }
            catch (Exception ex)
            {
                 Clients.Caller.SendAsync("onError", "OnDisconnected: " + ex.Message);
            }

            return base.OnDisconnectedAsync(exception);
        }

    }
}
