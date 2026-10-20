using NUnit.Framework;
using System.Collections;
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

    float cityXValue;
    float cityZValue;



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
        BuildingPlacement();
        BuildingColor();
    }


    public void BuildingPlacement()
    {
        cityXValue = 0f;

        for (int x = 0; x < cityWidth; x++)
        {
            cityZValue = 0f;

            for (int z = 0; z < cityLength; z++)
            {
                GameObject building = Instantiate(buildingPrefab, transform);

                BuildingScales(building);
                PlacingBuilding(building);

                allBuildings.Add(building);

                cityZValue += maxBuildingLength + Random.Range(buildingSpacingMin, buildingSpacingMax + 1);
            }

            cityXValue += maxBuildingWidth +
                Random.Range(buildingSpacingMin, buildingSpacingMax + 1);
        }
    }

    public void PlacingBuilding(GameObject building)
    {
        float buildingPlacementWidth = Random.Range(minBuildingWidth, maxBuildingWidth + 1);
        float buildingPlacementLength = Random.Range( minBuildingLength, maxBuildingLength + 1);

        building.transform.localPosition = new Vector3(cityXValue + buildingPlacementWidth / 2f, 0 , cityZValue + buildingPlacementLength / 2f);
    }


    // scales building based off of a set of rules
    public void BuildingScales(GameObject building)
    {
        Vector3 heightScale = building.transform.localScale;
        heightScale.y = Random.Range(minBuildingHeight, maxBuildingHeight);
        heightScale.x = Random.Range(minBuildingWidth, maxBuildingWidth);
        heightScale.z = Random.Range(minBuildingLength, maxBuildingLength);
        building.transform.localScale = heightScale;

        
    }

    public void BuildingColor()
    {
        foreach(GameObject building in allBuildings)
        {
            Renderer render;

            render = building.GetComponent<Renderer>();

            render.material.color = Random.ColorHSV(0f, 1f, 0.5f, 1f);
        }
    }
}


