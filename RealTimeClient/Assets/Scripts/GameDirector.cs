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

        roomModel.OnLeave += this.OnLeave;

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
        // 退室
        await roomModel.LeaveAsync();
    }

    // ユーザーが入室した時の処理
    private void OnJoinedUser(JoinedUser user)
    {
        GameObject characterObject = Instantiate(characterPrefab); // インスタンス生成
        characterObject.transform.position = new Vector3(0, 0, 0);
        characterList[user.ConnectionId] = characterObject; // フィールドで保持
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
