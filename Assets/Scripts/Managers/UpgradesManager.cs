using System;
using System.Collections.Generic;
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
    }
    #endregion

    [Serializable]
    public class PlayerUpgrades
    {
        public UpgradeNames upgradeName;
        public int upgradeCost;
        public int upgradedAmount;
        public int upgradeLimit;
        public float perUpgradeCostIncrease;
        public float upgradeValue;
    }

    [SerializeField]
    public List<PlayerUpgrades> defaultUpgrades;

    [SerializeField]
    public List<PlayerUpgrades> currentUpgrades;


    private Dictionary<UpgradeNames, float> Stats = new Dictionary<UpgradeNames, float>();

    public Dictionary<UpgradeNames, float> ConvertStatsToDictionary()
    {
        LoadStats();

        foreach (var upgrade in currentUpgrades)
        {
            Stats[upgrade.upgradeName] = upgrade.upgradeValue;
        }

        return Stats;
    }

    public void LoadStats()
    {
        currentUpgrades = SaveSystem<List<PlayerUpgrades>>.LoadData("upgrades");

        if (currentUpgrades == null)
        {
            currentUpgrades = new List<PlayerUpgrades>();
            currentUpgrades.AddRange(defaultUpgrades);
        }
    }

    public void UpgradeStat(UpgradeNames name, float value)
    {
        Stats[name] = value;
    }

    public void SaveStats()
    {
        SaveSystem<List<PlayerUpgrades>>.SaveData(currentUpgrades, "upgrades");
    }

}