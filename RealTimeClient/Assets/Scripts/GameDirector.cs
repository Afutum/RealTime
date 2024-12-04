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

    async void Start()
    {
        // ���[�U�[�����������Ƃ���OnJoinedUser���\�b�h�����s����悤�A���f���ɓo�^���Ă���
        roomModel.OnJoinedUser += this.OnJoinedUser;

        // ���[�U�[���ގ���������OnLeave���\�b�h�����s����悤�A���f���ɓo�^
        roomModel.OnLeave += this.OnLeave;

        roomModel.OnMoveCharacter += this.OnMove;

        // �ڑ�
        await roomModel.ConnectAsync();
    }

    public async void JoinRoom()
    {
        // ����
        await roomModel.JoinAsync("sampleRoom", int.Parse(userId.text));
    }

    public async void ExitRoom()
    {
        await roomModel.LeaveAsync();
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

        characterObject.transform.position = new Vector3(0, 0, 0);
        transform.rotation = Quaternion.identity;
        characterList[user.ConnectionId] = characterObject; // �t�B�[���h�ŕێ�
    }

    private void OnLeave(Guid ConnectionId)
    {
        if(roomModel.ConnectionId == ConnectionId)
        {
            foreach (var character in characterList)
            {
                Destroy(character.Value);
            }

            CancelInvoke("SendMove");
        }
        else
        {
            Destroy(characterList[ConnectionId]);
        }
    }

    private void OnMove(Guid ConnectionId,Vector3 pos,Quaternion rot)
    {
        characterList[ConnectionId].transform. = pos;
        characterList[ConnectionId].transform.rotation = rot;
    }

    private async void SendMove()
    {
       await roomModel.MoveAsync(characterList[roomModel.ConnectionId].transform.position, characterList[roomModel.ConnectionId].transform.rotation);
    }
}
