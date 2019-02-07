using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Commander : PlayerCharacter
{
    [SerializeField]
    private Map map;
    private Renderer rend;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        //camera.transform.SetParent(null);

        map = Instantiate(map);
        map.SetCameraPosition(submarine.transform.position);
        rend = map.GetComponentInChildren<Renderer>();
        //rend = transform.Find("MapPlane").gameObject.GetComponent<Renderer>(); //moet dit niet ook kunnen met map.GetComponentInChildren<>() oid
    }

    protected void Update()
    {
        float subHeight = (submarine.transform.position.y - Terrain.activeTerrain.transform.position.y - 6) / Terrain.activeTerrain.terrainData.heightmapHeight;
        rend.material.SetFloat("_Height", subHeight);
    }
}

/*
 * void Start()
    {
        rend = GetComponent<Renderer> ();

        // Use the Specular shader on the material
        rend.material.shader = Shader.Find("Specular");
    }

    void Update()
    {
        // Animate the Shininess value
        float shininess = Mathf.PingPong(Time.time, 1.0f);
        rend.material.SetFloat("_Shininess", shininess);
    }
}
*/