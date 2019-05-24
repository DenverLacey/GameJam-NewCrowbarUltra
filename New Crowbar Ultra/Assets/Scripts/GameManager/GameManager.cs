using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static bool GameIsPaused = false;
    private static bool gameWin = false;

    private GameObject m_winStateUI;
    private GameObject m_healthbarUI;
    private GameObject m_pauseMenuUI;

    private void Start()
    {
        m_healthbarUI = GameObject.FindGameObjectWithTag("HealthBarUI");
        m_pauseMenuUI = GameObject.FindGameObjectWithTag("PauseUI");
        m_winStateUI = GameObject.FindGameObjectWithTag("WinStateUI");
        m_winStateUI.SetActive(false);
        m_pauseMenuUI.SetActive(false);
        m_healthbarUI.SetActive(true);
    }
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Debug.Log("game quit");
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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

    public void Resume()
    {
        m_pauseMenuUI.SetActive(false);
        Time.timeScale = 1.0f;
        GameIsPaused = false;

    }

    public void Pause()
    {
        m_pauseMenuUI.SetActive(true);
        Time.timeScale = 0.0f;
        GameIsPaused = true;
    }

    public void LoadMainMenu()
    {

        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MainMenu");
    }

    public void WinState ()
    {
        m_winStateUI.SetActive(true);
        m_healthbarUI.SetActive(false);
        gameWin = true;
        Time.timeScale = 0.0f;
    }

    public void OnTriggerEnter(Collider Collision)
    {
        if(Collision.gameObject.GetComponent<Robbo>())
        {
            WinState();
        }
    }
}