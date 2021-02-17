using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{
    public bool GameIsPaused = false;

    public GameObject pauseMenu;
    public GameObject endMenu; 
    public GameObject scoreText;
    public GameObject goalDisplay;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject Football = GameObject.Find("Football");
        MouseDragControl mouseDragControl = Football.GetComponent<MouseDragControl>();

        if (mouseDragControl.gameIsOver)
        {
            GameOver();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        GameIsPaused = true;
    }

    public void GameOver()
    {
        endMenu.SetActive(true);
        goalDisplay.SetActive(false);
        Time.timeScale = 0;
        GameIsPaused = true;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Stadium");
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Start");
        Time.timeScale = 1;
    }
}
