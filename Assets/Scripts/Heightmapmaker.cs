﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heightmapmaker : MonoBehaviour {

    private GameObject plane;
    private Terrain terrain;
    [SerializeField]
    private Shader shader;


    // Use this for initialization
    void Start ()
    {
        plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        terrain = Terrain.activeTerrain;
        Createheighttexture();
    }

    // Update is called once per frame
    void Createheighttexture()
    {
        int terrain_resolution = terrain.terrainData.heightmapResolution - 1;
        Texture2D texture = new Texture2D(terrain_resolution, terrain_resolution);
        plane.GetComponent<Renderer>().material.mainTexture = texture;
        plane.GetComponent<Renderer>().material.shader = shader;
        plane.transform.position = new Vector3(terrain.transform.position.x + terrain.terrainData.size.x / 2, terrain.transform.position.y, terrain.transform.position.z + terrain.terrainData.size.z / 2);
        plane.transform.localScale = new Vector3(terrain.terrainData.size.x / 10, 0, terrain.terrainData.size.z / 10);
        plane.layer = 13; //layer 13 is commandermap

        //find lowest point in the map
        float min_height = terrain.terrainData.GetHeight(0, 0);
        for (int i = 0; i < terrain_resolution; i++)
        {
            for (int j = 0; j < terrain_resolution; j++)
            {
                min_height = Mathf.Min(min_height, terrain.terrainData.GetHeight(i, j));
            }
        }

        //create a color and draw it on the texture
        float waterlevel = terrain.transform.position.y * -1;
        for (int i = 0; i < terrain_resolution; i++)
        {
            for (int j = 0; j < terrain_resolution; j++)
            {
                float terrainheight = terrain.terrainData.GetHeight(i, j);
                Color color = new Color(0, 0, 0, 0);
                if (terrainheight > waterlevel)
                {
                    color = new Color(0.95f, 0.9f, 0.7f, 1); /*pale yellow*/
                }
                else
                {
                    if (terrainheight > waterlevel - 6)
                    {
                        color = new Color(1, 1, 1, 1); //white for the shoreline
                    }
                    else
                    {
                        color = new Color(0, Mathf.Floor((terrainheight - min_height - 6) / (waterlevel - min_height - 6) * 10) / 10, 1, 1); //color between blue(0,0,1,1) and cyan(0,1,1,1)
                    }
                }
                texture.SetPixel(terrain_resolution - i, terrain_resolution - j, color); //rotates the location 180° before saving the pixel, don't know why this is necessary but it works
            }
        }
        texture.Apply();
    }
}