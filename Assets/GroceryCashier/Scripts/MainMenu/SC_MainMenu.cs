using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_MainMenu : MonoBehaviour
{
    public GameObject MainMenu;
   

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void PlayNowButton()
    {
        // Play Now Button has been pressed, here you can initialize your game 
        GameManager.Singleton.SwitchScene(1);
    }

    public void QuitButton()
    {
        // Quit Game
        Application.Quit();
    }
}
