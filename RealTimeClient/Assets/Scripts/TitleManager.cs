using Assets.Model;
using Assets.Scripts;
using DG.Tweening;
using Newtonsoft.Json;
using Shared.Interfaces.StreamingHubs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class TitleManager : MonoBehaviour
{
    [SerializeField] Text playText;
    [SerializeField] Text userId;
    [SerializeField] Text load;
    [SerializeField] RoomModel roomModel;

    public class RoomName
    {
        public static string joinRoomName;
        public static int userId;
    }

    // Start is called before the first frame update
    async void Start()
    {
        roomModel.OnMatchingUser += OnMatchingUser;
        roomModel.OnLeave += this.OnLeave;

        // �ڑ�
        await roomModel.ConnectAsync();

        // Sequence�̃C���X�^���X���쐬
        var sequence = DOTween.Sequence();

        sequence.Append(playText.DOColor(Color.cyan, 5f)); // 5�b�����Đ�
                                                              // Sequence�����s
        sequence.Play();

        load.enabled = false;

        playText.DOFade(0.0f, 3f)   // �A���t�@�l��0�ɂ��Ă���
                       .SetLoops(-1, LoopType.Yoyo);    // �s�����𖳌��ɌJ��Ԃ�
    }

    // Update is called once per frame
    async void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            bool isSuccess = UserModel.Instance.LoadUserData();

            if (!isSuccess)
            {
                if (await UserModel.Instance.RegistUserAsync())
                {
                    load.enabled = true;
                    playText.enabled = false;

                    load.DOFade(0.0f, 0.5f)   // �A���t�@�l��0�ɂ��Ă���
                       .SetLoops(-1, LoopType.Yoyo);    // �s�����𖳌��ɌJ��Ԃ�

                    JoinLobbyAsync();
                }
                else
                {
                    return;
                }
            }
            else
            {
                UserModel.Instance.LoadUserData();

                load.enabled = true;
                    playText.enabled = false;

                    load.DOFade(0.0f, 0.5f)   // �A���t�@�l��0�ɂ��Ă���
                       .SetLoops(-1, LoopType.Yoyo);    // �s�����𖳌��ɌJ��Ԃ�

                JoinLobbyAsync();
            }
        }
    }

    /// <summary>
    /// �V�[���J��
    /// </summary>
    public void ChangeScene()
    {
        SceneManager.LoadScene("Game");
    }

    /// <summary>
    /// ���r�[����
    /// </summary>
    public async void JoinLobbyAsync()
    {
       await roomModel.JoinLobbyAsync(UserModel.Instance.userID);
    }

    /// <summary>
    /// �T�[�o�[����}�b�`���O�̒ʒm�󂯎���̃��[���ޏo����
    /// </summary>
    /// <param name="roomName"></param>
    private async void OnMatchingUser(string roomName)
    {
        RoomName.joinRoomName = roomName;

        await roomModel.LeaveAsync();
    }

    /// <summary>
    /// �T�[�o�[���烍�r�[�ޏo�ʒm��������̏���
    /// </summary>
    /// <param name="ConnectionId"></param>
    private void OnLeave(Guid ConnectionId)
    {
        ChangeScene();
    }
}
