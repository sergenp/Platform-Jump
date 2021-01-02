using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{

    public GameObject floatingTextPrefab;

    public Camera mainCam;

    public static GameManagerScript instance;

    private GameObject player;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        player = FindObjectOfType<CubeJump>().gameObject;

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
        } else if (playerScreenPos.x <= 0f)
        {
            Vector3 newPost = mainCam.ScreenToWorldPoint(new Vector3(Screen.width, playerScreenPos.y, playerScreenPos.z));
            player.transform.position = new Vector3(newPost.x, player.transform.position.y, player.transform.position.z);
        }
    }

    public void PlayerFinishedLevel()
    {
       // print("Finished");
    }

    public void AddCoinToPlayer(int value)
    {
        AudioManager.instance.PlayAudioOneShot("Coin Pickup");
        floatingTextPrefab.GetComponent<TextMesh>().text = $"+{value}";
        Instantiate(floatingTextPrefab, player.transform.position, Quaternion.identity, player.transform);
       // print($"Player gained +{value} coins");
    }

    public void AddCoinToPlayer(int valuePerCoin, int amount)
    {
        StartCoroutine(AddMultipleCoins(valuePerCoin, amount));
    }

    IEnumerator AddMultipleCoins(int valuePerCoin, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Instantiate(floatingTextPrefab, player.transform.position, Quaternion.identity, player.transform);
            AudioManager.instance.PlayAudioOneShot("Coin Pickup");
            yield return new WaitForSeconds(0.3f);
        }
       // print($"Player gained +{valuePerCoin}*{amount} coins");
    }
}
