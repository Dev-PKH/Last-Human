using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public PoolManager pool;
    // Player player;

    public int curEnemy;
    public int zombieDaamge = 5;
    //const int maxEnemy = 100;


    public bool isOver;
    public bool isClear;

    [Header("GameOver")]
    public Image overImg;
    public Image[] progressImages;
    public Slider progress;
    public Timer timer;
    public Text ResultText;
    public bool isBoss;
    public int BossCnt =0;
    public int curBoss = -1;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public void UpgradeMoster()
    {
        pool.LevelUp();
    }

    public void ResetGame()
    {
        isOver = false;
        isClear = false;
        AudioManager.instance.PlaySound(EAudio.Click);
        AudioManager.instance.BGM_Change(0);
        SceneManager.LoadScene(0);
    }

    public void Result()
    {
        int complete;
        if (timer.Min < 3)
            complete = 0;
        else if (timer.Min < 6)
            complete = 1;
        else if (timer.Min < 9)
            complete = 2;
        else if (timer.Min < 10)
            complete = 3;
        else
            complete = 4;

        progress.value = complete * 0.25f;

        if (complete == 4)
            ResultText.text = "CLEAR!!!";

        for (int i = 0; i < complete; i++)
            progressImages[i].color = new Color(255, 255, 255, 255);
    }

    public void Dead()
    {
        isOver = true;
        AudioManager.instance.PlaySound(EAudio.GameOver);

        overImg.gameObject.SetActive(true);
        Result();
    }

    public void GameClear()
    {
        isClear = true;
        AudioManager.instance.PlaySound(EAudio.GameClear);

        overImg.gameObject.SetActive(true);
        Result();
    }

    public void Boss(int num)
    {
        isBoss = true;
        pool.GenerateBoss(num);
    }

    public void CheckMidleBoss()
    {
        BossCnt++;
        if (BossCnt != 3) isBoss = false; 
    }
}
