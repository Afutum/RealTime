using Assets.Model;
using Cysharp.Net.Http;
using Cysharp.Threading.Tasks;
using Grpc.Net.Client;
using MagicOnion.Client;
using Shared.Interfaces.StreamingHubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class RoomModel : BaseModel,IRoomHubReceiver
{
    private GrpcChannel channel;
    private IRoomHub roomHub;

    // 接続ID
    public Guid ConnectionId { get; set; }
    // ユーザー接続通知
    public Action<JoinedUser> OnJoinedUser {  get; set; }

    // ユーザー退室通知
    public Action<Guid> OnLeave {  get; set; }

    // ユーザー移動通知
    public Action<Guid,Vector3,Quaternion> OnMoveCharacter { get; set; }

    // ボール移動通知
    public Action<Vector3,Quaternion> OnBallMove { get; set; }

    // 準備完了通知
    public Action OnGameReady { get; set; }

    // MagicOnion接続処理
    public async UniTask ConnectAsync()
    {
        var handler = new YetAnotherHttpHandler() { Http2Only = true };
        channel = GrpcChannel.ForAddress(ServerURL, new GrpcChannelOptions() { HttpHandler = handler });
        roomHub = await StreamingHubClient.ConnectAsync<IRoomHub, IRoomHubReceiver>(channel, this);
    }

    // MagicOnion切断処理
    public async void DisconnectAsync()
    {
        if(roomHub != null) await roomHub.DisposeAsync();
        if(channel != null) await channel.ShutdownAsync();
        roomHub = null;channel = null;
    }

    // 破棄処理
    async void OnDestory()
    {
        DisconnectAsync();
    }

    // 入室
    public async UniTask JoinAsync(string roomName,int userId)
    {
        JoinedUser[] users = await roomHub.JoinAsync(roomName, userId);
        foreach(var user in users)
        {
            if (user.UserData.Id == userId) this.ConnectionId = user.ConnectionId;
            OnJoinedUser(user);
        }
    }

    // 入室通知(IRoomHubReceiverインタフェースの実装)
    public void OnJoin(JoinedUser user)
    {
        OnJoinedUser(user);
    }

    public async UniTask LeaveAsync()
    {
        await roomHub.LeaveAsync();
    }

    public void OnLeaveUser(Guid ConnectionId)
    {
        OnLeave(ConnectionId);
    }

    // 位置・回転を送信する
    public async Task MoveAsync(Vector3 pos, Quaternion rot)
    {
        await roomHub.MoveAsync(pos, rot);
    }

    public void OnMove(Guid ConnectionId, Vector3 pos, Quaternion rot)
    {
        OnMoveCharacter(ConnectionId, pos, rot);
    }

    public void OnMoveBall(Vector3 pos, Quaternion rot)
    {
        OnBallMove(pos, rot);
    }

    public async Task MoveBallAsync(Vector3 pos, Quaternion rot)
    {
        await roomHub.MoveBallAsync(pos,rot);
    }

    public async Task ReadyAsync()
    {
        await roomHub.ReadyAsync();
    }

    public void OnReady()
    {
        OnGameReady();
    }
}
