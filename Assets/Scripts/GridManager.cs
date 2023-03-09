using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{

    public static GridManager Instance { get; private set; }

    public int height;
    public int width;
    public Tile[,] gridArray;
    public List<Tile> correctPath;
    
    public Tile tilePrefab;
    public Tile heroStartingTile;
    public Tile targetTile;
    public Transform tileParent;

    public bool heroMoving = false;


    public GameObject heroPrefab;
    private GameObject _hero;
    private GameObject _target;
    public GameObject targetPrefab;

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
                spawnedTile.x = x;
                spawnedTile.y = y;
                gridArray[x, y] = spawnedTile;
                if (x == 4  && y == 11)
                {
                    _hero = Instantiate(heroPrefab, new Vector3(x-0.5f, y+0.5f), Quaternion.identity);
                    spawnedTile.GetComponent<Tile>().isHeroOnTile = true;
                    heroStartingTile = spawnedTile;
                }
                else if (x == 27 && y == 11)
                {
                    _target = Instantiate(targetPrefab, new Vector3(x, y), Quaternion.identity);
                    spawnedTile.GetComponent<Tile>().isTargetOnTile = true;
                    targetTile = spawnedTile;
                }

            }
        }

        cam.transform.position = new Vector3((float)width / 2 - 0.5f, (float)height / 2 - 0.5f, -10f);
    }

    void Start()
    {
        gridArray = new Tile[width, height];
        cam = Camera.main.transform;
        correctPath = new List<Tile>();
        GenerateGrid();
    }

    public void SetTileValue(int x, int y, Tile value)
    {

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

    public IEnumerator CalculateBFSCo()
    {
        CoroutineWithData c = new CoroutineWithData(this, BFS.GetShortestPath(heroStartingTile, targetTile));
        yield return c.coroutine;
        List<Node> path = (List < Node > )c.result;
        Debug.Log(path);
        //List<Node> path = BFS.GetShortestPath(gridArray[4, 11], gridArray[27, 11]);
    }

    public void CalculateBFS()
    {
        StartCoroutine(CalculateBFSCo());
    }

    // Update is called once per frame
    void Update()
    {
        if (correctPath.Count != 0 && !heroMoving)
        {
            _hero.GetComponent<Hero>().nextTileIndex = 1;
            _hero.GetComponent<Hero>().moveToTarget = true;
            heroMoving = true;
        }
    }

}
