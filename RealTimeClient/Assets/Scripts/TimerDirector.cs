using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class TimerDirector : MonoBehaviour
{
    [SerializeField] TextMesh timerText;
    [SerializeField] RoomModel roomModel;

    GameDirector gameDirector;

    //整数とする変数を追加
    int second;
    float countTime = 90;

    void Start()
    {
        gameDirector = GameObject.Find("GameDirector").GetComponent<GameDirector>();
    }

    // Update is called once per frame
    void Update()
    {
        if (countTime <= 0)
        {
            countTime = 0;
            roomModel.EndGameAsync();
        }

        if (gameDirector.isStart)
        {
            countTime -= Time.deltaTime;
            second = (int)countTime;
            //(int)countTimeでint型に変換して表示させる。
            timerText.text = second.ToString();
        }
    }
}
