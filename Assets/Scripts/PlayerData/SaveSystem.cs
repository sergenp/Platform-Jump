using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    public static void SavePlayer(PlayerData player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Path.Combine(Application.persistentDataPath, "player.data");
        using (var stream = new FileStream(path, FileMode.Create))
        {
            formatter.Serialize(stream, player);
        }
    }

    public static PlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + Path.DirectorySeparatorChar + "player.data";        
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                try
                {
                    PlayerData data = formatter.Deserialize(stream) as PlayerData;
                    return data;
                }
                // serialization failed (probably user tampered with the file?)
                catch (SerializationException)
                {
                    return null;
                }
            }
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
}
