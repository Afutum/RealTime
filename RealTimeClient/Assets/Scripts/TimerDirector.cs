using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class TimerDirector : MonoBehaviour
{
    [SerializeField] TextMesh timerText;

    GameDirector gameDirector;

    //�����Ƃ���ϐ���ǉ�
    int second;
    float countTime = 90;

    void Start()
    {
        gameDirector = GameObject.Find("GameDirector").GetComponent<GameDirector>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameDirector.isStart)
        {
            countTime -= Time.deltaTime;
            second = (int)countTime;
            //(int)countTime��int�^�ɕϊ����ĕ\��������B
            timerText.text = second.ToString();
        }
    }
}
