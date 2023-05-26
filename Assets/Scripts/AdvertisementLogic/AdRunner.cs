using UnityEngine;
using UnityEngine.Advertisements;

namespace AdvertisementLogic
{
    public class AdRunner : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
    {
        private const string AndroidAdUnitId = "Interstitial_Android";
        private const string iOSAdUnitId = "Interstitial_iOS";
        private string _adUnitId;
    
        public static AdRunner instance;
        [SerializeField] private bool shouldRunAds;
        [SerializeField] private float secondsBeforeAdCanRunAgain;
        private float _timeOfLastAd;

        void Awake()
        {
            // Get the Ad Unit ID for the current platform:
            _adUnitId = Application.platform == RuntimePlatform.IPhonePlayer
                ? iOSAdUnitId
                : AndroidAdUnitId;
            instance = this;
        }

        private void Update()
        {
            // if (Input.GetKeyDown(KeyCode.L)) LoadAd();
            // if (Input.GetKeyDown(KeyCode.S)) ShowAd();
        }

        public void RunAd()
        {
            if (!shouldRunAds) return;
            if (_timeOfLastAd + secondsBeforeAdCanRunAgain > Time.time) return;
            Advertisement.Load(_adUnitId, this);
        }

        public void OnUnityAdsAdLoaded(string adUnitId)
        {
            _timeOfLastAd = Time.time;
            Advertisement.Show(adUnitId, this);
        }
 
        public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
        {
            Debug.Log($"Error loading Ad Unit: {adUnitId} - {error.ToString()} - {message}");
            // Optionally execute code if the Ad Unit fails to load, such as attempting to try again.
        }
 
        public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
        {
            Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
            // Optionally execute code if the Ad Unit fails to show, such as loading another ad.
        }
 
        public void OnUnityAdsShowStart(string adUnitId) { }
        public void OnUnityAdsShowClick(string adUnitId) { }
        public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState) { }
    }
}