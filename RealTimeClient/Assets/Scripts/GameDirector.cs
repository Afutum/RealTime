using Shared.Interfaces.StreamingHubs;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine.TextCore.Text;
using UnityEngine.SceneManagement;
using RialTimeServer.Model.Entity;


public class GameDirector : MonoBehaviour
{
    [SerializeField] GameObject characterPrefab;
    [SerializeField] RoomModel roomModel;
    Dictionary<Guid,GameObject> characterList = new Dictionary<Guid,GameObject>();
    [SerializeField] Text userId;
    [SerializeField] GameObject ballPrefab;
    [SerializeField] TimerDirector timer;

    GameObject ball;

    public Vector3 InitBallPos;

    UIManager manager;
    Character chara;

    public int joinOrder { get; set; }

    public bool isStart;
    public bool isEnd;

    async void Start()
    {
        // ユーザーが入室したときにOnJoinedUserメソッドを実行するよう、モデルに登録
        roomModel.OnJoinedUser += this.OnJoinedUser;

        // ユーザーが退室した時にOnLeaveメソッドを実行するよう、モデルに登録
        roomModel.OnLeave += this.OnLeave;

        // ユーザーが動いたときにOnMoveメソッドを実行するよう、モデルに登録
        roomModel.OnMoveCharacter += this.OnMove;

        roomModel.OnBallMove += this.OnMoveBall;

        roomModel.OnGoalCnt += this.OnGoal;

        roomModel.OnGameStart += this.OnStartGame;

        roomModel.OnGameEnd += this.OnEndGame;

        manager = GameObject.Find("UIManager").GetComponent<UIManager>();

        isStart = false;
        isEnd = false;

        // 接続
        await roomModel.ConnectAsync();

        // ルームに入室
        JoinRoom();
    }

    /// <summary>
    /// ルーム入室処理
    /// </summary>
    public async void JoinRoom()
    {
        string roomName = TitleManager.RoomName.joinRoomName;
        string userId = TitleManager.RoomName.userId;

        // 入室
        await roomModel.JoinAsync(roomName, int.Parse(userId));

        // UIを非表示
        manager.HideUI();
    }

    /// <summary>
    /// ユーザー退出
    /// </summary>
    public async void ExitRoom()
    {
        await roomModel.LeaveAsync();

        // UIを表示
        manager.DisplayUI();
    }

    // タイトルへ戻る
    /*public async void BackTitle()
    {
        await roomModel.LeaveAsync();

        SceneManager.LoadScene("Title");
    }*/

    /// <summary>
    /// ユーザーが入室した時の処理
    /// </summary>
    /// <param name="user"></param>
    private async void OnJoinedUser(JoinedUser user)
    {
        GameObject characterObject = Instantiate(characterPrefab); // インスタンス生成
        chara = characterObject.GetComponent<Character>();

        if(user.ConnectionId == roomModel.ConnectionId)
        {
            characterObject.GetComponent<Character>().isSelf = true;
            InvokeRepeating("SendMove", 0.1f,0.1f);
            joinOrder = user.JoinOrder;
        }

        if(user.JoinOrder == 1)
        {
            chara.InitPosition(new Vector3(-7.95f, 0, 1.93f), Quaternion.Euler(0,90,0));
        }
        else if(user.JoinOrder == 2)
        {
            chara.InitPosition(new Vector3(7.95f, 0, 1.93f), Quaternion.Euler(0, 270, 0));
        }

        transform.rotation = Quaternion.identity;
        characterList[user.ConnectionId] = characterObject; // フィールドで保持
        characterList[user.ConnectionId].GetComponent<Character>().connectionId = user.ConnectionId;

        if (user.JoinOrder == 1)
        {
            ball = Instantiate(ballPrefab);

            manager.SetBall(ball);

            InitBallPos = ball.transform.position;
        }

        if(characterList.Count >= 2)
        {
            roomModel.StartGameAsync();
        }
    }

