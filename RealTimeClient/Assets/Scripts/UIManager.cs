using Assets.Model;
using RialTimeServer.Model.Entity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    GameDirector gameDirector;

    BallDirector ball;

    public bool isShootArea {get; set;}

    // Start is called before the first frame update
    void Start()
    {
        gameDirector = GameObject.Find("GameDirector").GetComponent<GameDirector>();

        shootBtn.SetActive(false);
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
        shootBtn.SetActive(true);
    }

    public void PushReady()
    {
        gameDirector.SetReady();
    }

    public void GoalTextCount(int leftGoalCnt, int rightGoalCnt)
    {
        LeftGoal.text = "" + leftGoalCnt;
        RightGoal.text = "" + rightGoalCnt;

    }

    public void PushShootBtn()
    {
        if (isShootArea)
        {
            ball = GameObject.Find("Ball(Clone)").gameObject.GetComponent<BallDirector>();

            ball.shoot();
        }
    }
}
