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
        DetectBuildings();
        RandomColor();
    }


    public void RandomPlacements()
    {
        for(int x = 0; x < cityWidth; x++)
        {
            for(int z = 0; z < cityLength; z++)
            {
                GameObject building = Instantiate(buildingPrefab, transform);
                float posX = x * Random.Range(buildingSpacingMin, buildingSpacingMax + 1);
                float posZ = z * Random.Range(buildingSpacingMin, buildingSpacingMax + 1);
               

                building.transform.localPosition = new Vector3(posX, 0, posZ);
                allBuildings.Add(building);
                
            }
        }
    }

    public void DetectBuildings()
    {
        List<GameObject> toRemove = new List<GameObject>();

        for (int i = 0; i < allBuildings.Count; i++)
        {
            if(i != 0)
            {
                GameObject buildingToCheck = allBuildings[i];
                GameObject building = allBuildings[i - 1];

                Vector3[] buildingToCheckVertices = buildingToCheck.GetComponent<MeshFilter>().sharedMesh.vertices;
                Bounds buildingToCheckBounds = GeometryUtility.CalculateBounds(buildingToCheckVertices, buildingToCheck.transform.localToWorldMatrix);

                Vector3[] buildingVertices = building.GetComponent<MeshFilter>().sharedMesh.vertices;
                Bounds buildingBounds = GeometryUtility.CalculateBounds(buildingVertices, building.transform.localToWorldMatrix);

                float buildingToCheckMinX = buildingToCheckBounds.min.x;
                float buildingToCheckMinZ = buildingToCheckBounds.min.z;

                float buildingToCheckMaxX = buildingToCheckBounds.max.x;
                float buildingToCheckMaxZ = buildingToCheckBounds.max.z;

                float buildingMinX = buildingBounds.min.x;
                float buildingMinZ = buildingBounds.min.z;

                float buildingMaxX = buildingBounds.max.x;
                float buildingMaxZ = buildingBounds.max.z;

                // Need to see if its in the box by checking the corners (minx, minz) (maxX, maxZ), 

                if (buildingToCheckMinX <= buildingMaxX) // Min X - Min z, Max Z
                {
                    if (buildingToCheckMinZ <= buildingMaxZ)
                    {
                        toRemove.Add(buildingToCheck);
                    }
                    if (buildingToCheckMaxZ >= buildingMinX)
                    {
                        toRemove.Add(buildingToCheck);
                    }
                    
                }

                if (buildingToCheckMaxX >= buildingMinX) // Max X Max z
                {
                    if (buildingToCheckMaxZ >= buildingMinX)
                    {
                        toRemove.Add(buildingToCheck);
                    }
                    if (buildingToCheckMinZ <= buildingMaxZ)
                    {
                        toRemove.Add(buildingToCheck);
                    }
                }

            }


        }

        foreach(GameObject building in toRemove)
        {
            Destroy(building);
            allBuildings.Remove(building);
        }
        
    }


    // scales all building based off of a set of rules
    public void BuildingScales()
    {
        foreach(GameObject building in allBuildings)
        {
            Vector3 heightScale = building.transform.localScale;
            heightScale.y = HeightScale(heightScale.y, heightScale.z);
            //heightScale.x = Random.Range(minBuildingWidth, maxBuildingWidth);
            //heightScale.z = Random.Range(minBuildingLength, maxBuildingLength);
            building.transform.localScale = heightScale;

        }
    }

    

    private int HeightScale(float h, float z)
    {
        int buildingHeight = 0;

        float noise = Mathf.PerlinNoise(h * 0.01f, z * 0.01f);

        buildingHeight = Mathf.Clamp(Mathf.RoundToInt(noise * maxBuildingHeight) + Random.Range(minBuildingHeight, maxBuildingHeight), minBuildingHeight, maxBuildingHeight);
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


