using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyTrackerUI : MonoBehaviour
{
    private TextMeshProUGUI textMesh;

    private void OnDisable()
    {
        PlayerDataManager.instance.goldDecreased -= GoldChanged;
        PlayerDataManager.instance.goldIncreased -= GoldChanged;
    }

    private void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        GoldChanged();
        PlayerDataManager.instance.goldDecreased += GoldChanged;
        PlayerDataManager.instance.goldIncreased += GoldChanged;
    }
    void GoldChanged()
    {
        textMesh.text = $"Current $:{PlayerDataManager.instance.GetCurrentGold()}";
    }
}
