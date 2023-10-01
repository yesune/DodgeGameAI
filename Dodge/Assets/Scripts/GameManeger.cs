using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //UI ���� ���̺귯��
using UnityEngine.SceneManagement; //�� ���� ���� ���̺귯��
using NeuralNetwork;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject gameoverText; //���� ���� �� Ȱ��ȭ �� �ؽ�Ʈ ������Ʈ
    public Text timeText; //�����ð��� ǥ���� �ؽ�Ʈ ������Ʈ
    public Text recordText; //�ְ� ����� ǥ���� �ؽ�Ʈ ������Ʈ

    private float surviveTime;  //�����ð�
    private bool isGameOver; //���� ���� ����

    public PlayerAI playerAI;
    public PersistentObj persistent;


    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        //���� �ð��� ���� ���� ���� �ʱ�ȭ
        surviveTime = 0;
        isGameOver = false;
        persistent = FindObjectOfType<PersistentObj>();
        print(persistent);
        playerAI = persistent.playerAI;
        Vision[] visions = getVisions();
        persistent.restart(visions);
        persistent.setRadar(getRadar());
        persistent.playerAI.agent.setPlayerAI();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameOver)
        {
            //�����ð� ����
            surviveTime += Time.deltaTime;
            // ������ �����ð��� timeText �ؽ�Ʈ ������Ʈ�� �̿��� ǥ��
            timeText.text = "Time: " + (int)surviveTime;
        }
        else
        {
            /* if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene("SampleScene");

            } */
            if (playerAI.episode_counter < playerAI.episodes) {
                playerAI.episode_counter += 1;
                playerAI.train();
                SceneManager.LoadScene("SampleScene");
            }
        }

    }
    
    public void EndGame()
    {
        
        isGameOver = true;
        playerAI.agent.punishRecentActions();
        persistent.SaveTrainingData();
        //gameoverText.SetActive(true);

        //BestTime Ű�� ����� �ְ���� ��������
        /*float bestTime = PlayerPrefs.GetFloat("BestTime");

        //���������� �ְ� ��� ���� ���� �ð��� ��ٸ�
        if (surviveTime > bestTime)
        {
            bestTime = surviveTime;
            PlayerPrefs.SetFloat("BestTime", bestTime);
        }

        recordText.text = "Best.Time : " + (int)bestTime;*/
        
    }

    // FOR FUTURE ME WHEN YOU WANT TO PAUSE THE GAME FOR TRAINING
    // use Time.timeScale = 0 to pause the game and wait for the thing to finish training
    // might have to use threading to observe when its finished

    public Vision[] getVisions() {
        Vision[] visions = new Vision[8];
        for (int i = 0; i < 8; i++) {
            visions[i] = GameObject.Find("Vision" + i).GetComponent<Vision>();
        }
        return visions;
    }

    public Radar getRadar() {
        return GameObject.Find("Player").GetComponent<Radar>();
    }

    public bool getIsGameOver() {
        return isGameOver;
    }
    
}