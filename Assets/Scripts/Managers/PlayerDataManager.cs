using System;
using System.Collections;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager instance;

    PlayerData data;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        data = SaveSystem<PlayerData>.LoadData("player");
        if (data == null)
        {
            data = new PlayerData
            {
                goldAmount = 0,
                currentLevel = 1,
            };
            SaveSystem<PlayerData>.SaveData(data, "player");
        }
    }

    public int GetCurrentGold()
    {
        return data.goldAmount;
    }

    public int GetCurrentLevel()
    {
        return data.currentLevel;
    }

    public void IncreaseCurrentLevel()
    {
        data.currentLevel = Math.Min(data.currentLevel + 1, 50);
    }

    public void IncreaseGold(int amount)
    {
        data.goldAmount += amount;
    }

    public bool DecreaseGold(int amount)
    {
        if (data.goldAmount - amount >= 0)
        {
            data.goldAmount -= amount;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SavePlayerData()
    {
        SaveSystem<PlayerData>.SaveData(data, "player");
    }

    public void LoadPlayerData()
    {
        data = SaveSystem<PlayerData>.LoadData("player");
    }

    // save player data just in case
    private void OnApplicationPause(bool pause)
    {
        SavePlayerData();
    }

    private void OnApplicationQuit()
    {
        SavePlayerData();
    }
}
