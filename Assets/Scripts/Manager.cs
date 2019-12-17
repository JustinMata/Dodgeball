using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public static string lastScene;

    private static AudioSource audio;

    private static Manager _instance;

    public static Manager Instance { get { return _instance; } }

    private void Awake()
    {
        Screen.SetResolution(1024, 768, true);
        DontDestroyOnLoad(gameObject);
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public void playAgain()
    {
        audio.Play();
        if (lastScene == "MainScene")
            SceneManager.LoadScene("MainScene");
        else if (lastScene == "AIvsPlayer")
            SceneManager.LoadScene("AIvsPlayer");
        else if (lastScene == "2AIvs2Players")
            SceneManager.LoadScene("2AIvs2Players");
    }

    public void playerVsPlayer()
    {
        audio.Play();
        SceneManager.LoadScene("MainScene");
    }

    public void AIvsPlayer()
    {
        audio.Play();
        SceneManager.LoadScene("AIvsPlayer");
    }

    public void Coop()
    {
        audio.Play();
        SceneManager.LoadScene("2AIvs2Players");
    }

    public void howToPlay()
    {
        SceneManager.LoadScene("HowToPlay");
    }

    public void back()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("StartScreen");
    }

    public void resume()
    {
        GameObject.Find("PauseCanvas").SetActive(false);
        Time.timeScale = 1;
        audio.Play();
    }

    public static void endGame(string scene)
    {
        lastScene = scene;
        audio.Stop();
    }
    
    public void quitGame()
    {
        Application.Quit();
    }

    void Start()
    {
        audio = Instance.GetComponent<AudioSource>();
    }

    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(EventSystem.current.firstSelectedGameObject);
        }
    }
}
