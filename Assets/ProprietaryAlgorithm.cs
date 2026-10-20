using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LightTransport;

public class ProprietaryAlgorithm : MonoBehaviour
{
    [Header("Building Prefabs")]
    public GameObject buildingPrefab;
    public List<GameObject> allBuildings = new List<GameObject>();

    [Header("Generic City Stuff")]
    public int cityWidth = 10;
    public int cityLength = 10;

    public int buildingSpacingMin = 1;
    public int buildingSpacingMax = 5;
    public int minBuildingHeight = 10;
    public int maxBuildingHeight = 50;

    public int minBuildingWidth = 1;
    public int maxBuildingWidth = 5;
    public int minBuildingLength = 1;
    public int maxBuildingLength = 5;
    
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
        BuildingScales();
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
                DistanceBuildings();

                building.transform.localPosition = new Vector3(posX, 0, posZ);
                allBuildings.Add(building);
                
            }
        }
    }

    private void DistanceBuildings()
    {
        if (allBuildings.Count == 0) return;
        foreach(GameObject building in allBuildings)
        {

        }

    }

    // scales all building based off of a set of rules
    public void BuildingScales()
    {
        foreach(GameObject building in allBuildings)
        {
            Vector3 heightScale = building.transform.localScale;
            heightScale.y = HeightScale(heightScale.y, heightScale.z);
            heightScale.x = WidthScale(heightScale.x, heightScale.z);
            heightScale.z = LengthScale(heightScale.z, heightScale.x);
            building.transform.localScale = heightScale;
        }
    }

    private int LengthScale(float x, float z)
    {
        int buildingLength = 0;

        float noise = Mathf.PerlinNoise(x * 0.01f, z * 0.01f);

        buildingLength = Mathf.Clamp(Mathf.RoundToInt(noise * maxBuildingLength) + Random.Range(minBuildingLength, maxBuildingLength), minBuildingLength, maxBuildingLength);
        return buildingLength;
    }
    private int WidthScale(float x, float z)
    {
        int buildingWidth = 0;

        float noise = Mathf.PerlinNoise(x * 0.01f, z * 0.01f);

        buildingWidth = Mathf.Clamp(Mathf.RoundToInt(noise * maxBuildingWidth) + Random.Range(minBuildingWidth, maxBuildingWidth), minBuildingWidth, maxBuildingWidth);
        return buildingWidth;
    }

    private int HeightScale(float h, float z)
    {
        int buildingHeight = 0;

        float noise = Mathf.PerlinNoise(h * 0.01f, z * 0.01f);

        buildingHeight = Mathf.Clamp(Mathf.RoundToInt(noise * maxBuildingHeight) + Random.Range(minBuildingHeight, maxBuildingHeight), minBuildingHeight, maxBuildingHeight);
        float centerBias = 1f - (Vector2.Distance(new Vector2(h, z), new Vector2(cityWidth / 2f, cityLength / 2f)) / (cityWidth / 2f));
        buildingHeight = Mathf.Clamp(Mathf.RoundToInt(buildingHeight * (1f + centerBias * 0.5f)), minBuildingHeight, maxBuildingHeight);
        return buildingHeight;
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


