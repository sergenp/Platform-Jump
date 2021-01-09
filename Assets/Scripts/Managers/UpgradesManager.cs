using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpgradesManager : MonoBehaviour
{
   
    #region Singleton
    public static UpgradesManager instance;
    public void Awake()
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
        LoadStats();
    }

    #endregion
    [Serializable]
    public class PlayerUpgrade
    {
        public UpgradeNames upgradeName;
        public int initialUpgradeCost;
        public int upgradedAmount;
        public int upgradeLimit;
        // upgrade cost increase per upgradedAmount (upgradeCost=upgradeCost + perUpgradeCostIncrease*upgradedAmount)
        public int perUpgradeCostIncrease;
        // initial non-upgraded value 
        public float initialUpgradeValue;
        // explanation of what the upgrade does
        [TextArea(3, 10)]
        public string upgradeExplanation;
        // how much value one upgrade applies
        public float increasePerUpgrade;
    }


    [SerializeField]
    public List<PlayerUpgrade> defaultUpgrades;

    [SerializeField]
    public List<PlayerUpgrade> currentUpgrades = new List<PlayerUpgrade>();


    private Dictionary<UpgradeNames, float> Stats = new Dictionary<UpgradeNames, float>();

    public Dictionary<UpgradeNames, float> ConvertStatsToDictionary()
    {
        foreach (var upgrade in currentUpgrades)
        {
            Stats[upgrade.upgradeName] = upgrade.initialUpgradeValue + (upgrade.upgradedAmount * upgrade.increasePerUpgrade);
        }

        return Stats;
    }

    public void LoadStats()
    {
        currentUpgrades = SaveSystem<List<PlayerUpgrade>>.LoadData("upgrades");

        if (currentUpgrades == null)
        {
            currentUpgrades = new List<PlayerUpgrade>();
            currentUpgrades.AddRange(defaultUpgrades);
            SaveStats();
        }
    }

    public PlayerUpgrade GetPlayerUpgrade(UpgradeNames name) 
    {
        return currentUpgrades.FirstOrDefault(x => x.upgradeName == name);
    }

    public void SaveStats()
    {
        SaveSystem<List<PlayerUpgrade>>.SaveData(currentUpgrades, "upgrades");
    }
}