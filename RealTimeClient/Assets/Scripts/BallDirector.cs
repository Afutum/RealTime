using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;

public class BallDirector : MonoBehaviour
{
    public float speed = 4f;
    // 速さの最大値を指定する変数を追加
    public float minSpeed = 5f;
    // 速さの最小値を指定する変数を追加
    public float maxSpeed = 10f;

    private float startSpeed;

    Rigidbody myRigidbody;
    // Transformコンポーネントを保持しておくための変数を追加
    Transform myTransform;

    Character player;

    public Vector3 velocity;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        //myRigidbody.velocity = new Vector3(speed,0f, speed);
        // Transformコンポーネントを取得して保持しておく
        myTransform = transform;

        startSpeed = speed;

        player = GameObject.Find("Character(Clone)").GetComponent<Character>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public async void OnCollisionEnter(Collision collision)
    {
        // プレイヤーに当たったときに、跳ね返る方向を変える
        if (collision.gameObject.CompareTag("Player"))
        {
            // プレイヤーの位置を取得
            Vector3 playerPos = collision.transform.position;
            // ボールの位置を取得
            Vector3 ballPos = myTransform.position;
            // プレイヤーから見たボールの方向を計算
            Vector3 direction = (ballPos - playerPos).normalized;
            // 現在の速さを取得
            float speed = myRigidbody.velocity.magnitude;
            // 速度を変更
            myRigidbody.velocity = direction * (speed + player.moveSpeed);
        }
    }
}
