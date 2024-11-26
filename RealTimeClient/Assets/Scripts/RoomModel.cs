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

public class RoomModel : BaseModel,IRoomHubReceiver
{
    private GrpcChannel channel;
    private IRoomHub roomHub;

    // �ڑ�ID
    public Guid ConnectionId { get; set; }
    // ���[�U�[�ڑ��ʒm
    public Action<JoinedUser> OnJoinedUser {  get; set; }

    public Action<Guid> OnLeave {  get; set; }

    // MagicOnion�ڑ�����
    public async UniTask ConnectAsync()
    {
        var handler = new YetAnotherHttpHandler() { Http2Only = true };
        channel = GrpcChannel.ForAddress(ServerURL, new GrpcChannelOptions() { HttpHandler = handler });
        roomHub = await StreamingHubClient.ConnectAsync<IRoomHub, IRoomHubReceiver>(channel, this);
    }

    // MagicOnion�ؒf����
    public async void DisconnectAsync()
    {
        if(roomHub != null) await roomHub.DisposeAsync();
        if(channel != null) await channel.ShutdownAsync();
        roomHub = null;channel = null;
    }

    // �j������
    async void OnDestory()
    {
        DisconnectAsync();
    }

    // ����
    public async UniTask JoinAsync(string roomName,int userId)
    {
        JoinedUser[] users = await roomHub.JoinAsync(roomName, userId);
        foreach(var user in users)
        {
            if (user.UserData.Id == userId) this.ConnectionId = user.ConnectionId;
            OnJoinedUser(user);
        }
    }

    // �����ʒm(IRoomHubReceiver�C���^�t�F�[�X�̎���)
    public void OnJoin(JoinedUser user)
    {
        OnJoinedUser(user);
    }

   // �ގ�
   public async UniTask LeaveAsync()
    {
        await roomHub.LeaveAsync();
    }

    // �ގ��ʒm
    public void OnLeaveUser(Guid ConnectionId)
    {
        OnLeave(ConnectionId);
    }
}
