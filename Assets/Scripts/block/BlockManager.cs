using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockManager : MonoBehaviour
{
    bool GameOver = false;
    bool Replay = false;
    public void setGameOver() { GameOver = true; }
    public void setReplay(bool replay) { Replay = replay; }    

    /* 스테이지 박스 */
    public GameObject stage;

    /* 블럭 종류 */
    public GameObject basicBlock;
    public GameObject twoTimeBlock;
    public GameObject bombBlock;
    public GameObject missingBlock;

    int blockType;
    public int sendBlockType()
    {
        return blockType;
    }
    int stageLevel = 1;
    bool startGame = false;
    bool stop = false;

    public void setStageLevel(int stageLevel) { this.stageLevel = stageLevel; }
    public void setMakeBlock(bool exist)
    {
        stop = exist;
    }
    private void Update()
    {
        if (!startGame)
        {
            stop = true;
            stage.GetComponent<Text>().text = "phase1 : " + stageLevel;
            stageLevel++;
            startGame = true;
            makeBlock(stageLevel - 1);
        }
        if (GameOver)
        {
            stop = true;
        }
        if (Replay)
        {
            stop = false;
            Replay = false;

            int childNum = GameObject.Find("massBlock").transform.childCount;
            for (int i = 0; i < childNum; i++)
            {
                Destroy(GameObject.Find("massBlock").transform.GetChild(0));
            }
        }

        //bullet이 모두 없어졌단 정보와 스테이지 정보를 받아오기                
        if (!stop && startGame)
        {
            for (int i = 0; i < GameObject.Find("massBlock").transform.childCount; i++)
            {
                GameObject.Find("massBlock").transform.GetChild(i).Translate(0f, -0.4f, 0f);

            }
            makeBlock(stageLevel);
            stageLevel++;
            stop = true;
            if (stageLevel >= 50)
            {
                if (stageLevel >= 70)
                {
                    for (int i = 0; i < GameObject.Find("massBlock").transform.childCount; i++)
                    {
                        GameObject.Find("massBlock").transform.GetChild(i).Translate(0f, -0.6f, 0f);

                    }
                    makeBlock(stageLevel);
                }
                for (int i = 0; i < GameObject.Find("massBlock").transform.childCount; i++)
                {
                    GameObject.Find("massBlock").transform.GetChild(i).Translate(0f, -0.6f, 0f);

                }
                makeBlock(stageLevel);
            }
        }
    }

    private void makeBlock(int stageLevel)
    {
        int initNum = 0;

        // 블럭을 만들 순서를 정한다. 1이 제일 왼쪽, 5가 가장 오른쪽
        int[] numList = new int[4];             // 순서를 담는 배열
        int lnum = 0;

        int loopNum = 0; //무한루프 방지
        while (true)
        {
            bool exist = false;
            int randNum = Random.Range(0, 4) + 1;
            foreach (var item in numList)
            {
                if (item == randNum)             // 이미 배열에 있는 숫자면 다시 Random함수 돌리도록 함.
                {
                    exist = true;
                    break;
                }
            }
            if (!exist)
            {
                numList[lnum] = randNum;
                lnum++;
            }
            if (lnum == 4) { break; }

            if (loopNum++ > 10000)
            {
                Debug.Log("Infinite Loop");
                break;
            }
        }


        // 블럭을 만들 것인가, 만들지 않을것 인가, 이미 블럭이 3개이상 만들어 졌을 경우, 블럭이 하나도 만들어 지지 않았을 경우
        for (int i = 0; i < 4; i++)
        {            
            if (stageLevel <= 40 && (initNum >= 3))
            {
                break;
            }           
            else 
            {
                initNum++;
                blockType = 1;
                if (stageLevel > 30)
                {
                    blockType += Random.Range(0, 4);
                }
                else if (stageLevel > 20)
                {
                    blockType += Random.Range(0, 3);
                }
                else if (stageLevel > 10)
                {
                    blockType += Random.Range(0, 2);
                }


                GameObject tmpblock;
                switch (blockType)
                {
                    case 2:
                        tmpblock = Instantiate(twoTimeBlock) as GameObject;
                        tmpblock.gameObject.GetComponent<Block>().setBlockType(blockType);
                        break;
                    case 3:
                        tmpblock = Instantiate(bombBlock) as GameObject;
                        tmpblock.gameObject.GetComponent<Block>().setBlockType(blockType);
                        break;
                    case 4:
                        tmpblock = Instantiate(missingBlock) as GameObject;
                        tmpblock.gameObject.GetComponent<Block>().setBlockType(blockType);
                        break;
                    default:
                        tmpblock = Instantiate(basicBlock) as GameObject;
                        tmpblock.gameObject.GetComponent<Block>().setBlockType(blockType);
                        break;
                }

                tmpblock.transform.parent = GameObject.Find("massBlock").transform;

                switch (numList[i])
                {
                    case 1:
                        tmpblock.transform.position = new Vector3(-1.5f, 3.6f, 0);
                        break;
                    case 2:
                        tmpblock.transform.position = new Vector3(-0.5f, 3.6f, 0);
                        break;
                    case 3:
                        tmpblock.transform.position = new Vector3(0.5f, 3.6f, 0);
                        break;
                    case 4:
                        tmpblock.transform.position = new Vector3(1.5f, 3.6f, 0);
                        break;
                }
            }
        }

        if (stageLevel <= 10)
        {
            stage.GetComponent<Text>().text = "phase1 : " + stageLevel;
        }
        else if (stageLevel <= 20)
        {
            stage.GetComponent<Text>().text = "phase2 : " + stageLevel;
        }
        else if (stageLevel <= 30)
        {
            stage.GetComponent<Text>().text = "phase3 : " + stageLevel;
        }
        else if (stageLevel <= 40)
        {
            stage.GetComponent<Text>().text = "phase4 : " + stageLevel;
        }
        else
        {
            stage.GetComponent<Text>().text = "God Stage";
        }
    }
}
