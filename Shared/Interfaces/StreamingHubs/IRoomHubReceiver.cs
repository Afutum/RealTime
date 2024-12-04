using System;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;
using System.Text;

namespace Shared.Interfaces.StreamingHubs
{
    public interface IRoomHubReceiver
    {
        //[ここにサーバー側からクライアント側を呼び出す関数を定義する]

        // ユーザーの入室
        void OnJoin(JoinedUser user);

        // ユーザーの退室
        void OnLeaveUser(Guid ConnectionId);

        // ユーザーの移動
        void OnMove(Guid ConnectionId,Vector3 pos,Quaternion rot);
    }
}
