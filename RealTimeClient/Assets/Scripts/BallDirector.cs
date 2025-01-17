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

    public int shootPow = 4;

    Rigidbody myRigidbody;
    // Transformコンポーネントを保持しておくための変数を追加
    Transform myTransform;

    Character player;

    public Vector3 velocity;

    UIManager manager;

    RoomModel roomModel;

    GameDirector gameDirector;

    ShootAreaCheck shootArea;

    bool isGoal;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        //myRigidbody.velocity = new Vector3(speed,0f, speed);
        // Transformコンポーネントを取得して保持しておく
        myTransform = transform;

        startSpeed = speed;

        player = GameObject.Find("Character(Clone)").GetComponent<Character>();

        manager = GameObject.Find("UIManager").GetComponent<UIManager>();

        roomModel = GameObject.Find("RoomModel").GetComponent<RoomModel>();

        gameDirector = GameObject.Find("GameDirector").GetComponent<GameDirector>();

        roomModel.OnShootPow += this.OnShoot;

        isGoal = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (manager.isShootArea)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                shoot();
            }
        }
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
            myRigidbody.velocity = direction * (speed + player.moveSpeed / 2);
        }
    }

    private async void OnTriggerEnter(Collider other)
    {
        if (isGoal == false)
        {
            if (other.gameObject.CompareTag("Goal"))
            {
                if (other.gameObject.name == "LeftGoalWall" && gameDirector.joinOrder == 2)
                {
                    await roomModel.GoalAsync();

                    Invoke(nameof(StopBall), 0.3f);
                }
                else if (other.gameObject.name == "RightGoalWall" && gameDirector.joinOrder == 1)
                {
                    await roomModel.GoalAsync();

                    Invoke(nameof(StopBall), 0.3f);
                }

                isGoal = true;
            }
        }
    }

    public void StopBall()
    {
        myRigidbody.velocity = new Vector3(0,0,0);


    }

    public void shoot()
    {
        // プレイヤーの位置を取得
        Vector3 playerPos = player.transform.position;
        // ボールの位置を取得
        Vector3 ballPos = myTransform.position;
        // プレイヤーから見たボールの方向を計算
        Vector3 direction = (ballPos - playerPos).normalized;
        direction.y += 0.6f;
        // 現在の速さを取得
        float speed = myRigidbody.velocity.magnitude;

        roomModel.ShootAsync(direction * shootPow);

        Debug.Log(myRigidbody.velocity);
    }

    public void OnShoot(Vector3 shootPow)
    {
        if (gameDirector.joinOrder == 1)
        {
            myRigidbody.AddForce(shootPow, ForceMode.Impulse);
        }
    }

    public void ResetBallPos()
    {
        if (isGoal)
        {
            this.gameObject.transform.position = gameDirector.InitBallPos;
            isGoal = false;
        }
    }
}
