using Cysharp.Net.Http;
using Cysharp.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using MagicOnion.Client;
using Shared.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Model
{
    internal class UserModel:BaseModel
    {
        private int userId; // 登録ユーザーID

        public async UniTask<bool> RegistUserAsync(string name)
        {
            var handler = new YetAnotherHttpHandler() { Http2Only = true};
            var channel = GrpcChannel.ForAddress(ServerURL, new GrpcChannelOptions() { HttpHandler = handler });
            var cliant = MagicOnionClient.Create<IUserService>(channel);
            try
            {// 登録成功
                userId = await cliant.RegistUserAsync(name);
                Debug.Log("成功");
                return true;
            }
            catch (RpcException e)
            {// 登録失敗
                Debug.Log(e);
                Debug.Log("失敗");
                return false;
            }
        }
    }
}
