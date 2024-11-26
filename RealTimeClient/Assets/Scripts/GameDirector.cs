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

        roomModel.OnLeave += this.OnLeave;

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
        // �ގ�
        await roomModel.LeaveAsync();
    }

    // ���[�U�[�������������̏���
    private void OnJoinedUser(JoinedUser user)
    {
        GameObject characterObject = Instantiate(characterPrefab); // �C���X�^���X����
        characterObject.transform.position = new Vector3(0, 0, 0);
        characterList[user.ConnectionId] = characterObject; // �t�B�[���h�ŕێ�
    }

    private void OnLeave(Guid ConnectionId)
    {
        if (roomModel.ConnectionId == ConnectionId)
        {
            foreach (var character in characterList)
            {
                Destroy(character.Value);
            }
        }
        else
        {
            Destroy(characterList[ConnectionId]);
        }
    }
}
