using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class totalManage : MonoBehaviour
{
    /* 총알 선택 버튼 */
    public GameObject selectBullet;

    /* 목숨 표시 UI */
    public GameObject Life;

    /* 총알 선택 버튼 클릭 시 하이라이트 박스 */
    public GameObject hlbasicBullet;
    public GameObject hlthTimeBullet;
    public GameObject hlflowerBullet;
    public GameObject hlbombBullet;

    public GameObject GameOver;
    public GameObject Menu;
    public GameObject Back;
    public GameObject Replay;
    public GameObject Exit;


    //bool going = false;

    bool selectHide = true;
    int selectNum = 0;              // 무슨 bullet을 선택하였는가. 0->기본, 1->연속, 2->폭죽, 3->폭탄        

    public int sgCoin { get; set; }

    public void gameOver()
    {
        GameOver.SetActive(true);
        GameObject.Find("totalScore").GetComponent<Text>().text = sgCoin.ToString();
    }

    public void presskMenu()
    {
        Menu.SetActive(true);
    }
    public void pressBack()
    {
        Menu.SetActive(false);
    }
    // 게임시작 화면으로 돌아가기
    public void pressReplay()
    {
        SceneManager.LoadScene("survivalScene");
    }
    public void pressExit()
    {
        Application.Quit();
    }

    // bullet을 선택하는 버튼을 눌렀을 때 선택버튼들이 나오도록, 이미 나와있다면 들어가도록
    public void moveSelectBullet()
    {
        if (!selectHide)
        {
            selectBullet.transform.Translate(300f, 0f, 0f);
            selectHide = true;
        }
        else
        {
            selectBullet.transform.Translate(-300f, 0f, 0f);
            selectHide = false;
        }
    }

    public void selectBasic()
    {
        selectNum = 0;
        hlbasicBullet.SetActive(true);
        hlthTimeBullet.SetActive(false);
        hlflowerBullet.SetActive(false);
        hlbombBullet.SetActive(false);
    }
    public void selectthTime()
    {
        if (sgCoin < 5)
        {
            return;
        }

        hlbasicBullet.SetActive(false);
        hlthTimeBullet.SetActive(true);
        hlflowerBullet.SetActive(false);
        hlbombBullet.SetActive(false);

        selectNum = 1;
    }
    public void selectFlower()
    {
        if (sgCoin < 15)
        {
            return;
        }

        hlbasicBullet.SetActive(false);
        hlthTimeBullet.SetActive(false);
        hlflowerBullet.SetActive(true);
        hlbombBullet.SetActive(false);

        selectNum = 2;
    }
    public void selectBomb()
    {
        if (sgCoin < 20)
        {
            return;
        }

        hlbasicBullet.SetActive(false);
        hlthTimeBullet.SetActive(false);
        hlflowerBullet.SetActive(false);
        hlbombBullet.SetActive(true);

        selectNum = 3;
    }

    public int sendSelectNum() { return selectNum; }

    void Start()
    {
        sgCoin = 0;
    }
}
