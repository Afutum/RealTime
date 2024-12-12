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

    Rigidbody myRigidbody;
    // Transform�R���|�[�l���g��ێ����Ă������߂̕ϐ���ǉ�
    Transform myTransform;

    Character player;

    public Vector3 velocity;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        //myRigidbody.velocity = new Vector3(speed,0f, speed);
        // Transform�R���|�[�l���g���擾���ĕێ����Ă���
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
            myRigidbody.velocity = direction * (speed + player.moveSpeed);
        }
    }
}
