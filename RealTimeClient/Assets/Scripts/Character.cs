using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEditor.MemoryProfiler;
using System;

public class Character : MonoBehaviour
{
    public enum CharactorState
    {
        idle = 0,
        run = 1
    }

    public CharactorState state { get; set; }

    Rigidbody rb;

    float x;
    float z;
    public float moveSpeed;
    Vector3 move;

    float animSpeed;

    Animator animator;

    public bool isSelf;

    FloatingJoystick floatingJoystick;

    RoomModel roomModel;

    public Guid connectionId;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        floatingJoystick = GameObject.Find("Floating Joystick").GetComponent<FloatingJoystick>();

        roomModel = GameObject.Find("RoomModel").GetComponent<RoomModel>();

        animator = this.gameObject.GetComponent<Animator>();

        animSpeed = 0;

        state = CharactorState.idle;

        //animator.SetFloat("speed", moveSpeed);
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
        if (roomModel.ConnectionId != connectionId)
        {
            return;
        }
        Vector3 cameraForward = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z);
        Vector3 cameraRight = new Vector3(Camera.main.transform.right.x, 0, Camera.main.transform.right.z);

        //this.transform.DOMove(new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z),2.0f);

        move = (cameraForward * floatingJoystick.Vertical + cameraRight * floatingJoystick.Horizontal).normalized;

        rb.velocity = move * moveSpeed;

        animSpeed = rb.velocity.magnitude;

        if(animSpeed > 0)
        {
            state = CharactorState.run;
        }
        else
        {
            state = CharactorState.idle;
        }

        animator.SetInteger("state",(int)state);

        if (move != Vector3.zero)
        {
            //transform.forward = move;
            //-----------------------------------------åªç›ín,ñ⁄ìIÇÃíl,éûä‘
            transform.forward = Vector3.Slerp(transform.forward, move, Time.deltaTime * 6);
        }
    }
}
