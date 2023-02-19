using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Singleton { get; private set; }
    public ScreenFade fade;

    public int day = 0;
    public GameDay[] gameDays;
    private SceneDescriptor sceneDescriptor;

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

    internal void StartDay(int day)
    {
        this.day = day;
        SwitchScene(1);
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
        // Uggly dependencies :S
        // TODO: Refactor this

        // If we are on menu scene
        if (buildIndex == 0)
        {
            PlayerInputs.Singleton.onMenu = true;
        }
        // If we are on game scene
        else if (buildIndex == 1)
        {
            PlayerInputs.Singleton.onMenu = false;
            sceneDescriptor = GameObject.FindObjectOfType<SceneDescriptor>();
            foreach (var cr in sceneDescriptor.cashRegisters)
            {
                cr.checkoutCounter.cigarette = gameDays[day].cigarets;
                cr.checkoutCounter.security = gameDays[day].security;
            }

            GroceryFirstPersonController player = Instantiate(sceneDescriptor.playerPrefab);
            Transform spawn = sceneDescriptor.playerSpawnpoints[0].transform;
            player.SetPositionAndRotation(spawn.transform.position,spawn.rotation);
        }
    }
}
