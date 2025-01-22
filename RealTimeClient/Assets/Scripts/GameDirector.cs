using Shared.Interfaces.StreamingHubs;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class GameDirector : MonoBehaviour
{
    [SerializeField] GameObject characterPrefab;
    [SerializeField] RoomModel roomModel;
    Dictionary<Guid,GameObject> characterList = new Dictionary<Guid,GameObject>();
    [SerializeField] Text userId;
    [SerializeField] GameObject ballPrefab;

    GameObject ball;

    public Vector3 InitBallPos;

    UIManager manager;
    Character chara;

    public int joinOrder { get; set; }

    public bool isStart;

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

        manager = GameObject.Find("UIManager").GetComponent<UIManager>();

        isStart = false;

        // 接続
        await roomModel.ConnectAsync();
    }

    public async void JoinRoom()
    {
        // 入室
        await roomModel.JoinAsync("sampleRoom", int.Parse(userId.text));

        // UIを非表示
        manager.HideUI();
    }

    // ユーザー退出
    public async void ExitRoom()
    {
        await roomModel.LeaveAsync();
        // UIを表示
        manager.DisplayUI();
    }

    // ユーザーが入室した時の処理
    private async void OnJoinedUser(JoinedUser user)
    {
        GameObject characterObject = Instantiate(characterPrefab); // インスタンス生成

        if(user.ConnectionId == roomModel.ConnectionId)
        {
            characterObject.GetComponent<Character>().isSelf = true;
            InvokeRepeating("SendMove", 0.1f,0.1f);
            joinOrder = user.JoinOrder;
        }

        characterObject.transform.position = new Vector3(0, 0, 0);
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

    // 退出ユーザー削除
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

    // 移動
    private void OnMove(Guid ConnectionId,Vector3 pos,Quaternion rot,int state)
    {
        characterList[ConnectionId].transform.DOLocalMove(pos,0.1f);
        characterList[ConnectionId].transform.DOLocalRotateQuaternion(rot,0.1f);
        characterList[ConnectionId].GetComponent<Character>().state = (Character.CharactorState)state;
    }

    private async void SendMove()
    {
        if (joinOrder == 1)
        {
            await roomModel.MoveBallAsync(ball.transform.position, ball.transform.rotation);
        }

        await roomModel.MoveAsync(characterList[roomModel.ConnectionId].transform.position, 
            characterList[roomModel.ConnectionId].transform.rotation,(IRoomHubReceiver.CharactorState)characterList[roomModel.ConnectionId].GetComponent<Character>().state);
    }

    private void OnMoveBall(Vector3 pos, Quaternion rot)
    {
        ball.transform.transform.DOLocalMove(pos, 0.1f);
        ball.transform.transform.DOLocalRotateQuaternion(rot, 0.1f);
    }

    public async void SetReady()
    {
        await roomModel.ReadyAsync();
    }

    public void OnGoal(int leftGoalNum, int rightGoalNum)
    {
        manager.GoalTextCount(leftGoalNum, rightGoalNum);

        manager.GoalEffect();
    }

    public void OnStartGame()
    {
        isStart = true;
    }
}
