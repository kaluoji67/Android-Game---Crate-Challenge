using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    public Text highscore;

    private void Start()
    {
        highscore.text = "High Score : " + PlayerPrefs.GetInt("highscore",0);
    }
    public void quitGame()
    {
        Application.Quit();
    }

    
    
}
