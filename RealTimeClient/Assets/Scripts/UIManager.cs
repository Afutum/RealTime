using Assets.Model;
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
}
