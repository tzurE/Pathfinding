using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GridManager : MonoBehaviour
{

    public static GridManager Instance { get; private set; }

    public int height;
    public int width;
    public Tile[,] gridArray;
    public List<Tile> correctPath;
    public Dictionary<(int, int), Tile> tileDict = new Dictionary<(int, int), Tile>();
    public float cellSize;
    
    public Tile tilePrefab;
    public Tile heroTile;
    public Tile targetTile;
    public Transform tileParent;

    public bool heroMoving = false;


    public GameObject heroPrefab;
    public GameObject _hero;
    public GameObject _target;
    public GameObject targetPrefab;

    [SerializeField] private Transform cam;

    public bool spawnGrassTile = true;
    public bool spawnGroundTile = false;
    public bool spawnWallTile = false;
    public bool spawnHero = false;
    public bool spawnTarget = false;

    public string blockedTile = "wall";


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
                    heroTile = spawnedTile;
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
        cellSize = 1;
        GenerateGrid();
    }

    public Vector3 getWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize;
    } 

    public void SetValue(Vector3 worldPosition, int value)
    {

    }

    public void SetValue(int x, int y, Tile tile)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = tile;
        }
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

    public void MoveHero()
    {
        spawnHero = true;
    }

    public void MoveTarget()
    {
        spawnTarget = true;
    }

    public IEnumerator CalculateBFSCo()
    {
        CoroutineWithData c = new CoroutineWithData(this, BFS.GetShortestPath(heroTile, targetTile));
        yield return c.coroutine;
        List<Node> path = (List < Node > )c.result;
    }

    public IEnumerator CalculateAStarCo()
    {
        CoroutineWithData c = new CoroutineWithData(this, AStar.FindShortestPath(heroTile, targetTile));
        yield return c.coroutine;
    }

    public void CalculateBFS()
    {
        StartCoroutine(CalculateBFSCo());
    }

    public IEnumerator CalculateDFSCo()
    {
        CoroutineWithData c = new CoroutineWithData(this, DFS.FindShortestPath(heroTile, targetTile));
        yield return c.coroutine;
    }

    public void CalculateDFS()
    {
        StartCoroutine(CalculateDFSCo());
    }

    public void CalculateAStar()
    {
        StartCoroutine(CalculateAStarCo());
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

    public void resetScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void CleanGridWithoutPath()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (!tileDict.ContainsKey((x, y)))
                {
                    gridArray[x, y].markNotDiscovered();
                }
            }
        }
                
    }

    public void ClearGrid()
    {
        heroTile.isHeroOnTile = false;
        targetTile.isTargetOnTile = false;
        Destroy(_hero);
        Destroy(_target);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                gridArray[x,y].markNotDiscovered();
                gridArray[x, y].textOnTile.SetText("");
                gridArray[x, y].unMarkHighlightPath();
                if (x == 4 && y == 11)
                {
                    _hero = Instantiate(heroPrefab, new Vector3(x - 0.5f, y + 0.5f), Quaternion.identity);
                    gridArray[x, y].GetComponent<Tile>().isHeroOnTile = true;
                    heroTile = gridArray[x, y];
                }
                else if (x == 27 && y == 11)
                {
                    _target = Instantiate(targetPrefab, new Vector3(x, y), Quaternion.identity);
                    gridArray[x, y].GetComponent<Tile>().isTargetOnTile = true;
                    targetTile = gridArray[x, y];
                }
            }
        }

    }

}
