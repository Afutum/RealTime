﻿using MagicOnion;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Interfaces.StreamingHubs
{
    public interface IRoomHub:IStreamingHub<IRoomHub,IRoomHubReceiver>
    {
        //[ここにクライアント側からサーバー側を呼び出す関数を定義する]

        // ユーザー入室
        Task<JoinedUser[]> JoinAsync(string roomName, int userId);
    }
}
