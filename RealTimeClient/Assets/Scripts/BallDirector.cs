using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;

public class BallDirector : MonoBehaviour
{
    public float speed = 4f;
    // �����̍ő�l���w�肷��ϐ���ǉ�
    public float minSpeed = 5f;
    // �����̍ŏ��l���w�肷��ϐ���ǉ�
    public float maxSpeed = 10f;

    private float startSpeed;

    public int shootPow = 4;

    Rigidbody myRigidbody;
    // Transform�R���|�[�l���g��ێ����Ă������߂̕ϐ���ǉ�
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
        // Transform�R���|�[�l���g���擾���ĕێ����Ă���
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
        // �v���C���[�ɓ��������Ƃ��ɁA���˕Ԃ������ς���
        if (collision.gameObject.CompareTag("Player"))
        {
            // �v���C���[�̈ʒu���擾
            Vector3 playerPos = collision.transform.position;
            // �{�[���̈ʒu���擾
            Vector3 ballPos = myTransform.position;
            // �v���C���[���猩���{�[���̕������v�Z
            Vector3 direction = (ballPos - playerPos).normalized;
            // ���݂̑������擾
            float speed = myRigidbody.velocity.magnitude;
            // ���x��ύX
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
        // �v���C���[�̈ʒu���擾
        Vector3 playerPos = player.transform.position;
        // �{�[���̈ʒu���擾
        Vector3 ballPos = myTransform.position;
        // �v���C���[���猩���{�[���̕������v�Z
        Vector3 direction = (ballPos - playerPos).normalized;
        direction.y += 0.6f;
        // ���݂̑������擾
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
