using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PuaseMenu : MonoBehaviour
{
    public GameObject pauseCanves;
    public SceneFader fader;

    public void Pause()
    {
        pauseCanves.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        pauseCanves.SetActive(false);
        Time.timeScale = 1f;
    }

    public void MenuButton()
    {
        //SceneManager.LoadScene("MainMenu");
        fader.FadeTo("MainMenu");
        Time.timeScale = 1f;
    }
}

