﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Interfaces.StreamingHubs
{
    public interface IRoomHubReceiver
    {
        //[ここにサーバー側からクライアント側を呼び出す関数を定義する]

        // ユーザーの入室
        void OnJoin(JoinedUser user);

        void OnLeaveUser(Guid ConnectionId);
    }
}
