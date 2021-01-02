using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ProceduralLevelGenerator : MonoBehaviour
{

    [Serializable]
    public class SpawnPlatform
    {
        public Vector3[] rotations;
        public GameObject prefab;
    }

    public Vector3 startPos;
    public GameObject startObject;

    public GameObject finishObject;

    public SpawnPlatform[] platformsToSpawn;

    public float YDistanceBetweenObjects;

    public int SpawnAmount = 15;

    float YDistance;

    float rightXLimit;
    float leftXLimit;

    void Start()
    {
        Instantiate(startObject, startPos, Quaternion.identity);
        YDistance += startPos.y + YDistanceBetweenObjects;
        rightXLimit = Camera.main.ScreenToWorldPoint(new Vector3(0f, 10, Camera.main.transform.position.z)).x;
        leftXLimit = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 10, Camera.main.transform.position.z)).x;
        GenerateLevel();
    }

    private void GenerateLevel()
    {
        GameObject previousObj = startObject;
        for (int i = 0; i < SpawnAmount; i++)
        {
            SpawnPlatform objToSpawn = platformsToSpawn[UnityEngine.Random.Range(0, platformsToSpawn.Length)];
            float xPos = UnityEngine.Random.Range(leftXLimit / 2, rightXLimit / 2);
            float maxYPosInChildren = getMaxPosYInChildren(previousObj);
            if (maxYPosInChildren != float.MinValue)
            {
                YDistance = YDistanceBetweenObjects + maxYPosInChildren;
            }
            else
            {
                // if there is no children in the previous object, just add the distance and the object's y position
                YDistance = YDistanceBetweenObjects + previousObj.transform.position.y;
            }

            if (objToSpawn.rotations.Length > 0)
            {
                previousObj = Instantiate(objToSpawn.prefab, new Vector3(xPos, YDistance, startPos.z),
                Quaternion.Euler(objToSpawn.rotations[UnityEngine.Random.Range(0, objToSpawn.rotations.Length)]));
            }
            else
            {
                previousObj = Instantiate(objToSpawn.prefab, new Vector3(xPos, YDistance, startPos.z), Quaternion.identity);
            }
        }
        // add the last finishing platform after generating the level
        Instantiate(finishObject, new Vector3(0f, previousObj.transform.position.y + YDistanceBetweenObjects*2, startPos.z), Quaternion.identity);
    
    }


    /// <summary>
    /// Takes a gmObj, searches through its children and returns the maximum y position of its children
    /// </summary>
    /// <param name="gmObj">Unity GameObject</param>
    /// <returns>Maximum Y Position of the object's children, otherwise return float.MinValue</returns>
    public float getMaxPosYInChildren(GameObject gmObj) 
    {
        float maxYPosInChildren = float.MinValue;
        foreach (var tr in gmObj.GetComponentsInChildren<Transform>())
        {
            if (tr.gameObject != gmObj && maxYPosInChildren < tr.position.y)
            {
                maxYPosInChildren = tr.position.y;
            }
        }
        return maxYPosInChildren;
    }
}
