using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradesButtonUI : MonoBehaviour
{

    public UpgradeNames upgradeName;
    public string upgradeDisplayName;
    public TextMeshProUGUI upgradedLevelText;
    [Header("Panel for explaining the upgrade")]
    public GameObject UpgradeExplainerPanel;
    public TextMeshProUGUI Header;
    public TextMeshProUGUI ExplainUpgrade;
    public Button confirmButton;
    [Header("Panel for if upgrade fails")]
    public GameObject UpgradeFailedPanel;

    private Button thisButton;
    private UpgradesManager.PlayerUpgrade data;
    

    private void Start()
    {
        thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(() => OpenUpgradeExplainer());
        data = UpgradesManager.instance.GetPlayerUpgrade(upgradeName);
        if (data.upgradedAmount < data.upgradeLimit)
        {
            upgradedLevelText.text = $"Lvl {data.upgradedAmount+1}";
        }
        else
        {
            upgradedLevelText.text = "Maxed";
            thisButton.gameObject.SetActive(false);
        }
    }


    public void OpenUpgradeExplainer()
    {
        Header.text = upgradeDisplayName.ToString();
        int playerGold = PlayerDataManager.instance.GetCurrentGold();
        float upgradeCost = data.initialUpgradeCost + (data.perUpgradeCostIncrease * data.upgradedAmount);
        float currentUpgradedValue = data.initialUpgradeValue + (data.upgradedAmount * data.increasePerUpgrade);
        float nextUpgradedValue = data.initialUpgradeValue + ((data.upgradedAmount+1) * data.increasePerUpgrade);
        ExplainUpgrade.text = $"{data.upgradeExplanation}\n\nCurrent: {currentUpgradedValue}</b>\nNext: {nextUpgradedValue}</b>\nThis Upgrade Costs: {upgradeCost} $\n" +
            $"You have: {playerGold} $";
        confirmButton.onClick.RemoveAllListeners();
        confirmButton.onClick.AddListener(() => UpgradeConfirmed());
        UpgradeExplainerPanel.SetActive(true);
    }

    public void UpgradeConfirmed()
    {
        bool successfull = PlayerDataManager.instance.DecreaseGold(data.initialUpgradeCost + (data.perUpgradeCostIncrease * data.upgradedAmount));
        
        if (successfull)
        {
            data.upgradedAmount += 1;
            if (data.upgradedAmount < data.upgradeLimit)
            {
                upgradedLevelText.text = $"Lvl {data.upgradedAmount + 1}";
            }
            else
            {
                upgradedLevelText.text = "Max";
                thisButton.gameObject.SetActive(false);
            }
            UpgradesManager.instance.SaveStats();
            PlayerDataManager.instance.SavePlayerData();
            UpgradeExplainerPanel.SetActive(false);
        }
        else
        {
            UpgradeExplainerPanel.SetActive(false);
            UpgradeFailedPanel.SetActive(true);
        }
    }
}
