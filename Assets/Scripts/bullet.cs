using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    /* 게임에서 선택한 버튼 */
    int selectNum = 0;

    /* 총알이 부딪힌 횟수 */
    int bulletHealth = 10;

    /* 총알이 날라가는 각도 */
    float tmpAngle;

    /* 시간을 측정할 것인가 */
    bool timeCheck = false;
    float waiting = 0f;             // 측정 중인 시간

    /* 어떤 총알을 만들 것인가 */
    bool thTime = false;
    Vector3 firstPosition;          // 연속 총알 만들 때 생성위치
    Quaternion firstRotation;       // 연속 총알 만들 때 생성각도
    int madeBullet = 0;             // 연속 총알 만들 때  연속총알을 몇 개 만들었는가

    bool flower = false;
    bool bomb = false;

    /* 효과음 */
    AudioSource aud;
    public AudioClip Sbasic;
    public AudioClip Sflower;
    public AudioClip Sbomb;
    public AudioClip SwrongBomb;

    /* 효과음이 재생 중인지 체크 */
    bool playingSound = false;

    /* 폭탄일때 클릭했는가 */
    bool click = false;

    private void Start()
    {
        GetComponent<AudioSource>().Play();

        aud = GetComponent<AudioSource>();
        transform.parent = GameObject.Find("massBullet").transform;     // 이 문장을 빼면 총알이 커지는데 왜 그런지는 모르겠습니다.

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
            bulletHealth = 5;
            flower = true;
            GameObject.Find("totalManager").GetComponent<totalManage>().selectBasic();
        }
        else if (selectNum == 3)
        {
            bulletHealth = 2;
            bomb = true;
            GameObject.Find("totalManager").GetComponent<totalManage>().selectBasic();
        }
    }
    private void Update()
    {
        /* 정상적으로 처리되지 않은 미사일 */
        if (transform.position.x > 4 || transform.position.x < -9 || transform.position.y > 10 || transform.position.y < -9)
        {
            Debug.Log("업데이트에서 오류 처리");
            if (GameObject.Find("massBullet").transform.childCount - 1 <= 3)
            {
                exExist();
            }
            Destroy(gameObject);
        }
        /* 대기 시간(연속총알)*/
        if (timeCheck)
        {
            waiting += Time.deltaTime;
        }

        GameObject tmp;
        Rigidbody2D rigid;

        /* 연속 총알 구현 */
        if (thTime)
        {
            if (madeBullet == 0 && (waiting > 0.2f))
            {
                tmp = Instantiate(gameObject, firstPosition, firstRotation);
                rigid = tmp.GetComponent<Rigidbody2D>();
                rigid.AddForce(tmp.transform.up * 10, ForceMode2D.Impulse);
                madeBullet++;
            }
            else if (madeBullet == 1 && (waiting > 0.4f))
            {
                tmp = Instantiate(gameObject, firstPosition, firstRotation);
                rigid = tmp.GetComponent<Rigidbody2D>();
                rigid.AddForce(tmp.transform.up * 10, ForceMode2D.Impulse);
                timeCheck = false;
                waiting = 0f;
                thTime = false;
                madeBullet++;
            }
        }

        /* 폭죽 구현 */
        if (flower && Input.GetMouseButtonDown(0))
        {
            aud.PlayOneShot(Sflower);
            /* 각 bullet의 생성 될 때 각도를 얻어옴 */
            float firstBullet;
            float seBullet;
            float thBullet;

            bulletHealth = 10; // 3갈래로 나뉘었기 때문에 다시 10번 튕길수 있도록 셋팅
            GameObject.Find("totalManager").GetComponent<totalManage>().selectBasic();

            Vector3 nowPosition = gameObject.transform.position;
            Vector3 testRotation = gameObject.transform.rotation.eulerAngles;

            Rigidbody2D bullet = gameObject.GetComponent<Rigidbody2D>();

            /* 두 번째 bullet의 각도를 얻어옴 */
            seBullet = tmpAngle + 60;
            if (testRotation.z > 180)
            {
                testRotation.z -= 360;
            }
            tmp = Instantiate(gameObject, nowPosition, Quaternion.Euler(0, 0, seBullet));
            tmp.GetComponent<Rigidbody2D>().AddForce(tmp.transform.TransformDirection(Vector3.up) * 10, ForceMode2D.Impulse);

            /* 세 번째 bullet의 각도를 얻어옴 */
            thBullet = tmpAngle - 60;
            if (testRotation.z < -180)
            {
                testRotation.z += 360;
            }
            tmp = Instantiate(gameObject, nowPosition, Quaternion.Euler(0, 0, thBullet));
            tmp.GetComponent<Rigidbody2D>().AddForce(tmp.transform.TransformDirection(Vector3.up) * 10, ForceMode2D.Impulse);

            /* 첫번째 bullet의 각도를 얻어옴 */
            firstBullet = -180 + tmpAngle;
            tmpAngle = -180 + tmpAngle;

            transform.rotation = Quaternion.Euler(0, 0, firstBullet);
            bullet.velocity = Vector3.zero; // addForce 초기화
            bullet.AddForce(transform.TransformDirection(Vector3.up) * 10, ForceMode2D.Impulse);

            flower = false;
        }

        /* 폭탄 구현 */
        if (bomb && Input.GetMouseButtonDown(0))
        {
            aud.PlayOneShot(Sbomb);
            playingSound = true;
            click = true;
            rigid = gameObject.GetComponent<Rigidbody2D>();
            rigid.velocity = Vector3.zero;
            rigid.GetComponent<CircleCollider2D>().radius = 5;
            bulletHealth = 1;
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

    /* 총알이 충돌하였을 때 */
    void OnTriggerEnter2D(Collider2D col)
    {
        Rigidbody2D bullet = gameObject.GetComponent<Rigidbody2D>();

        if (col.CompareTag("destroy"))
        {
            bulletHealth = 0;
            Debug.Log("triggerEndter에서 오류처리");
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
                bulletHealth = 1;
            }
            else
            {
                AudioSource.PlayClipAtPoint(Sbasic, new Vector3(transform.position.x, 1f, -5f), 10f);
                bullet.velocity = Vector3.zero;

                bulletHealth--;
                if (tmpAngle < 0 && tmpAngle > -180)
                {
                    tmpAngle = -tmpAngle - 180;
                }
                else
                {
                    tmpAngle = 180 - tmpAngle;
                }
                transform.rotation = Quaternion.Euler(0, 0, tmpAngle);

                bullet.AddForce(transform.TransformDirection(Vector3.up) * 10, ForceMode2D.Impulse);
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
                bulletHealth = 1;
            }
            else
            {
                AudioSource.PlayClipAtPoint(Sbasic, new Vector3(transform.position.x, 1f, -5f), 10f);
                bullet.velocity = Vector3.zero;

                bulletHealth--;

                tmpAngle = -tmpAngle;

                transform.rotation = Quaternion.Euler(0, 0, tmpAngle);

                bullet.AddForce(transform.TransformDirection(Vector3.up) * 10, ForceMode2D.Impulse);
            }
        }

        // 10번째 충돌이라면 오브젝트 삭제
        if (bulletHealth == 0)
        {
            if (GameObject.Find("massBullet").transform.childCount - 1 <= 3)
            {
                exExist();
            }
            Destroy(gameObject);
        }
    }

    /* 블럭을 한 줄 만들도록 셋팅한다. */
    public void exExist()
    {
        GameObject.Find("blockManager").GetComponent<BlockManager>().setMakeBlock(false);
    }
}
