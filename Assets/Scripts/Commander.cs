using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Commander : PlayerCharacter
{
    [SerializeField]
    private Map map;
    private Renderer rend;
    private bool collisionMap = false;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        //camera.transform.SetParent(null);

        map = Instantiate(map);
        map.SetCameraPosition(submarine.transform.position);
        rend = map.GetComponentInChildren<Renderer>();
        rend.material.SetFloat("_Height", 1f);
    }

    protected void Update()
    {
        rigidbody.AddForce(rigidbody.transform.forward * 200000 * Input.GetAxis("Vertical")); // for debugging
        rigidbody.AddTorque(rigidbody.transform.up * 25000000* Input.GetAxis("Horizontal"));


        if (collisionMap)
        {
            float subHeight = (submarine.transform.position.y - Terrain.activeTerrain.transform.position.y - 7.5f) / Terrain.activeTerrain.terrainData.heightmapHeight;
            rend.material.SetFloat("_Height", subHeight);
        }
    }

    public void toggleCollisionMap()
    {
        if (collisionMap)
        {
            collisionMap = false;
            rend.material.SetFloat("_Height", 1f);
        }
        else
        {
            collisionMap = true;
        }
        
    }
}