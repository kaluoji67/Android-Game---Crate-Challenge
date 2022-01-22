using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;


public class gameControllerScript : MonoBehaviour
{
    [Header("From other Scripts")]
    public Camera mainCam;
    public balancer leftBal;
    public balancer rightBal;

    public Image leftUI;
    public Image rightUI;

    public MovementControl control;

    public ragDollController controlR;

    public spawnCrates crateSpawner;

    //public List<Transform[]> cratesList;

    public bool gameOverBool;

    float mostCurrentCrate;

    public Text resumeCountDown;

    public GameObject gameOverPanel;
    public Button pauseButton;
    public Text score;

    public Image reloadImage;

    public Text newScore;
    public Text highScore;
    public float pattern;
    public int intScore;
    public int intHighScore;
    public Button howToPlay;

    public GameObject pauseMenu;
    public GameObject gameOverMenu;

    public GameObject fallSound;

    /// //////////////////////
    public Button retryButton;
    public Text retriesText;

    public Image crownImage;

    // Calculate retries
    private DateTime lastDate, currentDate;
    private TimeSpan cycle;
    private double result;
    private int intRetries;
    private string lastTime;

    public AdsManager adsManager;

    public GameObject informPanel;
    public Text countdownInform;
    
    public float lerp;

    playServicesHandler playHandler;
    // Start is called before the first frame update
    void Start()
    {


        intHighScore = PlayerPrefs.GetInt("highscore", 0);
        if (intHighScore==0)
        {
            howToPlay.gameObject.SetActive(true);
        }

        //calculateRetries();
        intRetries = PlayerPrefs.GetInt("retries");
        Debug.Log("retries " + intRetries);

        if (intRetries == 0)
        {
            
            Time.timeScale = 0;
            informPanel.SetActive(true);
            StartCoroutine(countDown());

        }
        else {
            informPanel.SetActive(false);
            //set retries
            intRetries--;
            if (intRetries < 0)
                intRetries = 0;
            PlayerPrefs.SetInt("retries", intRetries);
        }
    }
    IEnumerator countDown()
    {
        lerp = 5;
        while ( lerp>0)
        {
            countdownInform.text = "Ad Starting in " + lerp;
            lerp--;
            yield return new WaitForSecondsRealtime(1f);

        }

        if (informPanel.activeSelf)
        {
            adsManager.DisplayRewardedInterstitialAd();
            yield return null;
        }
        
        yield return null;

    }

    public void cancelInformaction()
    {
        if (!gameOverBool)
        {
            goToMainMenu();
        }
    }


    // Update is called once per frame
    void Update()
    {
        mostCurrentCrate = Mathf.Max(leftBal.newCrate, rightBal.newCrate);
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                if (howToPlay.gameObject.activeSelf)
                {
                    howToPlay.gameObject.SetActive(false);
                    pauseMenu.SetActive(true);
                    return;
                }

                if (gameOverMenu.activeSelf)
                {
                    howToPlay.gameObject.SetActive(false);
                    pauseMenu.SetActive(false);
                    Time.timeScale = 1;
                    SceneManager.LoadScene(0);
                    return;
                }

                pauseMenu.SetActive(true);

                return;
            }
            Application.targetFrameRate = 30;

            QualitySettings.vSyncCount = 0;

            QualitySettings.antiAliasing = 0;

