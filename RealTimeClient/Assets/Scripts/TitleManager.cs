using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [SerializeField] Text playText;
    [SerializeField] Text userId;

    // Start is called before the first frame update
    void Start()
    {
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

            Invoke("ChangeScene", 1.7f);
        }
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene("Game");
    }
}
