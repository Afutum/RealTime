using Shared.Interfaces.StreamingHubs;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameDirector : MonoBehaviour
{
    [SerializeField] GameObject characterPrefab;
    [SerializeField] RoomModel roomModel;
    Dictionary<Guid,GameObject> characterList = new Dictionary<Guid,GameObject>();
    [SerializeField] Text userId;
    [SerializeField] GameObject ballPrefab;

    GameObject ball;

    UIManager manager;

    int joinOrder;

    async void Start()
    {
        // ���[�U�[�����������Ƃ���OnJoinedUser���\�b�h�����s����悤�A���f���ɓo�^
        roomModel.OnJoinedUser += this.OnJoinedUser;

        // ���[�U�[���ގ���������OnLeave���\�b�h�����s����悤�A���f���ɓo�^
        roomModel.OnLeave += this.OnLeave;

        // ���[�U�[���������Ƃ���OnMove���\�b�h�����s����悤�A���f���ɓo�^
        roomModel.OnMoveCharacter += this.OnMove;

        roomModel.OnGameReady += this.OnReady;

        roomModel.OnBallMove += this.OnMoveBall;

        manager = GameObject.Find("UIManager").GetComponent<UIManager>();

        // �ڑ�
        await roomModel.ConnectAsync();
    }

    public async void JoinRoom()
    {
        // ����
        await roomModel.JoinAsync("sampleRoom", int.Parse(userId.text));

        // UI���\��
        manager.HideUI();
    }

    // ���[�U�[�ޏo
    public async void ExitRoom()
    {
        await roomModel.LeaveAsync();
        // UI��\��
        manager.DisplayUI();
    }

    // ���[�U�[�������������̏���
    private void OnJoinedUser(JoinedUser user)
    {
        GameObject characterObject = Instantiate(characterPrefab); // �C���X�^���X����

        if(user.ConnectionId == roomModel.ConnectionId)
        {
            characterObject.GetComponent<Character>().isSelf = true;
            InvokeRepeating("SendMove", 0.1f,0.1f);
        }

        joinOrder = user.JoinOrder;

        characterObject.transform.position = new Vector3(0, 0, 0);
        transform.rotation = Quaternion.identity;
        characterList[user.ConnectionId] = characterObject; // �t�B�[���h�ŕێ�

        if (joinOrder == 1)
        {
            ball = Instantiate(ballPrefab);
        }
    }

    // �ޏo���[�U�[�폜
    private void OnLeave(Guid ConnectionId)
    {
        if(roomModel.ConnectionId == ConnectionId)
        {
            foreach (var character in characterList)
            {
                Destroy(character.Value);
            }

            Destroy(ball);

            CancelInvoke("SendMove");
        }
        else
        {
            Destroy(characterList[ConnectionId]);
        }
    }

    // �ړ�
    private void OnMove(Guid ConnectionId,Vector3 pos,Quaternion rot)
    {
        characterList[ConnectionId].transform.position = pos;
        characterList[ConnectionId].transform.rotation = rot;
    }

    private async void SendMove()
    {
        if (joinOrder == 1)
        {
            await roomModel.MoveBallAsync(ball.transform.position, ball.transform.rotation);
        }
        await roomModel.MoveAsync(characterList[roomModel.ConnectionId].transform.position, characterList[roomModel.ConnectionId].transform.rotation);
    }

    private void OnMoveBall(Vector3 pos, Quaternion rot)
    {
        ball.transform.position = pos;
        ball.transform.rotation = rot;
    }

    public async void SetReady()
    {
        await roomModel.ReadyAsync();
    }

    public void OnReady()
    {

    }
}