            QualitySettings.shadowCascades = 0;
            QualitySettings.shadowDistance = 15;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;

        }


    }

    public void pauseGame()
    {
        Time.timeScale = 0;
    }

    public void resumeGame()
    {
        StartCoroutine(resumeGameCoroutine());
    }
    public IEnumerator resumeGameCoroutine()
    {
        int countDown = 3;
        resumeCountDown.gameObject.SetActive(true);
        while (countDown>0)
        {
            resumeCountDown.text = countDown.ToString();
            countDown--;
            yield return new WaitForSecondsRealtime(1f);

        }
        resumeCountDown.gameObject.SetActive(false) ;
        Time.timeScale = 1;
        yield return null;
    }

    public void goToMainMenu()
    {
        int rand = UnityEngine.Random.Range(0,4);
        if (rand == 0)
        {
            adsManager.DisplayInterstitial();
        }
        else {
            Time.timeScale = 1;
            SceneManager.LoadScene(0);
        }
        
    }

    public void gameOver()
    {
        controlR.activateRagdoll();
        fallSound.SetActive(true);
        StopCoroutine( leftBal.randomForce());
        StopCoroutine(rightBal.randomForce());

        Destroy(leftBal.gameObject);
        Destroy(rightBal.gameObject);
        resetDrag();
   
        leftUI.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        rightUI.GetComponent<Rigidbody2D>().velocity = Vector3.zero;

        leftUI.GetComponent<Rigidbody2D>().isKinematic = true;
        rightUI.GetComponent<Rigidbody2D>().isKinematic = true;
       

        gameOverBool = true;

        StartCoroutine(gameOverCoroutine());
    }
    IEnumerator gameOverCoroutine()
    {
        
        yield return new WaitForSecondsRealtime(3f);

        pauseButton.gameObject.SetActive(false);
        gameOverPanel.SetActive(true);

        calculateRetries();


        newScore.text = "New Score : " + score.text;
        intScore = control.globalCounter;
        intHighScore = PlayerPrefs.GetInt("highscore",0);

        //set highScore
        if (intScore > intHighScore)
        {
            PlayerPrefs.SetFloat("pattern", intScore / 70f);
            PlayerPrefs.SetInt("highscore", intScore);
            highScore.text = "High Score : " + score.text;
            crownImage.gameObject.SetActive(true);

            if(playServicesHandler._instance.isConnectedToGooglePlayServices)
            {
                Social.ReportScore(intScore, GPGSIds.leaderboard_leaderboard, (success) =>
                {
                    if (!success)
                        Debug.LogError("unable to post highscore");
                });
            }

        }
        else {
            highScore.text = "High Score : " + intHighScore.ToString();
            crownImage.gameObject.SetActive(false);
        }
        
        score.gameObject.SetActive(false);


        yield return new WaitForSecondsRealtime(2f);
        
        
        if (intRetries == 0)
        {
            
            informPanel.SetActive(true);
            StartCoroutine(countDown());
        }

        yield return new WaitForSecondsRealtime(2f);
        Time.timeScale = 0;

        
    }
    /// <summary>
    /// ///////////////////////////////////////////////
    /// Reload
    /// </summary>
    public void reload()
    {
        if (intRetries == 0)
        {
            adsManager.DisplayRewarded();
        }
        else {
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex );
            
        }
        //Time.timeScale = 1;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex );

        //StartCoroutine(LoadAsyncOperation());

    }

    IEnumerator LoadAsyncOperation()
    {
        AsyncOperation gameLevel = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        while (gameLevel.progress < 1)
        {
            reloadImage.fillAmount = gameLevel.progress;
            yield return new WaitForEndOfFrame();
        }


    }

    public void calculateRetries()
    {
        intRetries = PlayerPrefs.GetInt("retries");

        lastTime = PlayerPrefs.GetString("lastTime", DateTime.Now.ToString());
        lastDate = DateTime.Parse(lastTime);

        currentDate = DateTime.Now;
        cycle = currentDate.Subtract(lastDate);
        result = cycle.TotalMinutes;

        if (result >= 1440)
        {
            PlayerPrefs.SetInt("retries", 5);
            PlayerPrefs.SetString("lastTime", currentDate.ToString());

        }

        intRetries = PlayerPrefs.GetInt("retries");
        //retry UI
        if (intRetries == 0)
        {
            retryButton.GetComponentInChildren<Text>().text = "GET 5 RETRIES";
        }
        else
        {
            retryButton.GetComponentInChildren<Text>().text = "RETRY";
        }

        retriesText.text = "Retries Left : " + intRetries;
    }
    

    void resetDrag()
    {
        foreach (Transform[] crateArrays in crateSpawner.crates)
        {
            foreach ( Transform crate in crateArrays)
            {
                
                if (crate!=null)
                {
                    crate.GetComponent<Rigidbody>().isKinematic = false;
                    crate.GetComponent<Rigidbody>().drag = 1;
                    crate.GetComponent<Rigidbody>().mass = 0.1f;
                    if (crate.transform.position.z <= mostCurrentCrate)
                    {
                        
                        //crate.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                        //crate.GetComponent<Rigidbody>().velocity = Vector3.zero;
                        //crate.GetComponent<Rigidbody>().isKinematic = true;
                        

                        //crate.GetComponent<Rigidbody>().isKinematic = false;
                        
                        //crate.GetComponent<Rigidbody>().velocity = Vector3.down * 10f;
                        //Vector3 v = new Vector3(0f, crate.GetComponent<Rigidbody>().velocity.y, 0f);


                       
                    }
                    
                    
                }
            }
        }
    }

    

}