    /// <summary>
    /// 退出ユーザー削除
    /// </summary>
    /// <param name="ConnectionId"></param>
    private void OnLeave(Guid ConnectionId)
    {
        if(roomModel.ConnectionId == ConnectionId)
        {
            foreach (var character in characterList)
            {
                Destroy(character.Value);
            }

            characterList.Clear();

            Destroy(ball);

            CancelInvoke("SendMove");
        }
        else
        {
            if (characterList.ContainsKey(ConnectionId))
            {
                Destroy(characterList[ConnectionId]);
                characterList.Remove(ConnectionId);
            }
        }
    }

    /// <summary>
    /// プレイヤーの移動通知
    /// </summary>
    /// <param name="ConnectionId"></param>
    /// <param name="pos"></param>
    /// <param name="rot"></param>
    /// <param name="state"></param>
    private void OnMove(Guid ConnectionId,Vector3 pos,Quaternion rot,int state)
    {
        if (characterList.ContainsKey(ConnectionId))
        {
            characterList[ConnectionId].transform.DOLocalMove(pos, 0.1f);
            characterList[ConnectionId].transform.DOLocalRotateQuaternion(rot, 0.1f);
            characterList[ConnectionId].GetComponent<Character>().state = (Character.CharacterState)state;
        }
    }

    /// <summary>
    /// 移動
    /// </summary>
    private async void SendMove()
    {
        if (joinOrder == 1)
        {
            await roomModel.MoveBallAsync(ball.transform.position, ball.transform.rotation);
        }

        if (characterList.ContainsKey(roomModel.ConnectionId))
        {
            await roomModel.MoveAsync(characterList[roomModel.ConnectionId].transform.position,
                characterList[roomModel.ConnectionId].transform.rotation,
                (IRoomHubReceiver.CharacterState)characterList[roomModel.ConnectionId].GetComponent<Character>().state);
        }
    }

    /// <summary>
    /// ボールの移動通知受け取り
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="rot"></param>
    private void OnMoveBall(Vector3 pos, Quaternion rot)
    {
        if (ball != null)
        {
            ball.transform.transform.DOLocalMove(pos, 0.1f);
            ball.transform.transform.DOLocalRotateQuaternion(rot, 0.1f);
        }
    }

    /// <summary>
    /// 準備完了
    /// </summary>
    /// <param name="leftGoalNum"></param>
    /// <param name="rightGoalNum"></param>
    public async void SetReady()
    {
        await roomModel.ReadyAsync();
    }

    /// <summary>
    /// ゴール通知受け取り
    /// </summary>
    /// <param name="leftGoalNum"></param>
    /// <param name="rightGoalNum"></param>
    public void OnGoal(int leftGoalNum, int rightGoalNum)
    {
        manager.GoalTextCount(leftGoalNum, rightGoalNum);

        manager.DisplayEffect();

        manager.GoalText();
    }

    /// <summary>
    /// ゲームスタート通知受け取り
    /// </summary>
    public void OnStartGame()
    {
        isEnd = false;
        isStart = true;
        manager.DisplayGameUI();
    }

    /// <summary>
    /// プレイヤーの位置リセット
    /// </summary>
    public void ResetCharaPos()
    {
        foreach(var character in characterList)
        {
            character.Value.GetComponent<Character>().ResetPos();
        }
    }

    /// <summary>
    /// ゲーム終了通知受け取り
    /// </summary>
    public void OnEndGame()
    {
        isStart = false;
        manager.isDrow = false;
        isEnd = true;

        manager.DelayHideUI();
        timer.ResetTimer();
        GameResult();
    }

    /// <summary>
    /// ゲームリザルト
    /// </summary>
    public void GameResult()
    {
        manager.ResultScore();
        if (manager.isDrow == false)
        {
            Invoke("ExitRoom", 3.0f);
        }
    }

    /// <summary>
    /// ゲーム終了
    /// </summary>
    public void EndGame()
    {
        roomModel.EndGameAsync();
    }
}
