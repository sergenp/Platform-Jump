using UnityEngine;
using UnityEngine.Advertisements;

public class InitializeAds : MonoBehaviour, IUnityAdsListener
{

    string gameId = "3960425";
    string rewardedVideo = "rewardedVideo";
    string skippableVideo = "video";
    bool testMode = true;

    // Initialize the Ads listener and service:
    void Start()
    {
        Advertisement.AddListener(this);
        Advertisement.Initialize(gameId, testMode);
    }

    public void ShowAd(string name)
    {
        if (Advertisement.IsReady(name))
        {
            Advertisement.Show(name);
        }
        else
        {
            Debug.Log("Rewarded video is not ready at the moment! Please try again later!");
        }
    }

    public void ShowRewardedVideo()
    {
        ShowAd(rewardedVideo);
    }

    public void ShowInterstellarAd()
    {
        ShowAd(skippableVideo);
    }

    // Implement IUnityAdsListener interface methods:
    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        // Define conditional logic for each ad completion status:
        if (showResult == ShowResult.Finished)
        {
            if (placementId == rewardedVideo)
            {
                GameManager.instance.AddCoinToPlayerWithoutAnim(100);
            }
        }
        else if (showResult == ShowResult.Skipped)
        {
            print("Skipped");
        }
        else if (showResult == ShowResult.Failed)
        {
            Debug.LogWarning("The ad did not finish due to an error.");
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