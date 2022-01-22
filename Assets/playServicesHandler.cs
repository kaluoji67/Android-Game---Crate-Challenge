using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;
using System;
using GooglePlayGames.BasicApi;

public class playServicesHandler : MonoBehaviour
{
    public bool isConnectedToGooglePlayServices;


    public static playServicesHandler _instance;
    void Awake()
    {
        //if we don't have an [_instance] set yet
        if (!_instance)
            _instance = this;
        //otherwise, if we do, kill this thing
        else
            Destroy(this.gameObject);


        //DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();

        SignInToGooglePlayServices();
    }

    private void SignInToGooglePlayServices()
    {
        PlayGamesPlatform.Instance.Authenticate(GooglePlayGames.BasicApi.SignInInteractivity.CanPromptOnce, (result) => {
            switch (result)
            {
                case SignInStatus.Success:
                    isConnectedToGooglePlayServices = true;
                    break;

                default:
                    isConnectedToGooglePlayServices = false;
                    break;
            }
        
        });

    }

    public void ShowLeaderboard()
    {
        if (Social.localUser.authenticated)
        {
            PlayGamesPlatform.Instance.ShowLeaderboardUI(GPGSIds.leaderboard_leaderboard);
        }
        else
        {
            SignInToGooglePlayServices();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
