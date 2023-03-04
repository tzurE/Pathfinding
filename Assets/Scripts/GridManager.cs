using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{

    public static GridManager Instance { get; private set; }

    [SerializeField] private int height;
    [SerializeField] private int width;
    private int[,] gridArray;
    
    public Tile tilePrefab;
    public Transform tileParent;

    [SerializeField] private Transform cam;

    public bool spawnGrassTile = true;
    public bool spawnGroundTile = false;
    public bool spawnWallTile = false;


    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void GenerateGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Tile spawnedTile = Instantiate(tilePrefab ,new Vector3(x, y), Quaternion.identity, tileParent);
                spawnedTile.name = $"Tile {x}, {y}";

                //bool isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                //spawnedTile.Init(isOffset);
            }
        }

        cam.transform.position = new Vector3((float)width / 2 - 0.5f, (float)height / 2 - 0.5f, -10f);
    }

    void Start()
    {
        cam = Camera.main.transform;
        GenerateGrid();
    }

    public void SpawnGroundTile()
    {
        spawnGroundTile = true;
        spawnGrassTile = false;
        spawnWallTile = false;
    }

    public void SpawnWallTile()
    {
        spawnGroundTile = false;
        spawnGrassTile = false;
        spawnWallTile = true;
    }

    public void SpawnGrasTile()
    {
        spawnGroundTile = false;
        spawnGrassTile = true;
        spawnWallTile = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

}
