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
        // ユーザーが入室したときにOnJoinedUserメソッドを実行するよう、モデルに登録しておく
        roomModel.OnJoinedUser += this.OnJoinedUser;

        // ユーザーが退室した時にOnLeaveメソッドを実行するよう、モデルに登録
        roomModel.OnLeave += this.OnLeave;

        roomModel.OnMoveCharacter += this.OnMove;

        // 接続
        await roomModel.ConnectAsync();
    }

    public async void JoinRoom()
    {
        // 入室
        await roomModel.JoinAsync("sampleRoom", int.Parse(userId.text));
    }

    public async void ExitRoom()
    {
        await roomModel.LeaveAsync();
    }

    // ユーザーが入室した時の処理
    private void OnJoinedUser(JoinedUser user)
    {
        GameObject characterObject = Instantiate(characterPrefab); // インスタンス生成

        if(user.ConnectionId == roomModel.ConnectionId)
        {
            characterObject.GetComponent<Character>().isSelf = true;
            InvokeRepeating("SendMove", 0.1f,0.1f);
        }

        characterObject.transform.position = new Vector3(0, 0, 0);
        transform.rotation = Quaternion.identity;
        characterList[user.ConnectionId] = characterObject; // フィールドで保持
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
