using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class loadNewScene : MonoBehaviour
{
    public GameObject quitMenu;
    public GameObject mainMenu;
    public GameObject optionMenu;
    public Button HowToPlay;
    public Image loadingIMG;

    public Text highscore;

    public Text retries;
    int intRetries;

    private DateTime lastDate, currentDate;
    private TimeSpan cycle;
    private double result;
    private string lastTime;

    // Start is called before the first frame update

    private void Awake()
    {
        //PlayerPrefs.DeleteAll();
    }

    private void Start()
    {
        intRetries = PlayerPrefs.GetInt("retries", 5);
        PlayerPrefs.SetInt("retries", intRetries);

        lastTime = PlayerPrefs.GetString("lastTime", DateTime.Now.ToString());
        PlayerPrefs.SetString("lastTime", lastTime);
        
        lastDate = DateTime.Parse(lastTime);

        currentDate = DateTime.Now;
        cycle = currentDate.Subtract(lastDate);
        result = cycle.TotalMinutes;

        if (result >= 5)
        {
            PlayerPrefs.SetInt("retries", 5);
            PlayerPrefs.SetString("lastTime", currentDate.ToString());

            Debug.Log("retries reset");
        }

        intRetries = PlayerPrefs.GetInt("retries");
        retries.text = "Daily retries : " + intRetries;

        highscore.text = "HighScore : " + PlayerPrefs.GetInt("highscore",0);
        Time.timeScale = 1;

    }
    private void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                if (optionMenu.activeSelf)
                {
                    optionMenu.SetActive(false);
                    mainMenu.SetActive(true);
                    return;
                }

                if (HowToPlay.gameObject.activeSelf)
                {
                    HowToPlay.gameObject.SetActive(false);
                    optionMenu.SetActive(true);
                    return;
                }


                quitMenu.SetActive(true);
                mainMenu.SetActive(false);

                return;
            }


        }
    }
    public void playGame()
    {
        //SceneManager.LoadScene("loadingScene");
        StartCoroutine(LoadAsyncOperation());
    }

    IEnumerator LoadAsyncOperation()
    {
        AsyncOperation gameLevel = SceneManager.LoadSceneAsync(1);

        while (gameLevel.progress < 1)
        {
            loadingIMG.fillAmount = gameLevel.progress;
            yield return new WaitForEndOfFrame();
        }


    }
}
