using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    /* ���ӿ��� ������ ��ư */
    int selectNum = 0;

    /* �Ѿ��� �ε��� Ƚ�� */
    int bltHealth = 10;

    /* �Ѿ��� �ӵ� */
    int bltSpeed = 10;

    /* �Ѿ��� ���󰡴� ���� */
    float tmpAngle;

    /* �ð��� ������ ���ΰ� */
    bool timeCheck = false;
    float waiting = 0f;             // ���� ���� �ð�

    /* � �Ѿ��� ���� ���ΰ� */
    bool thTime = false;
    Vector3 firstPosition;          // ���� �Ѿ� ���� �� ������ġ
    Quaternion firstRotation;       // ���� �Ѿ� ���� �� ��������
    int madeBullet = 0;             // ���� �Ѿ� ���� ��  �����Ѿ��� �� �� ������°�

    bool flower = false;
    bool bomb = false;

    /* ȿ���� */
    AudioSource aud;
    public AudioClip Sbasic;
    public AudioClip Sflower;
    public AudioClip Sbomb;
    public AudioClip SwrongBomb;

    /* ȿ������ ��� ������ üũ */
    bool playingSound = false;

    /* ��ź�϶� Ŭ���ߴ°� */
    bool click = false;

    private void Start()
    {
        GetComponent<AudioSource>().Play();

        aud = GetComponent<AudioSource>();
        transform.parent = GameObject.Find("massBullet").transform;     // �� ������ ���� �Ѿ��� Ŀ���µ� �� �׷����� �𸣰ڽ��ϴ�.

        firstPosition = gameObject.transform.position;
        firstRotation = gameObject.transform.rotation;

        tmpAngle = transform.rotation.eulerAngles.z;

        selectNum = GameObject.Find("totalManager").GetComponent<totalManage>().sendSelectNum();

        if (selectNum == 1)
        {
            timeCheck = true;
            waiting = 0f;
            thTime = true;
            GameObject.Find("totalManager").GetComponent<totalManage>().selectBasic();
        }
        else if (selectNum == 2)
        {
            bltHealth = 5;
            flower = true;
            GameObject.Find("totalManager").GetComponent<totalManage>().selectBasic();
        }
        else if (selectNum == 3)
        {
            bltHealth = 2;
            bomb = true;
            GameObject.Find("totalManager").GetComponent<totalManage>().selectBasic();
        }
    }
    private void Update()
    {
        /* ���������� ó������ ���� �̻��� */
        if (transform.position.x > 4 || transform.position.x < -9 || transform.position.y > 10 || transform.position.y < -9)
        {
            Debug.Log("������Ʈ���� ���� ó��");
            if (GameObject.Find("massBullet").transform.childCount - 1 <= 3)
            {
                exExist();
            }
            Destroy(gameObject);
        }
        /* ��� �ð�(�����Ѿ�)*/
        if (timeCheck)
        {
            waiting += Time.deltaTime;
        }

        GameObject tmp;
        Rigidbody2D rigid;

        /* ���� �Ѿ� ���� */
        if (thTime)
        {
            if (madeBullet == 0 && (waiting > 0.2f))
            {
                tmp = Instantiate(gameObject, firstPosition, firstRotation);
                rigid = tmp.GetComponent<Rigidbody2D>();
                rigid.AddForce(tmp.transform.up * bltSpeed, ForceMode2D.Impulse);
                madeBullet++;
            }
            else if (madeBullet == 1 && (waiting > 0.4f))
            {
                tmp = Instantiate(gameObject, firstPosition, firstRotation);
                rigid = tmp.GetComponent<Rigidbody2D>();
                rigid.AddForce(tmp.transform.up * bltSpeed, ForceMode2D.Impulse);
                timeCheck = false;
                waiting = 0f;
                thTime = false;
                madeBullet++;
            }
        }

        /* ���� ���� */
        if (flower && Input.GetMouseButtonDown(0))
        {
            aud.PlayOneShot(Sflower);
            /* �� bullet�� ���� �� �� ������ ���� */
            float firstBullet;
            float seBullet;
            float thBullet;

            bltHealth = 10; // 3������ �������� ������ �ٽ� 10�� ƨ��� �ֵ��� ����
            GameObject.Find("totalManager").GetComponent<totalManage>().selectBasic();

            Vector3 nowPosition = gameObject.transform.position;
            Vector3 testRotation = gameObject.transform.rotation.eulerAngles;

            Rigidbody2D bullet = gameObject.GetComponent<Rigidbody2D>();

            /* �� ��° bullet�� ������ ���� */
            seBullet = tmpAngle + 60;
            if (testRotation.z > 180)
            {
                testRotation.z -= 360;
            }
            tmp = Instantiate(gameObject, nowPosition, Quaternion.Euler(0, 0, seBullet));
            tmp.GetComponent<Rigidbody2D>().AddForce(tmp.transform.TransformDirection(Vector3.up) * bltSpeed, ForceMode2D.Impulse);

            /* �� ��° bullet�� ������ ���� */
            thBullet = tmpAngle - 60;
            if (testRotation.z < -180)
            {
                testRotation.z += 360;
            }
            tmp = Instantiate(gameObject, nowPosition, Quaternion.Euler(0, 0, thBullet));
            tmp.GetComponent<Rigidbody2D>().AddForce(tmp.transform.TransformDirection(Vector3.up) * bltSpeed, ForceMode2D.Impulse);

            /* ù��° bullet�� ������ ���� */
            firstBullet = -180 + tmpAngle;
            tmpAngle = -180 + tmpAngle;

            transform.rotation = Quaternion.Euler(0, 0, firstBullet);
            bullet.velocity = Vector3.zero; // addForce �ʱ�ȭ
            bullet.AddForce(transform.TransformDirection(Vector3.up) * 10, ForceMode2D.Impulse);

            flower = false;
        }

        /* ��ź ���� */
        if (bomb && Input.GetMouseButtonDown(0))
        {
            aud.PlayOneShot(Sbomb);
            playingSound = true;
            click = true;
            rigid = gameObject.GetComponent<Rigidbody2D>();
            rigid.velocity = Vector3.zero;
            rigid.GetComponent<CircleCollider2D>().radius = 5;
            bltHealth = 1;
        }

        if (playingSound)
        {
            if (!aud.isPlaying)
            {
                exExist();
                Destroy(gameObject);
                playingSound = false;
            }
        }
    }

    /* �Ѿ��� �浹�Ͽ��� �� */
    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log(bltHealth);
        Debug.Log(col);
        Rigidbody2D bullet = gameObject.GetComponent<Rigidbody2D>();

        if (col.CompareTag("destroy"))
        {
            bltHealth = 0;
            Debug.Log("triggerEndter���� ����ó��");
        }

        if (col.CompareTag("udborder"))
        {
            if (bomb)
            {
                bullet.velocity = Vector3.zero;
                if (!click)
                {
                    aud.PlayOneShot(SwrongBomb);
                    playingSound = true;
                }
                bltHealth = 1;
            }
            else
            {
                AudioSource.PlayClipAtPoint(Sbasic, new Vector3(transform.position.x, 1f, -5f), 10f);
                bullet.velocity = Vector3.zero;

                bltHealth--;
                if (tmpAngle < 0 && tmpAngle > -180)
                {
                    tmpAngle = -tmpAngle - 180;
                }
                else
                {
                    tmpAngle = 180 - tmpAngle;
                }
                transform.rotation = Quaternion.Euler(0, 0, tmpAngle);

                bullet.AddForce(transform.TransformDirection(Vector3.up) * bltSpeed, ForceMode2D.Impulse);
            }
        }
        else if (col.CompareTag("rlborder"))
        {
            if (bomb)
            {
                bullet.velocity = Vector3.zero;
                if (!click)
                {
                    aud.PlayOneShot(SwrongBomb);
                    playingSound = true;
                }
                bltHealth = 1;
            }
            else
            {
                AudioSource.PlayClipAtPoint(Sbasic, new Vector3(transform.position.x, 1f, -5f), 10f);
                bullet.velocity = Vector3.zero;

                bltHealth--;

                tmpAngle = -tmpAngle;

                transform.rotation = Quaternion.Euler(0, 0, tmpAngle);

                bullet.AddForce(transform.TransformDirection(Vector3.up) * bltSpeed, ForceMode2D.Impulse);
            }
        }

        // 10��° �浹�̶�� ������Ʈ ����
        if (bltHealth <= 0)
        {
            if (GameObject.Find("massBullet").transform.childCount - 1 <= 3)
            {
                exExist();
            }
            Destroy(gameObject);
        }
    }

    /* ���� �� �� ���鵵�� �����Ѵ�. */
    public void exExist()
    {
        GameObject.Find("blockManager").GetComponent<BlockManager>().setMakeBlock(false);
    }
}
