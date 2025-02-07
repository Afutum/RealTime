using DG.Tweening;
using Shared.Interfaces.StreamingHubs;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class TitleManager : MonoBehaviour
{
    [SerializeField] Text playText;
    [SerializeField] Text userId;
    [SerializeField] RoomModel roomModel;

    public class RoomName
    {
        public static string joinRoomName;
        public static string userId;
    }

    // Start is called before the first frame update
    async void Start()
    {
        roomModel.OnMatchingUser += OnMatchingUser;
        roomModel.OnLeave += this.OnLeave;

        // 接続
        await roomModel.ConnectAsync();

        // Sequenceのインスタンスを作成
        var sequence = DOTween.Sequence();

        sequence.Append(playText.DOColor(Color.cyan, 5f)); // 5秒かけて青に
                                                              // Sequenceを実行
        sequence.Play();

        playText.DOFade(0.0f, 3f)   // アルファ値を0にしていく
                       .SetLoops(-1, LoopType.Yoyo);    // 行き来を無限に繰り返す
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            playText.DOFade(0.0f, 0.1f)   // アルファ値を0にしていく
                       .SetLoops(-1, LoopType.Yoyo);    // 行き来を無限に繰り返す
        }
    }

    /// <summary>
    /// シーン遷移
    /// </summary>
    public void ChangeScene()
    {
        SceneManager.LoadScene("Game");
    }

    /// <summary>
    /// ロビー入室
    /// </summary>
    public async void JoinLobbyAsync()
    {
       await roomModel.JoinLobbyAsync(int.Parse(userId.text));
    }

    /// <summary>
    /// サーバーからマッチングの通知受け取り後のルーム退出処理
    /// </summary>
    /// <param name="roomName"></param>
    private async void OnMatchingUser(string roomName)
    {
        RoomName.joinRoomName = roomName;
        RoomName.userId = userId.text;

        await roomModel.LeaveAsync();
    }

    /// <summary>
    /// サーバーからロビー退出通知が来た後の処理
    /// </summary>
    /// <param name="ConnectionId"></param>
    private void OnLeave(Guid ConnectionId)
    {
        ChangeScene();
    }
}
