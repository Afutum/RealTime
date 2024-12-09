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
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
