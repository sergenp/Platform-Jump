using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour, IUnityAdsListener
{
    #region Singleton
    public static AdManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    string gameId = "3960425";
    readonly string rewardedVideo = "rewardedVideo";
    readonly string skippableVideo = "video";
    bool testMode = false;

    // show ad after x times of calling the skippable vid function
    int showAdAfter = 4;
    int showAdCounter = 0;

    // Initialize the Ads listener and service:
    void Start()
    {
        Advertisement.AddListener(this);
        Advertisement.Initialize(gameId, testMode);
    }

    bool isAdReady(string name)
    {
        return Advertisement.IsReady(name);
    }

    public bool isRewardedVideoReady()
    {
        return isAdReady(rewardedVideo);
    }
    public bool isSkippableVideoReady()
    {
        return isAdReady(skippableVideo);
    }

    public bool ShowAd(string name)
    {
        if (isAdReady(name))
        {
            Advertisement.Show(name);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ShowRewardedVideo()
    {
        ShowAd(rewardedVideo);
    }

    public void ShowInterstellarAd()
    {
        showAdCounter++;
        if (showAdCounter == showAdAfter)
        {
            ShowAd(skippableVideo);
            showAdCounter = 0;
        }
    }

    // Implement IUnityAdsListener interface methods:
    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        // Define conditional logic for each ad completion status:
        if (showResult == ShowResult.Finished)
        {
            if (placementId == rewardedVideo)
            {
                AudioManager.instance.PlayAudioOneShot("Coin Pickup");
                PlayerDataManager.instance.IncreaseGold(80);
            }
        }
        else if (showResult == ShowResult.Skipped)
        {
            //print("Skipped");
        }
        else if (showResult == ShowResult.Failed)
        {
            
        }
    }

    public void OnUnityAdsReady(string placementId)
    {

    }

    public void OnUnityAdsDidError(string message)
    {
        // Log the error.
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        // Optional actions to take when the end-users triggers an ad.
    }

    // When the object that subscribes to ad events is destroyed, remove the listener:
    public void OnDestroy()
    {
        Advertisement.RemoveListener(this);
    }
}