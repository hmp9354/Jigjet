using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Block : MonoBehaviour
{
    public GameObject TotalManager;
    public GameObject BlockManager;
    public GameObject Coin;

    int nowCoin;                // �����ϰ� �ִ� ����(����)
    int coin = 2;               // ��� �ı��� ȹ���ϴ� ����

    int blockType = 1;
    bool trick = false;         // ����� ���� ����ΰ�?    

    int healthPoint = 1;        // ����� ü��

    public static bool stop = true;     // ��� ������ ���� ���ΰ�, ����� ���ΰ�

    public void setBlockType(int blockType)
    {
        this.blockType = blockType;
    }

    private void Start()
    {
        if (blockType == 1)
        {
            healthPoint = 1;
        }
        else if (blockType == 2)
        {
            healthPoint = 2;
        }
        else if (blockType == 4)
        {
            healthPoint = 2;
            trick = true;
        }
    }
    private void Update()
    {
        if (transform.position.y <= -2.75)
        {
            Debug.Log("���� ����");
            TotalManager.GetComponent<totalManage>().gameOver();
            BlockManager.GetComponent<BlockManager>().setGameOver();
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("bullet") && (blockType != 3))
        {
            healthPoint--;
            if (healthPoint == 0)
            {
                if (blockType == 2)
                {
                    coin = 3;
                }
                else if (blockType == 4)
                {
                    coin = 15;
                }
                nowCoin = TotalManager.GetComponent<totalManage>().sgCoin;
                nowCoin += coin;
                Coin.GetComponent<Text>().text = nowCoin.ToString();
                TotalManager.GetComponent<totalManage>().sgCoin = nowCoin;
                Destroy(gameObject);
            }

            if (trick)
            {
                Color tmpColor = gameObject.GetComponent<SpriteRenderer>().color;
                tmpColor.a = 255f;
                gameObject.GetComponent<SpriteRenderer>().color = tmpColor;
                trick = false;
            }
        }
        else if (col.CompareTag("bomb"))
        {
            if (blockType == 2)
            {
                coin = 3;
            }
            else if (blockType == 3)
            {
                coin = 10;
            }
            else if (blockType == 4)
            {
                coin = 15;
            }
            nowCoin = TotalManager.GetComponent<totalManage>().sgCoin;
            nowCoin += coin;
            Coin.GetComponent<Text>().text = nowCoin.ToString();
            TotalManager.GetComponent<totalManage>().sgCoin = nowCoin;
            Destroy(gameObject);
        }
    }
}