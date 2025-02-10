using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TimerDirector : MonoBehaviour
{
    [SerializeField] TextMesh timerText;
    [SerializeField] RoomModel roomModel;

    GameDirector gameDirector;
    UIManager uiManager;
    BallDirector ball;

    int second;
    float countTime = 30;
    int returnCount = 0;

    void Start()
    {
        gameDirector = GameObject.Find("GameDirector").GetComponent<GameDirector>();
        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(countTime <= 0)
        {
            if (uiManager.leftGoalScore == uiManager.rightGoalScore)
            {
                uiManager.DisplayDrow();
                Invoke("HideDrow", 1.5f);
                Invoke("ResetTimer", 3.2f);
            }
            else if(gameDirector.isEnd == false && uiManager.isDrow == false)
            {
                countTime = 0;
                roomModel.EndGameAsync();
                return;
            }
        }

        if (gameDirector.isStart && uiManager.isStop == false)
        {
            countTime -= Time.deltaTime;
            second = (int)countTime;
            //(int)countTimeでint型に変換して表示させる。
            timerText.text = second.ToString();
        }
    }

    public void HideDrow()
    {
        uiManager.HideDrow();
    }

    /// <summary>
    /// タイマーリセット
    /// </summary>
    public void ResetTimer()
    {
        GameObject.Find("Ball(Clone)").GetComponent<BallDirector>().ResetBallPos();
        uiManager.isDrow = false;
        countTime = 30;
        timerText.text = second.ToString();
    }
}
