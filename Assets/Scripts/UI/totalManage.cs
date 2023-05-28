using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class totalManage : MonoBehaviour
{
    /* �Ѿ� ���� ��ư */
    public GameObject selectBullet;

    /* ��� ǥ�� UI */
    public GameObject Life;

    /* �Ѿ� ���� ��ư Ŭ�� �� ���̶���Ʈ �ڽ� */
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
    int selectNum = 0;              // ���� bullet�� �����Ͽ��°�. 0->�⺻, 1->����, 2->����, 3->��ź        

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
    // ���ӽ��� ȭ������ ���ư���
    public void pressReplay()
    {
        SceneManager.LoadScene("survivalScene");
    }
    public void pressExit()
    {
        Application.Quit();
    }

    // bullet�� �����ϴ� ��ư�� ������ �� ���ù�ư���� ��������, �̹� �����ִٸ� ������
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
