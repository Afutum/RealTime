using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Character : MonoBehaviour
{
    Rigidbody rb;

    float x;
    float z;
    public float moveSpeed;
    Vector3 move;

    public bool isSelf;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    // Update is called once per frame
    void Update()
    {
        if(isSelf == false)
        {
            return;
        }
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        Vector3 cameraForward = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z);
        Vector3 cameraRight = new Vector3(Camera.main.transform.right.x, 0, Camera.main.transform.right.z);

        move = (cameraForward * z + cameraRight * x).normalized;

        rb.velocity = move * moveSpeed;

        if (move != Vector3.zero)
        {
            //transform.forward = move;
            //-----------------------------------------åªç›ín,ñ⁄ìIÇÃíl,éûä‘
            transform.forward = Vector3.Slerp(transform.forward, move, Time.deltaTime * 6);
        }
    }
}
