using System;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private bool gamePaused = false;
    private float gamePlayedTime;

    public List<GameObject> playersObject;
    public GameObject timerUI;
    public GameObject scoreUI;
    public GameObject gameOverScoreUI;
    public GameObject gameoverDialog;
    public GameObject pauseDialog;

    private TMPro.TextMeshProUGUI timerUIText;
    private TMPro.TextMeshProUGUI scoreUIText;
    private TMPro.TextMeshProUGUI gameOverScoreUIText;
    private RectTransform gameoverDialogRect;
    private RectTransform pauseDialogRect;
    // Start is called before the first frame update

    void Start()
    {
        Time.timeScale = 1;
        gamePlayedTime = 0;

        timerUIText = timerUI.GetComponent<TMPro.TextMeshProUGUI>();
        scoreUIText = scoreUI.GetComponent<TMPro.TextMeshProUGUI>();
        gameOverScoreUIText = gameOverScoreUI.GetComponent<TMPro.TextMeshProUGUI>();
        gameoverDialogRect = gameoverDialog.GetComponent<RectTransform>();
        pauseDialogRect = pauseDialog.GetComponent<RectTransform>();
        
        gameoverDialog.SetActive(false);
        pauseDialog.SetActive(false);
        MyEvents.OnPlayerDeath.AddListener(OpenGameoverDialog);


        PlayerPrefs.SetInt("Score", 0);
    }

    // Update is called once per frame
    void Update()
    {
        gamePlayedTime += Time.deltaTime;
        updateGameUI();
        checkPlayerStatus();
    }

    void updateGameUI()
    {
        timerUIText.text = getFormattedTime(gamePlayedTime);
        scoreUIText.text = PlayerPrefs.GetInt("Score").ToString();
    }

    void checkPlayerStatus()
    {
        int deathCount = 0;
        foreach (GameObject player in playersObject)
        {
            if (player.GetComponent<PlayerController>().isPlayerDeath())
            {
                deathCount++;
            }
        }

        if (deathCount == playersObject.Count)
        {
            OpenGameoverDialog();
        }
    }

    private string getFormattedTime(float timeInSeconds)
    {
        int minutes = ((int)timeInSeconds) / 60;
        int seconds = ((int)timeInSeconds) % 60;
 
        return minutes + ":" + ((seconds < 10)? "0" + seconds : seconds.ToString());
    }

    public void OpenPauseDialog()
    {
        // pauseDialogRect.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width/2, Screen.height/2, 0));
        pauseDialog.SetActive(true);
        gamePaused = true;
        Time.timeScale = 0;
    }

    public void ClosePauseDialog()
    {
        // pauseDialogRect.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width/2, 9999999, 0));
        pauseDialog.SetActive(false);
        gamePaused = false;
        Time.timeScale = 1.0f;
    }

    public void OpenGameoverDialog()
    {
        // gameoverDialogRect.position = new Vector3(gameoverDialogRect.position.x, 0, gameoverDialogRect.position.z);
        gameOverScoreUIText.text = "Score: " + PlayerPrefs.GetInt("Score");
        gameoverDialog.SetActive(true);
        gamePaused = true;
        Time.timeScale = 0;
    }
}
