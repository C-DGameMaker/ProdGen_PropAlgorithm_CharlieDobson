using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ProprietaryAlgorithm : MonoBehaviour
{
    public int cityWidth = 10;
    public int cityLength = 10;

    public int buildingSpacing = 5;
    public int minBuildingHeight = 10;
    public int maxBuildingHeight = 50;
    [Header("Building Prefabs")]
    public GameObject buildingPrefab;
    public List<GameObject> allBuildings = new List<GameObject>();
    

    void Start()
    {
        Regenerate();
    }

    public void DeleteAll()
    {
        if (allBuildings.Count == 0) return;

        foreach(GameObject building in allBuildings)
        {
            Destroy(building);
        }

        allBuildings.Clear();

    }

    public void Regenerate()
    {
        DeleteAll();
        RandomPlacements();
        RandomizeScale();
    }

    public void RandomPlacements()
    {
        for(int x = 0; x < cityWidth; x++)
        {
            for(int z = 0; z < cityLength; z++)
            {
                GameObject building = Instantiate(buildingPrefab, transform);
                float posX = x * buildingSpacing;
                float posZ = z * buildingSpacing;

                building.transform.localPosition = new Vector3(posX, 0, posZ);
                allBuildings.Add(building);
                
            }
        }
    }

    public void RandomizeScale()
    {
        foreach(GameObject building in allBuildings)
        {
            float buildingHeight = Random.Range(minBuildingHeight, maxBuildingHeight + 1);
            float buildingWidth = Random.Range(1, 5);
            float buidlingLength = Random.Range(1, 5);
            Vector3 heightScale = building.transform.localScale;
            heightScale.y = buildingHeight;
            heightScale.x = buildingWidth;
            heightScale.z = buidlingLength;
            building.transform.localScale = heightScale;
        }
    }
}


