using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public int buildingsPerBlock = 3;

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

    public void Regenerate()
    {
        DeleteAllBuildings();
        BuildingPlacement();
        BuildingRelativePlacement();
        BuildingRelativeAttributes();

    }

    public void DeleteAllBuildings()
    {
        if (allBuildings.Count == 0) return;

        foreach (GameObject building in allBuildings)
        {
            Destroy(building);
        }

        allBuildings.Clear();
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

                if ((z + 1) % buildingsPerBlock == 0)
                {
                    cityZValue += 10f;
                }
            }

            cityXValue += maxBuildingWidth + Random.Range(buildingSpacingMin, buildingSpacingMax + 1);

            if ((x + 1) % buildingsPerBlock == 0)
            {
                cityXValue += 10f;
            }
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
        foreach (GameObject building in allBuildings)
        {
            MovingBuildings(building);
        }
    }

    #region Phase 1 Randon Generate Stuff
    // Place the building
    public void PlacingBuilding(GameObject building)
    {
        float buildingPlacementWidth = Random.Range(minBuildingWidth, maxBuildingWidth + 1);
        float buildingPlacementLength = Random.Range(minBuildingLength, maxBuildingLength + 1);

        building.transform.localPosition = new Vector3(cityXValue + buildingPlacementWidth / 2f, 0, cityZValue + buildingPlacementLength / 2f);
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

    #region Phase 2 Moving buildings closer

    public void MovingBuildings(GameObject building)
    {
        foreach (GameObject otherBuilding in allBuildings)
        {
            if (otherBuilding == building) continue;

            Vector3 difference =
                building.transform.position - otherBuilding.transform.position;

            float minX = (building.transform.localScale.x + otherBuilding.transform.localScale.x) / 2f + buildingSpacingMin;

            float minZ = (building.transform.localScale.z + otherBuilding.transform.localScale.z) / 2f + buildingSpacingMin;

            if (Mathf.Abs(difference.x) < minX && Mathf.Abs(difference.z) < minZ)
            {
                if (minX - Mathf.Abs(difference.x) <
                    minZ - Mathf.Abs(difference.z))
                {
                    float direction = difference.x >= 0 ? 1f : -1f;

                    building.transform.position = new Vector3(otherBuilding.transform.position.x + direction * minX, 0, building.transform.position.z);
                }
                else
                {
                    float direction = difference.z >= 0 ? 1f : -1f;

                    building.transform.position = new Vector3(building.transform.position.x, 0,otherBuilding.transform.position.z + direction * minZ);
                }
            }
        }
    }

    #endregion

    #region Phase 3 Resizing n Color

    public void BuildingTagAddition(GameObject building)
    {
        if (building.transform.localScale.y < 30)
        {
            Renderer render = building.GetComponent<Renderer>();

            render.material.color = shortBuildingColor;

            building.tag = "SmallBuilding";
        }
        else if (building.transform.localScale.y < 55)
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
        foreach (GameObject otherBuilding in allBuildings)
        {
            if (otherBuilding == building) continue;

            if (Vector3.Distance(building.transform.position, otherBuilding.transform.position) <= 25) continue;

            float buildingXScale = building.transform.localScale.x;
            float buildingZScale = building.transform.localScale.z;

            float otherBuildingXScale = otherBuilding.transform.localScale.x;
            float otherBuildingZScale = otherBuilding.transform.localScale.z;

            if (buildingXScale < otherBuildingXScale)
            {
                buildingXScale = otherBuildingXScale;
            }
            if (buildingZScale < otherBuildingZScale)
            {
                buildingZScale = otherBuildingZScale;
            }
        }
    }

    #endregion
}

   



