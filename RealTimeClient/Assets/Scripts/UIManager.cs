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

    GameObject effect1;
    GameObject effect2;
    GameObject effect3;

    GameDirector gameDirector;

    BallDirector ball;

    public bool isShootArea {get; set;}

    public int leftGoalScore;
    public int rightGoalScore;

    // Start is called before the first frame update
    void Start()
    {
        gameDirector = GameObject.Find("GameDirector").GetComponent<GameDirector>();

        shootBtn.SetActive(false);
        joyStick.SetActive(false);
        goalText.enabled = false;

        leftPlayer.SetActive(false);
        rightPlayer.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            PushShootBtn();
        }
    }

    public void PushButton()
    {
        userModel.RegistUserAsync(name.text);
    }

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

    public void PushReady()
    {
        gameDirector.SetReady();
    }

    public void GoalTextCount(int leftGoalCnt, int rightGoalCnt)
    {
        leftGoalScore = leftGoalCnt;
        rightGoalScore = rightGoalCnt;

        LeftGoal.text = "" + leftGoalCnt;
        RightGoal.text = "" + rightGoalCnt;
    }

    public void PushShootBtn()
    {
        if (isShootArea)
        {
            ball.shoot();
        }
    }

    public void GoalText()
    {
        goalText.enabled = true;
        goalText.DOText("GOAL", 3.0f);

        Invoke(("DestoryEffect"), 3f);

        goalText.enabled = false;

        ball.ResetBallPos();
    }

    public void DestoryEffect()
    {
        Destroy(effect1);
        Destroy(effect2);
        Destroy(effect3);
    }

    public void SetBall(GameObject ballObj)
    {
        ball = ballObj.GetComponent<BallDirector>();
    }

    public void DisplayGameUI()
    {
        shootBtn.SetActive(true);
        joyStick.SetActive(true);
    }

    public void HideGameUI()
    {
        shootBtn.SetActive(false);
        joyStick.SetActive(false);
    }

    public void DelayHideUI()
    {
        Invoke("HideGameUI", 1.0f);
    }

    public void ResultScore()
    {
        if(leftGoalScore > rightGoalScore)
        {
            leftPlayer.SetActive(true);
        }
        else
        {
            rightPlayer.SetActive(true);
        }
    }

    public void DisplayEffect()
    {
        effect1 = Instantiate(goalEffect1);
        effect2 = Instantiate(goalEffect2);
        effect3 = Instantiate(goalEffect3);

        effect1.transform.position = new Vector3(-2.7f, 0, 0);
        effect2.transform.position = new Vector3(3.86f, 0, 0);
        effect3.transform.position = new Vector3(0, -2.92f, 0);
    }
}
