﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{

    private GameObject plane;
    private Terrain terrain;

    [SerializeField]
    private Shader shader;

    [SerializeField]
    private float detectionRadius = 10f;

    CapsuleCollider collider;

    // Use this for initialization
    void Start()
    {
        // Creates a collider for detection of objects
        collider = gameObject.AddComponent<CapsuleCollider>();
        collider.radius = detectionRadius;

        plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        terrain = Terrain.activeTerrain;
        CreateTexture();
    }

    private int FindLowestPointInTerrain()
    {
        float minHeight = terrain.terrainData.GetHeight(0, 0);
        for (int i = 0; i < terrainResolution; i++)
        {
            for (int j = 0; j < terrainResolution; j++)
            {
                minHeight = Mathf.Min(minHeight, terrain.terrainData.GetHeight(i, j));
            }
        }
        return minHeight;
    }

    // Update is called once per frame
    void CreateTexture()
    {
        int terrainResolution = terrain.terrainData.heightmapResolution - 1;
        Texture2D texture = new Texture2D(terrainResolution, terrainResolution);
        plane.GetComponent<Renderer>().material.mainTexture = texture;
        plane.GetComponent<Renderer>().material.shader = shader;
        plane.transform.position = new Vector3(terrain.transform.position.x + terrain.terrainData.size.x / 2, terrain.transform.position.y, terrain.transform.position.z + terrain.terrainData.size.z / 2);
        plane.transform.localScale = new Vector3(terrain.terrainData.size.x / 10, 0, terrain.terrainData.size.z / 10);
        plane.layer = 13; //layer 13 is commandermap

        // Find lowest point in the map.
        float minHeight = terrain.terrainData.GetHeight(0, 0);
        for (int i = 0; i < terrainResolution; i++)
        {
            for (int j = 0; j < terrainResolution; j++)
            {
                minHeight = Mathf.Min(minHeight, terrain.terrainData.GetHeight(i, j));
            }
        }

        //The collider height should be twice minHeight
        collider.height = minHeight * 2;

        // Create a color and draw it on the texture.
        float waterlevel = terrain.transform.position.y * -1;
        for (int i = 0; i < terrainResolution; i++)
        {
            for (int j = 0; j < terrainResolution; j++)
            {
                float terrainheight = terrain.terrainData.GetHeight(i, j);
                Color color = new Color(0, 0, 0, 0);
                if (terrainheight > waterlevel)
                {
                    color = new Color(0.95f, 0.9f, 0.7f, 1); // Pale yellow
                }
                else
                {
                    if (terrainheight > waterlevel - 6)
                    {
                        color = new Color(1, 1, 1, 1); // White for the shoreline
                    }
                    else
                    {
                        color = new Color(0, Mathf.Floor((terrainheight - minHeight - 6) / (waterlevel - minHeight - 6) * 10) / 10, 1, 1); // Color between blue(0,0,1,1) and cyan(0,1,1,1)
                    }
                }
                texture.SetPixel(terrainResolution - i, terrainResolution - j, color); // Rotates the location 180° before saving the pixel, don't know why this is necessary but it works
            }
        }
        texture.Apply();
    }
}
