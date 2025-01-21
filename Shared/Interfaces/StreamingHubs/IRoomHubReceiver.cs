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

        public enum CharactorState
        {
            idle = 0,
            run = 1
        }

        // ユーザーの入室
        void OnJoin(JoinedUser user);

        // ユーザーの退室
        void OnLeaveUser(Guid ConnectionId);

        // ユーザーの移動
        void OnMove(Guid ConnectionId,Vector3 pos,Quaternion rot,CharactorState state);

        // ユーザー準備完了
        void OnReady();

        void OnMoveBall(Vector3 pos, Quaternion rot);

        void OnGoal(int leftGoalNum,int rightGoalNum);

        void OnShoot(Vector3 shootPow);

        //void OnStart(bool isStart);
    }
}
