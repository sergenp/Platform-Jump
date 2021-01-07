using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WatchAdButton : MonoBehaviour
{
    public Button watchAdButton;

    private void Start()
    {
        StartCoroutine(adReadyChecker());
        if (watchAdButton == null)
        {
            watchAdButton = GetComponent<Button>();
        }
        watchAdButton.onClick.RemoveAllListeners();
        watchAdButton.onClick.AddListener(() => AdManager.instance.ShowRewardedVideo());
    }

    
    IEnumerator adReadyChecker()
    {
        while (true)
        {
            if (AdManager.instance.isRewardedVideoReady())
            {
                watchAdButton.interactable = true;
            }
            else
            {
                watchAdButton.interactable = false;
            }
            yield return new WaitForSeconds(5f);
        }
    }

}
