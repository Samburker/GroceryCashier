
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Singleton { get; private set; }
    public ScreenFade fade;
    public int day = 0;
    public GameDay[] gameDays;    
    public PauseMenu pauseMenuPrefab;
    private PauseMenu _pauseMenu;
    internal SceneDescriptor sceneDescriptor;

    #region UNITY CALLBACKS
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
        PlayerInputs.Singleton.OnPauseStatusChanges += OnPause;
    }

    private void OnEnable()
    {
        PlayerInputs.Singleton.OnPauseStatusChanges += OnPause;
    }

    private void OnDisable()
    {
        PlayerInputs.Singleton.OnPauseStatusChanges -= OnPause;
    }
    #endregion

    internal void StartDay(int day)
    {
        this.day = day;
        SwitchScene(gameDays[day].sceneNumber);
    }

    public void SwitchScene(int sceneNumber)
    {
        if (fade == null)
            SceneManager.LoadScene(sceneNumber);
        else
        {
            // Fading and then loading scene
            fade.Fade(1f, () => SceneManager.LoadScene(sceneNumber));
        }
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        UpdateSceneSpecificSettings(arg0.buildIndex);

        StartCoroutine(GameDayCoroutine());
        // Fading back after the scene load
        if (fade != null)
            fade.Fade(0f);
    }
    private void UpdateSceneSpecificSettings(int buildIndex)
    {
        // If we are on menu scene
        if (buildIndex == 0)
        {
            PlayerInputs.Singleton.onMenu = true;
        }
        // If we are on game scene
        else if (buildIndex == 1)
        {
            PlayerInputs.Singleton.onMenu = false;
            _pauseMenu = Instantiate(pauseMenuPrefab);
            _pauseMenu.Hide();
        }
    }

    private IEnumerator GameDayCoroutine()
    {
        Debug.Log("Starting Game day " + gameDays[day].ToString());
        GameDay d = gameDays[day];
        sceneDescriptor = FindObjectOfType<SceneDescriptor>();
        foreach (var register in sceneDescriptor.cashRegisters)
        {
            register.checkoutCounter.cigarette = d.cigarets;
            register.checkoutCounter.security = d.security;
        }

        GroceryFirstPersonController player = Instantiate(sceneDescriptor.playerPrefab);
        Transform spawn = sceneDescriptor.playerSpawnpoints[0].transform;
        player.SetPositionAndRotation(spawn.transform.position, spawn.rotation);

        if(fade != null)
            yield return new WaitUntil(() => fade.IsCompleted);

        yield return new WaitForSeconds(2f);

        CustomerManager customerManager = CustomerManager.Singleton;
        int customers = Random.Range(d.customerAmountMin, d.customerAmountMax);
        for (int c = 0; c < customers; c++)
        {
            customerManager.shoppingList = gameDays[day].shoppingList;
            customerManager.Spawn();
            yield return new WaitForSeconds(Random.Range(d.customerSpawnIntervalMin, d.customerSpawnIntervalMax));
        }

        yield return new WaitUntil(() => customerManager.CustomerCount() == 0);

        Debug.Log("Day ${day} ended");

        // Next day
        StartDay(day + 1);
    }

    private void OnPause(bool obj)
    {
        if (obj)
        {
            _pauseMenu?.Show();
            Time.timeScale = 0f;
        }
        else
        {
            _pauseMenu?.Hide();
            Time.timeScale = 1;
        }
    }
}
