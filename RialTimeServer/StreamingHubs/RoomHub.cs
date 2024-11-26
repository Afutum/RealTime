using MagicOnion.Server.Hubs;
using RialTimeServer.Model.Context;
using RialTimeServer.Model.Entity;
using Shared.Interfaces.StreamingHubs;


namespace RialTimeServer.StreamingHubs
{
    public class RoomHub:StreamingHubBase<IRoomHub,IRoomHubReceiver>,IRoomHub
    {
        private IGroup room;
        public async Task<JoinedUser[]> JoinAsync(string roomName,int userId)
        {
            // ルームに参加&ルームを保持
            this.room = await this.Group.AddAsync(roomName);

            // DBからユーザー情報取得
            GameDbContext context = new GameDbContext();
            var user = context.Users.Where(user => user.Id == userId).First();

            // グループストレージにユーザーデータを格納
            var roomStorage = this.room.GetInMemoryStorage<RoomData>();
            var joinedUser = new JoinedUser() { ConnectionId = this.ConnectionId, UserData = user};
            var roomData = new RoomData() { JoinedUser = joinedUser };  
            roomStorage.Set(this.ConnectionId,roomData);

            // ルーム参加者全員に、ユーザー入室通知を送信
            this.BroadcastExceptSelf(room).OnJoin(joinedUser);

            RoomData[] roomDataList = roomStorage.AllValues.ToArray<RoomData>();
            // 参加中のユーザー情報を返す
            JoinedUser[] joinedUsersList = new JoinedUser[roomDataList.Length];

            for(int i = 0; i < roomDataList.Length; i++)
            {
                joinedUsersList[i] = roomDataList[i].JoinedUser;
            }

            return joinedUsersList;
        }

        public async Task LeaveAsync()
        {
            // グループから削除
            this.room.GetInMemoryStorage<RoomData>().Remove(this.ConnectionId);

            // ルーム参加者全員に、ユーザー退室通知を送信
            this.Broadcast(room).OnLeaveUser(this.ConnectionId);

            // ルーム内のメンバーから自分を削除
            await room.RemoveAsync(this.Context);
        }
    }
}
