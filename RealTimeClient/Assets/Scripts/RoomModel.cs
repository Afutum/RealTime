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

    // �ڑ�ID
    public Guid ConnectionId { get; set; }
    // ���[�U�[�ڑ��ʒm
    public Action<JoinedUser> OnJoinedUser {  get; set; }

    // ���[�U�[�ގ��ʒm
    public Action<Guid> OnLeave {  get; set; }

    // ���[�U�[�ړ��ʒm
    public Action<Guid,Vector3,Quaternion> OnMoveCharacter { get; set; }

    // �{�[���ړ��ʒm
    public Action<Vector3,Quaternion> OnBallMove { get; set; }

    // ���������ʒm
    public Action OnGameReady { get; set; }

    public Action OnGoalCnt {  get; set; }

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

    public async UniTask LeaveAsync()
    {
        await roomHub.LeaveAsync();
    }

    public void OnLeaveUser(Guid ConnectionId)
    {
        OnLeave(ConnectionId);
    }

    // �ʒu�E��]�𑗐M����
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

    public async Task GoalAsync()
    {
        await roomHub.GoalAsync();
    }

    public void OnGoal(int leftGoalNum, int rightGoalNum)
    {
        OnGoalCnt();
    }
}
