using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class TimerDirector : MonoBehaviour
{
    [SerializeField] TextMesh timerText;
    [SerializeField] RoomModel roomModel;

    GameDirector gameDirector;

    //®”‚Æ‚·‚é•Ï”‚ğ’Ç‰Á
    int second;
    float countTime = 5;

    void Start()
    {
        gameDirector = GameObject.Find("GameDirector").GetComponent<GameDirector>();
    }

    // Update is called once per frame
    void Update()
    {
        if (countTime <= 0 && gameDirector.isEnd == false)
        {
            countTime = 0;
            roomModel.EndGameAsync();
            return;
        }

        if (gameDirector.isStart)
        {
            countTime -= Time.deltaTime;
            second = (int)countTime;
            //(int)countTime‚ÅintŒ^‚É•ÏŠ·‚µ‚Ä•\¦‚³‚¹‚éB
            timerText.text = second.ToString();
        }
    }

    public void ResetTimer()
    {
        countTime = 30;
        timerText.text = second.ToString();
    }
}
