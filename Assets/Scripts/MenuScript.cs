using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    GameObject menuPanel, optionsPanel;
    bool inOptions;
    Text bestScoreText;
    Slider volumeSlider;
    public static int colorNum;
    const int whiteColor = 1, redColor = 2, blueColor = 3;

    void Awake()
    {
        inOptions = false;
        menuPanel = GameObject.Find("MenuPanel");
        optionsPanel = GameObject.Find("OptionsPanel");
        bestScoreText = GameObject.Find("BestScoreText").GetComponent<Text>();
        volumeSlider = GameObject.Find("VolumeSlider").GetComponent<Slider>();
        menuPanel.SetActive(true);
        optionsPanel.SetActive(false);
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1f);
        colorNum = PlayerPrefs.GetInt("ColorN", 1);
    }

    void Update()
    {
        bestScoreText.text = "Best Score: " + SnakeScr.bestScore.ToString();
        if(inOptions)
        {
            menuPanel.SetActive(false);
            optionsPanel.SetActive(true);
        }
        else
        {
            menuPanel.SetActive(true);
            optionsPanel.SetActive(false);
        }
        AudioListener.volume = volumeSlider.value;
    }

//TODO: rename in more obvious way
    public void SetWhiteColor()
    {
        colorNum = whiteColor;
        PlayerPrefs.SetInt("ColorN", colorNum);
    }

    public void SetRedColor()
    {
        colorNum = redColor;
        PlayerPrefs.SetInt("ColorN", colorNum);
    }

    public void SetBlueColor()
    {
        colorNum = blueColor;
        PlayerPrefs.SetInt("ColorN", colorNum);
    }

    public void OpenGameLevel()
    {
        SceneManager.LoadScene("LevelOne");
    }

    public void OptionsActivation()
    {
        inOptions = !inOptions;
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
    }

    public void ResetScore()
    {
        PlayerPrefs.SetInt("BestScore", 0);
        SnakeScr.bestScore = 0;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
