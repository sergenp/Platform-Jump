using System;
using System.Collections;
using UnityEngine;


[System.Serializable]
public class SerializableColor
{
    [SerializeField]
    float[] CubeColorArr = new float[4];

    public Color CubeColor
    {
        get
        {
            return new Color(CubeColorArr[0], CubeColorArr[1], CubeColorArr[2], CubeColorArr[3]);
        }
        set
        {
            CubeColorArr[0] = value.r;
            CubeColorArr[1] = value.g;
            CubeColorArr[2] = value.b;
            CubeColorArr[3] = value.a;
        }
    }
    public SerializableColor(Color color)
    {
        CubeColorArr[0] = color.r;
        CubeColorArr[1] = color.g;
        CubeColorArr[2] = color.b;
        CubeColorArr[3] = color.a;
    }
}


[System.Serializable]
public class PlayerData
{
    public int currentLevel;
    public int goldAmount;
    public SerializableColor cubeColor;
}

public class PlayerDataManager : MonoBehaviour
{

    public Color defaultCubeColor;

    #region Singleton
    public static PlayerDataManager instance;

    public delegate void GoldChanged();
    public GoldChanged goldIncreased;
    public GoldChanged goldDecreased;

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
                cubeColor = new SerializableColor(defaultCubeColor)
            };
            SaveSystem<PlayerData>.SaveData(data, "player");
        }
    }
    #endregion

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
        data.currentLevel += 1;
    }

    public void IncreaseGold(int amount)
    {
        data.goldAmount += amount;
        goldIncreased?.Invoke();
    }

    public bool DecreaseGold(int amount)
    {
        if (data.goldAmount - amount >= 0)
        {
            data.goldAmount -= amount;
            goldDecreased?.Invoke();
            return true;
        }
        else
        {
            return false;
        }
    }

    public Color GetCubeColor()
    {
        return data.cubeColor.CubeColor;
    }

    public void SetCubeColor(Color color)
    {
        data.cubeColor.CubeColor = color;
        SavePlayerData();
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
