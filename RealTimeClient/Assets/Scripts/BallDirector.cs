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
    }

    // Update is called once per frame
    void Update()
    {
       
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
        if (other.gameObject.CompareTag("Goal"))
        {
            if(other.gameObject.name == "LeftGoalWall" && gameDirector.joinOrder == 2)
            {
                await roomModel.GoalAsync();

                Invoke(nameof(StopBall), 0.3f);
            }
            else if(other.gameObject.name == "RightGoalWall" && gameDirector.joinOrder == 1)
            {
                await roomModel.GoalAsync();

                Invoke(nameof(StopBall), 0.3f);
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
        direction.y += 0.7f;
        // ���݂̑������擾
        float speed = myRigidbody.velocity.magnitude;

        myRigidbody.AddForce(direction * shootPow, ForceMode.Impulse);
        Debug.Log(myRigidbody.velocity);
    }
}
