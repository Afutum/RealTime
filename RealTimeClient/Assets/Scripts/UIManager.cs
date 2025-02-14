using Assets.Model;
using RialTimeServer.Model.Entity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [SerializeField] UserModel userModel;
    [SerializeField] Text name;
    [SerializeField] RoomModel roomModel;
    [SerializeField] Text roomName;
    [SerializeField] GameObject userId;
    [SerializeField] GameObject inRoomButton;
    [SerializeField] GameObject ReadyButton;
    [SerializeField] TextMesh LeftGoal;
    [SerializeField] TextMesh RightGoal;
    [SerializeField] GameObject shootBtn;
    [SerializeField] GameObject goalEffect1;
    [SerializeField] GameObject goalEffect2;
    [SerializeField] GameObject goalEffect3;
    [SerializeField] Text goalText;
    [SerializeField] GameObject joyStick;
    [SerializeField] GameObject leftPlayer;
    [SerializeField] GameObject rightPlayer;
    [SerializeField] Text drowText;
    [SerializeField] Text timerReset;

    GameObject effect1;
    GameObject effect2;
    GameObject effect3;

    GameDirector gameDirector;
    BallDirector ball;

    public bool isShootArea {get; set;}

    public int leftGoalScore { get; set;}
    public int rightGoalScore { get; set;}

    public bool isDrow { get; set;}
    public int drowCount;
    public bool isStop { get; set;}

    // Start is called before the first frame update
    void Start()
    {
        gameDirector = GameObject.Find("GameDirector").GetComponent<GameDirector>();

        shootBtn.SetActive(false);
        joyStick.SetActive(false);
        goalText.enabled = false;

        leftPlayer.SetActive(false);
        rightPlayer.SetActive(false);

        isDrow = false;
        drowCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            PushShootBtn();
        }
    }

    /// <summary>
    /// 登録ボタンが押された
    /// </summary>
    public async void PushButton()
    {
        await userModel.RegistUserAsync();
    }

    /// <summary>
    /// 特定UIの非表示
    /// </summary>
    public void HideUI()
    {
        userId.SetActive(false);
        inRoomButton.SetActive(false);
    }

    public void DisplayUI()
    {
        userId.SetActive(true);
        inRoomButton.SetActive(true);
    }

    /// <summary>
    /// 準備完了ボタンが押された
    /// </summary>
    public void PushReady()
    {
        gameDirector.SetReady();
    }

    /// <summary>
    /// ゴールカウント
    /// </summary>
    /// <param name="leftGoalCnt"></param>
    /// <param name="rightGoalCnt"></param>
    public void GoalTextCount(int leftGoalCnt, int rightGoalCnt)
    {
        leftGoalScore = leftGoalCnt;
        rightGoalScore = rightGoalCnt;

        LeftGoal.text = "" + leftGoalCnt;
        RightGoal.text = "" + rightGoalCnt;
    }

    /// <summary>
    /// シュートボタンがプッシュされた
    /// </summary>
    public void PushShootBtn()
    {
        if (isShootArea)
        {
            ball.shoot();
        }
    }

    /// <summary>
    /// ゴール時のテキスト表示
    /// </summary>
    public void GoalText()
    {
        goalText.enabled = true;
        goalText.DOText("GOAL", 3.0f);

        Invoke(("DestoryEffect"), 3f);

        //ball.ResetBallPos();
    }

    /// <summary>
    /// エフェクトの削除
    /// </summary>
    public void DestoryEffect()
    {
        Destroy(effect1);
        Destroy(effect2);
        Destroy(effect3);

        if(goalText.enabled == true)
        {
            goalText.text = "";
            goalText.enabled = false;
            ball.ResetBallPos();
        }
    }

    /// <summary>
    /// ボールをセット
    /// </summary>
    /// <param name="ballObj"></param>
    public void SetBall(GameObject ballObj)
    {
        ball = ballObj.GetComponent<BallDirector>();
    }

    /// <summary>
    /// 特定UIの表示
    /// </summary>
    public void DisplayGameUI()
    {
        shootBtn.SetActive(true);
        joyStick.SetActive(true);
    }

    /// <summary>
    /// 特定UIの非表示
    /// </summary>
    public void HideGameUI()
    {
        shootBtn.SetActive(false);
        joyStick.SetActive(false);
    }

    /// <summary>
    /// 遅れてUIの表示
    /// </summary>
    public void DelayHideUI()
    {
        Invoke("HideGameUI", 1.0f);
    }

    /// <summary>
    /// リザルトの表示
    /// </summary>
    public void ResultScore()
    {
        if(leftGoalScore > rightGoalScore)
        {
            leftPlayer.SetActive(true);
            isDrow = false;
        }
        else if (leftGoalScore == rightGoalScore)
        {
            drowText.DOFade(1.0f, 1.5f);
            Invoke("HideDrow", 1.5f);
        }
        else if(rightGoalScore > leftGoalScore)
        {
            rightPlayer.SetActive(true);
            isDrow = false;
        }
    }

    /// <summary>
    /// エフェクトの表示
    /// </summary>
    public void DisplayEffect()
    {
        effect1 = Instantiate(goalEffect1);
        effect2 = Instantiate(goalEffect2);
        effect3 = Instantiate(goalEffect3);

        effect1.transform.position = new Vector3(-2.7f, 0, 0);
        effect2.transform.position = new Vector3(3.86f, 0, 0);
        effect3.transform.position = new Vector3(0, -2.92f, 0);
    }

    public void DisplayDrow()
    {
        isDrow = true;
        drowCount++;
        isStop = true;
        drowText.DOFade(1.0f, 1.5f);
        timerReset.DOFade(1.0f, 1.5f);
    }

    public void HideDrow()
    {
        isStop = false;
        drowText.DOFade(0.0f, 1.5f);
        timerReset.DOFade(0.0f, 1.5f);
    }
}
