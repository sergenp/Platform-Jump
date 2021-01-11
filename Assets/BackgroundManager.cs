using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{

    public CubeJump cubeJump;

    [Header("Background Particle Effects")]
    public List<GameObject> backgroundEffects;

    void Start()
    {
        StartCoroutine(createEffect());
    }

    private IEnumerator createEffect()
    {
        while (true) 
        {
            if (backgroundEffects != null)
            {
                int chance = UnityEngine.Random.Range(0, 100);
                if (chance < 15)
                {
                    int randomEffect = UnityEngine.Random.Range(0, backgroundEffects.Count);
                    GameObject created = Instantiate(backgroundEffects[randomEffect], cubeJump.transform.position, Quaternion.identity);
                    Destroy(created, 8f);
                }
            }
            yield return new WaitForSeconds(10f);
        }
    }
}
