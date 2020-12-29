using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{

    private GameObject player;

    private Camera cam;


    private void Start()
    {
        player = FindObjectOfType<CubeJump>().gameObject;
        cam = Camera.main;
    }

    private void Update()
    {
        Vector3 playerScreenPos = cam.WorldToScreenPoint(player.transform.position);

        if (playerScreenPos.x >= Screen.width)
        {
            Vector3 newPost = cam.ScreenToWorldPoint(new Vector3(0f, playerScreenPos.y, playerScreenPos.z));
            player.transform.position = new Vector3(newPost.x, newPost.y, newPost.z);
        } else if (playerScreenPos.x <= 0f)
        {
            Vector3 newPost = cam.ScreenToWorldPoint(new Vector3(Screen.width, playerScreenPos.y, playerScreenPos.z));
            player.transform.position = new Vector3(newPost.x, newPost.y, newPost.z);
        }

    }
}
