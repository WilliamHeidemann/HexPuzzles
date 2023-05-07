using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsInitializer : MonoBehaviour, IUnityAdsInitializationListener
{
    private const string androidGameId = "5270973";
    private const string iOSGameId = "5270972";
    [SerializeField] private bool testMode = true;
    private string _gameId;

    private void Awake()
    {
        InitializeAds();
    }
    
    public void InitializeAds()
    {
    #if UNITY_IOS
            _gameId = iOSGameId;
    #elif UNITY_ANDROID
            _gameId = androidGameId;
    #elif UNITY_EDITOR
            _gameId = androidGameId; //Only for testing the functionality in the Editor
    #endif
        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(_gameId, testMode, this);
        }
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }
}
