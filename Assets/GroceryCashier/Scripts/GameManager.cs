using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Singleton { get; private set; }
    public ScreenFade fade;

    private void Awake()
    {
        DontDestroyOnLoad(transform);
        SceneManager.sceneLoaded += OnSceneLoaded;
        Singleton = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
        // Fading back after the scene load
        if(fade != null)
            fade.Fade(0f);
    }
}
