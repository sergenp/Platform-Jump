using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


[System.Serializable]
public class PlayerData
{
    public int currentLevel;
    public int goldAmount;
}

public class GameManager : MonoBehaviour
{

    [Header("Player")]
    public GameObject player;

    [Header("Text Prefabs")]
    public GameObject floatingTextPrefab;

    public TextMeshProUGUI levelTextMesh;
    
    public TextMeshProUGUI goldTextMesh;

    [Header("Camera")]
    public Camera mainCam;

    [Header("Level Finish UI")]
    public GameObject levelFinishPanel;
    public GameObject youHaveDiedPanel;

    public static GameManager instance;

    private PlayerData data;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        Application.targetFrameRate = 90;
        data = SaveSystem<PlayerData>.LoadData("player");

        var levelGenerator = GetComponent<RandomLevelGenerator>();

        if (data == null)
        {
            data = new PlayerData
            {
                goldAmount = 0,
                currentLevel = 1,
            };
        }
        player.GetComponent<CubeJump>().playerDied += PlayerDied;
        levelGenerator.SpawnAmount = data.currentLevel + 5;
        levelTextMesh.text = $"Level {data.currentLevel}";
        goldTextMesh.text = $"{data.goldAmount}$";
        levelGenerator.GenerateLevel();
    }

    private void Start()
    {
        if (mainCam == null)
            mainCam = Camera.main;
    }

    public Transform GetPlayerTransform()
    {
        return player.transform;
    }

    private void Update()
    {
        Vector3 playerScreenPos = mainCam.WorldToScreenPoint(player.transform.position);

        if (playerScreenPos.x >= Screen.width)
        {
            Vector3 newPos = mainCam.ScreenToWorldPoint(new Vector3(0f, playerScreenPos.y, playerScreenPos.z));
            player.transform.position = new Vector3(newPos.x, player.transform.position.y, player.transform.position.z);
        }
        else if (playerScreenPos.x <= 0f)
        {
            Vector3 newPos = mainCam.ScreenToWorldPoint(new Vector3(Screen.width, playerScreenPos.y, playerScreenPos.z));
            player.transform.position = new Vector3(newPos.x, player.transform.position.y, player.transform.position.z);
        }
    }

    public void PlayerFinishedLevel()
    {
        if (data != null)
        {
            data.currentLevel = Mathf.Min(data.currentLevel + 1, 120);
        }
        player.gameObject.SetActive(false);
        SaveSystem<PlayerData>.SaveData(data, "player");
        levelFinishPanel.SetActive(true);
    }

    public void PlayerDied()
    {
        youHaveDiedPanel.SetActive(true);
    }

    public void LoadCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void AddCoinToPlayerWithoutAnim(int value)
    {
        data.goldAmount += value;
    }

    public void AddCoinToPlayer(int value)
    {
        StartCoroutine(AddMultipleCoins(value, 1));
    }

    public void AddCoinToPlayer(int valuePerCoin, int amount)
    {
        StartCoroutine(AddMultipleCoins(valuePerCoin, amount));
    }

    IEnumerator AddMultipleCoins(int valuePerCoin, int amount)
    {
        data.goldAmount += valuePerCoin * amount;
        goldTextMesh.text = $"{data.goldAmount}$";
        for (int i = 0; i < amount; i++)
        {
            CreateFloatingText($"+{valuePerCoin}", "Coin Pickup");
            yield return new WaitForSeconds(0.3f);
        }
    }

    public void CreateFloatingText(string text, string soundClipToPlay)
    {
        floatingTextPrefab.GetComponent<TextMesh>().text = text;
        Instantiate(floatingTextPrefab, player.transform.position, Quaternion.identity, player.transform);
        AudioManager.instance.PlayAudioOneShot(soundClipToPlay);
    }

    public void CreateFloatingText(string text)
    {
        floatingTextPrefab.GetComponent<TextMesh>().text = text;
        Instantiate(floatingTextPrefab, player.transform.position, Quaternion.identity, player.transform);
    }
}
