using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAreaCheck : MonoBehaviour
{
    UIManager manager;
            
    private void Start()
    {
        manager = GameObject.Find("UIManager").GetComponent<UIManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            manager.isShootArea = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            manager.isShootArea = false;
        }
    }
}
