using MagicOnion;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Interfaces.StreamingHubs
{
    public interface IRoomHub:IStreamingHub<IRoomHub,IRoomHubReceiver>
    {
        //[ここにクライアント側からサーバー側を呼び出す関数を定義する]

        // ユーザー入室
        Task<JoinedUser[]> JoinAsync(string roomName, int userId);

        // ユーザー退室
        Task LeaveAsync();

        // ユーザー移動
        Task MoveAsync(Vector3 pos, Quaternion rot,IRoomHubReceiver.CharacterState state);

        // ユーザー準備完了
        Task ReadyAsync();

        Task MoveBallAsync(Vector3 pos, Quaternion rot);

        Task GoalAsync();

        Task ShootAsync(Vector3 shootPow);

        Task StartGameAsync();

        Task EndGameAsync();

        Task<JoinedUser[]> JoinLobbyAsync(int userId);
    }
}
