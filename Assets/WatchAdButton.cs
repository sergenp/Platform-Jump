using UnityEngine;
using UnityEngine.UI;

public class WatchAdButton : MonoBehaviour
{
    public Button watchAdButton;

    private void Start()
    {
        if (watchAdButton == null)
        {
            watchAdButton = GetComponent<Button>();
        }
        watchAdButton.onClick.RemoveAllListeners();
        watchAdButton.onClick.AddListener(() => AdManager.instance.ShowRewardedVideo());
    }

    private void Update()
    {
        watchAdButton.interactable = AdManager.instance.isRewardedVideoReady();
    }
}
