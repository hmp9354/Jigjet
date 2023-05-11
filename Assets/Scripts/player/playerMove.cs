using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerMove : MonoBehaviour
{
    Vector2 firstPosition;                   // �巡�׸� �ϱ� ���� ������ ���� ��ġ    

    /* �Ѿ��̹���. ������� �⺻, ����, ��ź */
    public GameObject bulletObjA;
    public GameObject bulletObjC;
    public GameObject bulletObjD;

    GameObject bullet;
    Rigidbody2D rigid;

    static public float playerAngle = 0;    // ����� ����
    float distance = 0;                     // ����Ⱑ �̵��� �Ÿ�

    /* �ð��� ������ ���ΰ� */
    bool timeCheck = false;
    float dragTime = 0f;              // �������� �ð�

    /* ����⸦ ȸ���� ���ΰ� */
    bool rotate = true;

    private void Update()
    {
        if (timeCheck)
        {
            dragTime += Time.deltaTime;
        }
    }

    void OnMouseUp()
    {
        if (dragTime >= 1 && rotate && GameObject.Find("massBullet").transform.childCount <= 3)
        {
            Fire();
        }
        dragTime = 0f;
        timeCheck = false;
        rotate = true;
    }

    void OnMouseDown()
    {
        timeCheck = true;
        firstPosition = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
        firstPosition.y = -2.2f;
    }

    /* ����� ��ġ �� ���� ���� */
    void OnMouseDrag()
    {
        Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // �ð��� 1�� �̸��϶� ����(0.1��ŭ) ���������� Ȯ��. �������ٸ� rotate�� �״��(true)
        if (dragTime < 1)
        {
            objPosition.y = -2.2f;
            distance = objPosition.x - firstPosition.x;

            if (transform.position.x + distance < -2)
            {
                distance = -2f - transform.position.x;
            }
            else if (transform.position.x + distance > 2)
            {
                distance = 2f - transform.position.x;
            }
            transform.position = new Vector2(transform.position.x + distance, -2.2f);
            firstPosition = objPosition;
            
            if (Mathf.Abs(distance) > 0.1)
            {
                rotate = false;
            }
        }
        else
        {
            if (rotate)
            {
                playerAngle = Mathf.Atan2(objPosition.y - transform.position.y, objPosition.x - transform.position.x) * Mathf.Rad2Deg;

                if (playerAngle < 0 && playerAngle > -90)
                {
                    playerAngle = 0;
                }
                else if (playerAngle > -180 && playerAngle < -90)
                {
                    playerAngle = 180;
                }

                this.transform.rotation = Quaternion.Euler(0f, 0f, playerAngle - 90f);
            }
            else
            {
                objPosition.y = -2.2f;

                distance = objPosition.x - firstPosition.x;

                if (transform.position.x + distance < -2)
                {
                    distance = -2f - transform.position.x;
                }
                else if (transform.position.x + distance > 2)
                {
                    distance = 2f - transform.position.x;
                }

                transform.position = new Vector2(transform.position.x + distance, -2.2f);
                firstPosition = objPosition;
            }
        }
    }

    void Fire()
    {
        Vector2 vbullet = transform.position;
        int select = GameObject.Find("totalManager").GetComponent<totalManage>().sendSelectNum();
        int nowCoin = GameObject.Find("totalManager").GetComponent<totalManage>().sgCoin;

        if (select == 0 || (select == 1))
        {
            bullet = Instantiate(bulletObjA, vbullet, transform.rotation);
            if (select == 1)
            {
                nowCoin -= 5;
            }
        }
        else if (select == 2)
        {
            nowCoin -= 15;
            bullet = Instantiate(bulletObjC, vbullet, transform.rotation);
        }
        else
        {
            nowCoin -= 20;
            bullet = Instantiate(bulletObjD, vbullet, transform.rotation);
        }

        GameObject.Find("coin").GetComponent<Text>().text = nowCoin.ToString();
        GameObject.Find("totalManager").GetComponent<totalManage>().sgCoin = nowCoin;
        rigid = bullet.GetComponent<Rigidbody2D>();
        rigid.AddForce(transform.up * 10, ForceMode2D.Impulse);
    }
}
