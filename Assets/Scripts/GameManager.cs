using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public GameObject player;

    public GameObject floatingTextPrefab;

    public GameObject levelTextMesh;

    public Camera mainCam;

    public static GameManager instance;

    private PlayerData data;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        data = SaveSystem.LoadPlayer();

        var levelGenerator = GetComponent<RandomLevelGenerator>();

        if (data == null)
        {
            data = new PlayerData
            {
                goldAmount = 0,
                currentLevel = 1,
                upgrades = new PlayerUpgrades()
            };
        }
        player.GetComponent<CubeJump>().ApplyUpgrades(data.upgrades);
        levelGenerator.SpawnAmount = data.currentLevel + 5;
        levelTextMesh.GetComponent<TextMeshProUGUI>().text = $"Level {data.currentLevel}";
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
            Vector3 newPost = mainCam.ScreenToWorldPoint(new Vector3(0f, playerScreenPos.y, playerScreenPos.z));
            player.transform.position = new Vector3(newPost.x, player.transform.position.y, player.transform.position.z);
        }
        else if (playerScreenPos.x <= 0f)
        {
            Vector3 newPost = mainCam.ScreenToWorldPoint(new Vector3(Screen.width, playerScreenPos.y, playerScreenPos.z));
            player.transform.position = new Vector3(newPost.x, player.transform.position.y, player.transform.position.z);
        }
    }

    public void PlayerFinishedLevel()
    {
        if (data != null)
        {
            data.currentLevel = Mathf.Min(data.currentLevel + 1, 120);
        }
        SaveSystem.SavePlayer(data);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void AddCoinToPlayerWithoutAnim(int value)
    {
        data.goldAmount += value;
    }

    public void AddCoinToPlayer(int value)
    {
        AudioManager.instance.PlayAudioOneShot("Coin Pickup");
        floatingTextPrefab.GetComponent<TextMesh>().text = $"+{value}";
        Instantiate(floatingTextPrefab, player.transform.position, Quaternion.identity, player.transform);
        data.goldAmount += value;
    }

    public void AddCoinToPlayer(int valuePerCoin, int amount)
    {
        StartCoroutine(AddMultipleCoins(valuePerCoin, amount));
        data.goldAmount += valuePerCoin * amount;
    }

    IEnumerator AddMultipleCoins(int valuePerCoin, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Instantiate(floatingTextPrefab, player.transform.position, Quaternion.identity, player.transform);
            AudioManager.instance.PlayAudioOneShot("Coin Pickup");
            yield return new WaitForSeconds(0.3f);
        }
    }
}
