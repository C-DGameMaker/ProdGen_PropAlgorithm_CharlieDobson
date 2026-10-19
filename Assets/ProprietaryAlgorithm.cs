using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LightTransport;

public class ProprietaryAlgorithm : MonoBehaviour
{
    [Header("Generic cityStuff")]
    public int cityWidth = 10;
    public int cityLength = 10;

    public int buildingSpacingMin = 1;
    public int buildingSpacingMax = 5;
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
        RandomizeBuildingScale();
        RandomColor();
    }

    public void RandomPlacements()
    {
        for(int x = 0; x < cityWidth; x++)
        {
            for(int z = 0; z < cityLength; z++)
            {
                GameObject building = Instantiate(buildingPrefab, transform);
                float posX = x * Random.Range(buildingSpacingMin, buildingSpacingMax+1);
                float posZ = z * Random.Range(buildingSpacingMin, buildingSpacingMax + 1);

                building.transform.localPosition = new Vector3(posX, 0, posZ);
                allBuildings.Add(building);
                
            }
        }
    }

    public void RandomizeBuildingScale()
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

    public void RandomColor()
    {
        foreach(GameObject building in allBuildings)
        {
            Renderer render;

            render = building.GetComponent<Renderer>();

            render.material.color = Random.ColorHSV(0f, 1f, 0.5f, 1f);
        }
    }
}


