using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Block : MonoBehaviour
{
    public GameObject TotalManager;
    public GameObject BlockManager;
    public GameObject Coin;

    int nowCoin;                // 소지하고 있는 코인(점수)
    int coin = 2;               // 블록 파괴시 획득하는 코인

    int blockType = 1;
    bool trick = false;         // 블록이 투명 블록인가?    

    int healthPoint = 1;        // 블록의 체력

    public static bool stop = true;     // 블록 생성을 멈출 것인가, 계속할 것인가

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
            Debug.Log("게임 오버");
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