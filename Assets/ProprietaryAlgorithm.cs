using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LightTransport;
using UnityEngine.UIElements;

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

    public Color shortBuildingColor;
    public Color medBuildingColor;
    public Color tallBuildingColor;

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
        BuildingRelativeAttributes();
        BuildingRelativePlacement();
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
                BuildingColor(building);

                allBuildings.Add(building);
                building.tag = "Building";

                cityZValue += maxBuildingLength + Random.Range(buildingSpacingMin, buildingSpacingMax + 1);
            }

            cityXValue += maxBuildingWidth + Random.Range(buildingSpacingMin, buildingSpacingMax + 1);
        }
    }
    public void BuildingRelativeAttributes()
    {
        foreach (GameObject building in allBuildings)
        {
            BuildingTagAddition(building);
            BuildingAttritubeDetection(building);
        }
    }

    public void BuildingRelativePlacement()
    {
        foreach(GameObject building in allBuildings)
        {
            Replacement(building);
        }
    }

    #region Phase 1 Randon Generate Stuff
    // Place the building
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

    public void BuildingColor(GameObject building)
    {
            Renderer render;

            render = building.GetComponent<Renderer>();

            render.material.color = Random.ColorHSV(0f, 1f, 0.5f, 1f);
        
    }
    #endregion

    #region Phase 2 Resizing n Color

    public void BuildingTagAddition(GameObject building)
    {
        if(building.transform.localScale.y < 30)
        {
            Renderer render = building.GetComponent<Renderer>();

            render.material.color = shortBuildingColor;

            building.tag = "SmallBuilding";
        }
        else if(building.transform.localScale.y < 55)
        {
            Renderer render = building.GetComponent<Renderer>();

            render.material.color = medBuildingColor;

            building.tag = "MedimunBuilding";
        }
        else
        {
            Renderer render = building.GetComponent<Renderer>();

            render.material.color = tallBuildingColor;

            building.tag = "TallBuilding";
        }
    }
    public void BuildingAttritubeDetection(GameObject building)
    {
        foreach(GameObject otherBuilding in allBuildings)
        {
            if (otherBuilding == building) continue;

            if(Vector3.Distance(building.transform.position, otherBuilding.transform.position) < 5) continue;

            float buildingXScale = building.transform.localScale.x;
            float buildingZScale = building.transform.localScale.z;

            float otherBuildingXScale = otherBuilding.transform.localScale.x;
            float otherBuildingZScale = otherBuilding.transform.localScale.z;

            if(buildingXScale < otherBuildingXScale)
            {
                buildingXScale = otherBuildingXScale;
            }
            if(buildingZScale < otherBuildingZScale)
            {
                buildingZScale = otherBuildingZScale;
            }
        }
    }
    
    #endregion

    #region Phase 3 Replacing with roads

    public void Replacement(GameObject building)
    {
        foreach (GameObject otherBuilding in allBuildings)
        {
            if(otherBuilding == building) continue;

            if(Vector3.Distance(building.transform.position, otherBuilding.transform.position) < 50) continue;
            //else
            //{
            //    building.transform.localPosition = new Vector3
            //        (building.transform.position.x + Random.Range(minBuildingLength, maxBuildingLength + 1), 0, building.transform.position.z + Random.Range(minBuildingLength, maxBuildingLength + 1));
            //}

            
        }
    }
    #endregion
}


