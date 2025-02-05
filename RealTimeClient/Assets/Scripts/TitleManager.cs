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
        // Sequence�̃C���X�^���X���쐬
        var sequence = DOTween.Sequence();

        sequence.Append(playText.DOColor(Color.cyan, 5f)); // 5�b�����Đ�
                                                              // Sequence�����s
        sequence.Play();

        playText.DOFade(0.0f, 3f)   // �A���t�@�l��0�ɂ��Ă���
                       .SetLoops(-1, LoopType.Yoyo);    // �s�����𖳌��ɌJ��Ԃ�
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            playText.DOFade(0.0f, 0.1f)   // �A���t�@�l��0�ɂ��Ă���
                       .SetLoops(-1, LoopType.Yoyo);    // �s�����𖳌��ɌJ��Ԃ�

            Invoke("ChangeScene", 1.7f);
        }
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene("Game");
    }
}
