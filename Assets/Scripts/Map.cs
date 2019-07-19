using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    private GameObject _mapPlane;
    private Terrain _terrain;
    private Camera _camera;

    [SerializeField]
    private float detectionRadius = 10f;

    private CapsuleCollider collider;
    private int terrainResolution;
    private float lowestPointBelowWater, waterLevel = 0; // This should be a fancy function that gets the waterLevel.
    private Vector3 mapSize;
    private float maxZoom;

    private void Awake()
    {
        _mapPlane = transform.Find("MapPlane").gameObject;
        _terrain = Terrain.activeTerrain;
        terrainResolution = _terrain.terrainData.heightmapResolution - 1;

        lowestPointBelowWater = GetLowestPointBelowWater();
        CreateTexture();

        // Creates a collider for detection of objects.
        collider = GetComponent<CapsuleCollider>();
        collider.radius = detectionRadius;

        //The collider height should be twice the height of the lowest point in the terrain.
        collider.height = lowestPointBelowWater * 2;

        _camera = GetComponentInChildren<Camera>();
        mapSize = _terrain.terrainData.size;
        maxZoom = Mathf.Min(mapSize.x * (float)Screen.height / (float)Screen.width * 0.5f, mapSize.z / 2);
    }

    private void Update()
    {
        Zoom();
        Scroll();
    }

    // Finds lowest point in the map.
    private float GetLowestPointBelowWater()
    {
        float lowestPoint = _terrain.terrainData.GetHeight(0, 0);
        for (int x = 0; x < terrainResolution; x++)
        {
            for (int y = 0; y < terrainResolution; y++)
            {
                float currentHeight = _terrain.terrainData.GetHeight(x, y);
                lowestPoint = lowestPoint < currentHeight ? lowestPoint : currentHeight;
            }
        }
        return lowestPoint + (waterLevel + _terrain.transform.position.y);
    }

    private void CreateTexture()
    {
        // Places the mapPlane over the terrain and makes it the same size as the terrain.       
        _mapPlane.transform.position = new Vector3(_terrain.transform.position.x + _terrain.terrainData.size.x / 2, _terrain.transform.position.y, _terrain.transform.position.z + _terrain.terrainData.size.z / 2);
        _mapPlane.transform.localScale = new Vector3(_terrain.terrainData.size.x / 10, 0, _terrain.terrainData.size.z / 10);

        // Place the mapPlane on a seperate layer only the attached _camera can see.
        _mapPlane.layer = LayerMask.NameToLayer("Map");

        // Use a color array since this is much quicker than calling texture.SetPixel for each pixel in the texture.
        Color[] textureColors = new Color[terrainResolution * terrainResolution];

        // Finds the corect color for each pixel in the texture 
        for (int y = 0; y < terrainResolution; y++)
        {
            for (int x = 0; x < terrainResolution; x++)
            {
                float terrainheight = _terrain.terrainData.GetHeight(y, x) + _terrain.transform.position.y;
                if (terrainheight > waterLevel)
                {
                    textureColors[x + (y * terrainResolution)] = new Color(0.95f, 0.9f, 0.7f, 1); // Color for terrain above waterLevel.
                }
                else if (terrainheight > waterLevel - 6) // Vraag van Maarten: wat is 6? Is dit shoreOffset of iets dergelijks?
                {
                    textureColors[x + (y * terrainResolution)] = Color.white;
                }
                else
                {
                    // Todo: fix the magic numbers here.
                    textureColors[x + (y * terrainResolution)] = new Color(0, Mathf.Floor((terrainheight - lowestPointBelowWater - 6) / (waterLevel - lowestPointBelowWater - 6) * 10) / 10, 1, 1); // Color between blue(0,0,1,1) and cyan(0,1,1,1).
                }
            }
        }

        // Creates the texture for the planeMap and sets the colors using the color array.
        Texture2D texture = new Texture2D(terrainResolution, terrainResolution);
        texture.SetPixels(textureColors);
        texture.Apply();
        _mapPlane.GetComponent<Renderer>().material.mainTexture = texture;
    }

    private void Zoom()
    {
        // If there are two touches on the device...
        if (Input.touchCount == 2)
        {
            // Store both touches.
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            // ... change the orthographic size based on the change in distance between the touches.
            _camera.orthographicSize += deltaMagnitudeDiff;

            // Make sure the orthographic size never drops below zero.
            _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize, 250, maxZoom);

            if (_camera.ScreenToWorldPoint(new Vector3(0, 0, 0)).x < _terrain.transform.position.x) { _camera.transform.position -= new Vector3(_camera.ScreenToWorldPoint(new Vector3(0, 0, 0)).x - _terrain.transform.position.x, 0, 0); }
            if (_camera.ScreenToWorldPoint(new Vector3(0, 0, 0)).z < _terrain.transform.position.z) { _camera.transform.position -= new Vector3(0, 0, _camera.ScreenToWorldPoint(new Vector3(0, 0, 0)).z + -_terrain.transform.position.z); }

            if (_camera.ScreenToWorldPoint(new Vector3((float)Screen.width, (float)Screen.height, 0)).x > mapSize.x + _terrain.transform.position.x) { _camera.transform.position -= new Vector3(_camera.ScreenToWorldPoint(new Vector3((float)Screen.width, (float)Screen.height, 0)).x - mapSize.x - _terrain.transform.position.x, 0, 0); }
            if (_camera.ScreenToWorldPoint(new Vector3((float)Screen.width, (float)Screen.height, 0)).z > mapSize.z + _terrain.transform.position.z) { _camera.transform.position -= new Vector3(0, 0, _camera.ScreenToWorldPoint(new Vector3((float)Screen.width, (float)Screen.height, 0)).z - mapSize.z - _terrain.transform.position.z); }
        }
    }

    private void Scroll()
    {
        if (Input.touchCount == 1)
        {
            Touch touchZero = Input.GetTouch(0);
            if (touchZero.phase == TouchPhase.Moved)
            {
                Vector3 camera_movement = new Vector3(touchZero.deltaPosition.x, 0, touchZero.deltaPosition.y) * _camera.orthographicSize / -300;

                if (_camera.ScreenToWorldPoint(new Vector3(0, 0, 0)).x + camera_movement.x < _terrain.transform.position.x) { camera_movement.x = -_camera.ScreenToWorldPoint(new Vector3(0, 0, 0)).x + _terrain.transform.position.x; }
                if (_camera.ScreenToWorldPoint(new Vector3(0, 0, 0)).z + camera_movement.z < _terrain.transform.position.z) { camera_movement.z = -_camera.ScreenToWorldPoint(new Vector3(0, 0, 0)).z + _terrain.transform.position.z; }

                if (_camera.ScreenToWorldPoint(new Vector3((float)Screen.width, (float)Screen.height, 0)).x + camera_movement.x > mapSize.x + _terrain.transform.position.x) { camera_movement.x = mapSize.x + _terrain.transform.position.x - _camera.ScreenToWorldPoint(new Vector3((float)Screen.width, (float)Screen.height, 0)).x; }
                if (_camera.ScreenToWorldPoint(new Vector3((float)Screen.width, (float)Screen.height, 0)).z + camera_movement.z > mapSize.z + _terrain.transform.position.z) { camera_movement.z = mapSize.z + _terrain.transform.position.z - _camera.ScreenToWorldPoint(new Vector3((float)Screen.width, (float)Screen.height, 0)).z; }

                _camera.transform.position += camera_movement;
            }
        }
    }

    // When a detectable object enters our detection radius we enable it's icon.
    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.GetComponent<DetectableObject>().SetIconActive(true);        
    }

    // If a detectable object leaves our detection radius we disable it's icon.
    private void OnTriggerExit(Collider other)
    {
        other.gameObject.GetComponent<DetectableObject>().SetIconActive(true);
    }

    public void SetCameraPosition(Vector3 submarineLocation)
    {
        Vector3 cameraPosition = _camera.transform.position;
        cameraPosition.x = submarineLocation.x;
        cameraPosition.z = submarineLocation.z;
        _camera.transform.position = cameraPosition;
    }
}
