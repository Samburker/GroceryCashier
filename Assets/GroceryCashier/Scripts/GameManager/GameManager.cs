using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Singleton { get; private set; }
    public ScreenFade fade;

    public int day = 0;
    public GameDay[] gameDays;


    private void Awake()
    {
        DontDestroyOnLoad(transform);
        SceneManager.sceneLoaded += OnSceneLoaded;
        Singleton = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        UpdateSceneSpecificSettings(SceneManager.GetActiveScene().buildIndex);
    }


    public void SwitchScene(int sceneNumber)
    {
        if (fade == null)
            SceneManager.LoadScene(sceneNumber);
        else
        {
            // Fading and then loading scene
            fade.Fade(1f, delegate
            {
                SceneManager.LoadScene(sceneNumber);
            });
        }
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        UpdateSceneSpecificSettings(arg0.buildIndex);
        // Fading back after the scene load
        if (fade != null)
            fade.Fade(0f);
    }
    private void UpdateSceneSpecificSettings(int buildIndex)
    {
        switch (buildIndex)
        {
            case 0:
                PlayerInputs.Singleton.onMenu = true;
                break;
            default:
                PlayerInputs.Singleton.onMenu = false;
                break;
        }
    }
}
