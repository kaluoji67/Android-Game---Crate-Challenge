using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;
using UnityEngine.SceneManagement;

public class AdsManager : MonoBehaviour
{
    //private static AdsManager _instance;
    
    private string APPID = "ca-app-pub-3744673264829029~2677389480";

    private BannerView bannerAd;
    private InterstitialAd interstitialAd;
    private RewardedInterstitialAd rewardedInterstitialAd;
    private RewardedAd rewardedAd;

    public gameControllerScript gameController;

    public GameObject panelInf;

    private void Awake()
    {
        /*
        if (!_instance)
            _instance = this;
        //otherwise, if we do, kill this thing
        else
            Destroy(this.gameObject);


        DontDestroyOnLoad(this.gameObject);
        */
    }

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the Google Mobile Ads SDK.
        //MobileAds.Initialize(initStatus => { });

        //for testing
        
        /*
        List<string> deviceIds = new List<string>();
        deviceIds.Add("2077ef9a63d2b398840261c8221a0c9b");
        RequestConfiguration requestConfiguration = new RequestConfiguration
            .Builder()
            .SetTestDeviceIds(deviceIds)
            .build();
        MobileAds.SetRequestConfiguration(requestConfiguration);
        */

        //RequestBanner();
        RequestInterstitial();
        RequestRewarded();
        RequestRewardedInterstitial();
    }

    // /////////////////////////////////////////////////////////////Request and display banner
    void RequestBanner()
    {
        
        string bannerID = "ca-app-pub-3940256099942544/6300978111";
        bannerAd = new BannerView(bannerID, AdSize.SmartBanner, AdPosition.Top);

        // Create an empty ad request for Real.
        AdRequest request = new AdRequest.Builder().Build();

        //for testing
        

        // Load the banner with the request.
        bannerAd.LoadAd(request);
    }

    public void DisplayBanner()
    {
        bannerAd.Show();
    }

    ////////////////////////////////////////////////////////////// Request and interstitial ad
    void RequestInterstitial()
    {
        //real
        string interstitialID = "ca-app-pub-3744673264829029/3933549269";

       

        interstitialAd = new InterstitialAd(interstitialID);

        // Called when an ad request has successfully loaded.
        this.interstitialAd.OnAdLoaded += HandleOnAdLoaded;
        // Called when an ad request failed to load.
        this.interstitialAd.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        // Called when an ad is shown.
        this.interstitialAd.OnAdOpening += HandleOnAdOpened;
        // Called when the ad is closed.
        this.interstitialAd.OnAdClosed += HandleOnAdClosed;
        
        // Create an empty ad request for Real.
        AdRequest request = new AdRequest.Builder().Build();

        interstitialAd.LoadAd(request);
    }
    #region //interstitial events
    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        //DisplayInterstitial();
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        //MonoBehaviour.print("HandleFailedToReceiveAd event received with message: "
        //                   + args.Message);
        RequestInterstitial();
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        //MonoBehaviour.print("HandleAdOpened event received");
        AudioListener.volume = 0f;
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        AudioListener.volume = 0.5f;
        gameController.goToMainMenu();
    }

   
    #endregion
    public void DisplayInterstitial()
    {
        if (this.interstitialAd.IsLoaded())
        {
            this.interstitialAd.Show();
        }
    }

    /////////////////////////////////////////////////////////////////// Request and rewarded Ad
    void RequestRewarded()
    {
        //real
        string rewardedID = "ca-app-pub-3744673264829029/2947770652";

      
        rewardedAd = new RewardedAd(rewardedID);



        // Called when an ad request has successfully loaded.
        this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        // Called when an ad request failed to load.
        this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        // Called when an ad is shown.
        this.rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        // Called when an ad request failed to show.
        this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        // Called when the user should be rewarded for interacting with the ad.
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        // Called when the ad is closed.
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;



        // Create an empty ad request for Real.
        AdRequest request = new AdRequest.Builder().Build();

        //for testing


        // Load the banner with the request.
        rewardedAd.LoadAd(request);
    }
    public void DisplayRewarded()
    {
        if (rewardedAd.IsLoaded())
        {
           rewardedAd.Show();
        }
        else
            Debug.Log("rewardAd not ready");
    }
    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        //MonoBehaviour.print("HandleRewardedAdLoaded event received");
    }

    public void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        //RequestRewarded();
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        //MonoBehaviour.print("HandleRewardedAdOpening event received");
        AudioListener.volume = 0f;
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        //RequestRewarded();
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        Time.timeScale = 1;
        AudioListener.volume = 0.5f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
       
        string type = args.Type;
        double amount = args.Amount;
        // PlayerPrefs.SetInt(args.Type, (int)args.Amount);

        PlayerPrefs.SetInt("retries", 5);
        gameController.calculateRetries();
    }


    //////////////////////////////////////////////////////////// Request rewardedInterstitialAd
    void RequestRewardedInterstitial()
    {
        //real
        string rewardedInterstitialID = "ca-app-pub-3744673264829029/9055875826";

        

        // Create an empty ad request for Real.
        AdRequest request = new AdRequest.Builder().Build();

        //for testing


        // Load the banner with the request.
        RewardedInterstitialAd.LoadAd(rewardedInterstitialID, request, adLoadCallback); ;
    }

    private void adLoadCallback(RewardedInterstitialAd arg1, AdFailedToLoadEventArgs arg2)
    {
        if (arg2 == null)
        {
            rewardedInterstitialAd = arg1;

            rewardedInterstitialAd.OnAdFailedToPresentFullScreenContent += HandleAdFailedToPresent;
            rewardedInterstitialAd.OnAdDidPresentFullScreenContent += HandleAdDidPresent;
            rewardedInterstitialAd.OnAdDidDismissFullScreenContent += HandleAdDidDismiss;
            rewardedInterstitialAd.OnPaidEvent += HandlePaidEvent;
        }
    }

    
    private void HandleAdFailedToPresent(object sender, AdErrorEventArgs args)
    {
        //MonoBehavior.print("Rewarded interstitial ad has failed to present.");
        RequestRewardedInterstitial();
    }

    private void HandleAdDidPresent(object sender, EventArgs args)
    {
        AudioListener.volume = 0.5f;
        panelInf.SetActive(false);
        //MonoBehavior.print("Rewarded interstitial ad has presented.");
    }

    private void HandleAdDidDismiss(object sender, EventArgs args)
    {
        //MonoBehavior.print("Rewarded interstitial ad has dismissed presentation.");
        AudioListener.volume = 0.5f;
    }

    private void HandlePaidEvent(object sender, AdValueEventArgs args)
    {
        //MonoBehaviour.print(
        //    "Rewarded interstitial ad has received a paid event.");
    }

    public void DisplayRewardedInterstitialAd()
    {
        if (rewardedInterstitialAd != null)
        {
            rewardedInterstitialAd.Show(userEarnedRewardCallback);
            RequestRewardedInterstitial();
            AudioListener.volume = 0f;
        }
    }

    private void userEarnedRewardCallback(Reward reward)
    {
        string type = reward.Type;
        double amount = reward.Amount;
        // PlayerPrefs.SetInt(args.Type, (int)args.Amount);
        PlayerPrefs.SetInt("retries",5);
        gameController.calculateRetries();

        if (!gameController.gameOverBool) {
            gameController.reload();
        }
        
    }

    private void OnDestroy()
    {
        interstitialAd.Destroy();
    }


}//class
