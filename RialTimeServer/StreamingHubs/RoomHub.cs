using MagicOnion.Server.Hubs;
using RialTimeServer.Model.Context;
using Shared.Interfaces.StreamingHubs;
using UnityEngine;


namespace RialTimeServer.StreamingHubs
{
    public class RoomHub:StreamingHubBase<IRoomHub,IRoomHubReceiver>,IRoomHub
    {
        private IGroup room;

        //準備完了
        public async Task ReadyAsync()
        {
            // 準備できたことを自分のRoomDataに保存
            var roomDataStorage = this.room.GetInMemoryStorage<RoomData>();
            lock (roomDataStorage)
            {
                var roomData = roomDataStorage.Get(this.ConnectionId);
                // roomDataで準備完了状態を保存しておく
                roomData.JoinedUser.IsReady = true;

                var roomDataList = roomDataStorage.AllValues.ToArray<RoomData>();

                foreach (var rmData in roomDataList)
                {
                    if(!rmData.JoinedUser.IsReady)
                    {// 全員が準備完了していなかったら
                        return;
                    }
                }

                // 全員にゲーム開始を通知
                this.Broadcast(room).OnReady();
            }
        }

        public async Task<JoinedUser[]> JoinAsync(string roomName,int userId)
        {
            // ルームに参加&ルームを保持
            this.room = await this.Group.AddAsync(roomName);

            // DBからユーザー情報取得
            GameDbContext context = new GameDbContext();
            var user = context.Users.Where(user => user.Id == userId).First();

            // グループストレージにユーザーデータを格納
            var roomStorage = this.room.GetInMemoryStorage<RoomData>();

            lock (roomStorage)
            {
                var joinedUser = new JoinedUser() { ConnectionId = this.ConnectionId, UserData = user };
                var roomData = new RoomData() { JoinedUser = joinedUser };
                roomStorage.Set(this.ConnectionId, roomData);

                RoomData[] roomDataList = roomStorage.AllValues.ToArray<RoomData>();

                joinedUser.JoinOrder = roomDataList.Length;

                // ルーム参加者全員に、ユーザー入室通知を送信
                this.BroadcastExceptSelf(room).OnJoin(joinedUser);

                // 参加中のユーザー情報を返す
                JoinedUser[] joinedUsersList = new JoinedUser[roomDataList.Length];

                for (int i = 0; i < roomDataList.Length; i++)
                {
                    joinedUsersList[i] = roomDataList[i].JoinedUser;
                }

                return joinedUsersList;
            }
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

        // 位置・回転をクライアントに通知する
        public async Task MoveAsync(Vector3 pos, Quaternion rot,IRoomHubReceiver.CharacterState state)
        {
            // グループストレージからRoomDataを取得して、位置と回転を保存
            var roomStorage = this.room.GetInMemoryStorage<RoomData>();
            var roomData = roomStorage.Get(this.ConnectionId);
            if (roomData != null)
            {
                roomData.pos = pos;
                roomData.rot = rot;

                // ローム内の他のユーザーに位置・回転の変更を送信
                this.BroadcastExceptSelf(room).OnMove(this.ConnectionId, pos, rot,state);
            }
        }

        public async Task MoveBallAsync(Vector3 pos, Quaternion rot)
        {
            // ローム内の他のユーザーに位置・回転の変更を送信
            this.BroadcastExceptSelf(room).OnMoveBall(pos, rot);
        }

        protected override ValueTask OnDisconnected()
        {
            if (this.room != null)
            {
                // ルームデータ削除
                this.room.GetInMemoryStorage<RoomData>().Remove(this.ConnectionId);
                // 退室したことを全メンバーに通知
                this.Broadcast(room).OnLeaveUser(this.ConnectionId);

                // ゲーム終了したことを自分以外に通知
                this.BroadcastExceptSelf(room).OnEndGame();

                // ルーム内のメンバーから削除
                room.RemoveAsync(this.Context);
            }
            return CompletedTask;
        }

        public async Task GoalAsync()
        {
            var roomStorage = this.room.GetInMemoryStorage<RoomData>();
            var roomData = roomStorage.Get(this.ConnectionId);
            roomData.GoalCount++;

            RoomData[] roomDataList = roomStorage.AllValues.ToArray<RoomData>();

            // ゴールしたことを全メンバーに通知
            this.Broadcast(room).OnGoal(roomDataList[0].GoalCount, roomDataList[1].GoalCount);
        }

        public async Task ShootAsync(Vector3 shootPow)
        {
            // ローム内の他のユーザーに位置・回転の変更を送信
            this.Broadcast(room).OnShoot(shootPow);
        }

        public async Task StartGameAsync()
        {
            var roomStorage = this.room.GetInMemoryStorage<RoomData>();
            var roomData = roomStorage.Get(this.ConnectionId);
            roomData.isGameStart = true;

            RoomData[] roomDataList = roomStorage.AllValues.ToArray<RoomData>();

            bool isStartGame = true;

            for (int i = 0; i < roomDataList.Length; i++)
            {
                if (roomDataList[i].isGameStart)
                {
                    isStartGame = true;
                }
                else
                {
                    isStartGame = false;
                }
            }

            if (isStartGame)
            {
                this.Broadcast(room).OnStart();
            }
        }

        public async Task EndGameAsync()
        {
            var roomStorage = this.room.GetInMemoryStorage<RoomData>();
            var roomData = roomStorage.Get(this.ConnectionId);

            lock (roomStorage)
            {
                roomData.isGameStart = false;

                RoomData[] roomDataList = roomStorage.AllValues.ToArray<RoomData>();

                bool isEndGame = true;

                for (int i = 0; i < roomDataList.Length; i++)
                {
                    if (roomDataList[i].isGameStart == true)
                    {
                        isEndGame = false;
                    }
                }

                if (isEndGame)
                {
                    this.Broadcast(room).OnEndGame();
                }
            }
        }

        public async Task<JoinedUser[]> JoinLobbyAsync(int userId)
        {
            JoinedUser[] joinedUserList = await JoinAsync("Lobby", userId);

            if(joinedUserList.Length == 2)
            {
                Guid guidValue = Guid.NewGuid();
                this.Broadcast(room).OnMatching(guidValue.ToString());
            }

            return joinedUserList;
        }
    }
}
