using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject gamePanel, pausePanel, gameOverPanel;
    bool isPause;
    public static bool isRestart;
    Text startText;

    void Start()
    {
        startText = GameObject.Find("startText").GetComponent<Text>();
        isRestart = false;
        pausePanel.SetActive(false);
        gamePanel.SetActive(true);
        gameOverPanel.SetActive(false);
        isPause = false;
    }

    void Update()
    {
        if(Input.touchCount > 0)
        {
            startText.color = Color.clear;
        }
        if (isPause)
        {
            pausePanel.SetActive(true);
            gamePanel.SetActive(false);
            Time.timeScale = 0;
        }
        else 
        {
            pausePanel.SetActive(false);
            gamePanel.SetActive(true);
            Time.timeScale = 1;
        }

        if(Input.GetKeyDown(KeyCode.Escape) && !SnakeScr.isGameOver)
        {
            isPause = !isPause;
        }

        if(SnakeScr.isGameOver)
        {
            pausePanel.SetActive(false);
            gamePanel.SetActive(false);
            gameOverPanel.SetActive(true);
        }
        
        if(Input.GetKeyDown(KeyCode.R) && !isPause)
        {
            pausePanel.SetActive(false);
            gamePanel.SetActive(true);
            gameOverPanel.SetActive(false);
        }
    }

    public void PauseMenu()
    {
        if(!SnakeScr.isGameOver)
        {
            isPause = !isPause;
        }
    }

    public void GameOverMenu()
    {
        if(!isPause)
        {
            isRestart = true;
            pausePanel.SetActive(false);
            gamePanel.SetActive(true);
            gameOverPanel.SetActive(false);
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
