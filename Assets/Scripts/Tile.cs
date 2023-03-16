using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tile : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private List<Sprite> groundTileToChoose;
    [SerializeField] private List<Sprite> walkableTileToChoose;
    [SerializeField] private List<Sprite> wallTileToChoose;
    [SerializeField] private GameObject hightlight;
    [SerializeField] private GameObject hightlightPath;
    [SerializeField] private Color discoverColor, calculatedColor, originalColor;
    public Transform textTransform;
    public TextMeshProUGUI textOnTile;

    GridManager gridInstance;
    public bool isHeroOnTile = false;
    public bool isTargetOnTile = false;
    public int x, y;

    void Start()
    {
        int index = Random.Range(0, groundTileToChoose.Count);
        spriteRenderer.sprite = groundTileToChoose[index];
        this.tag = "grass";
        originalColor = spriteRenderer.color;
        gridInstance = GridManager.Instance;
        textOnTile = textTransform.gameObject.GetComponent<TextMeshProUGUI>();
        textOnTile.SetText("");
    }

    public void markAsCalculated()
    {
        spriteRenderer.color = calculatedColor;
    }

    public void markAsDiscovered()
    {
        spriteRenderer.color = discoverColor;
    }

    public void markNotDiscovered()
    {
        spriteRenderer.color = originalColor;
    }

    public void markHighlightPath()
    {
        hightlightPath.SetActive(true);
    }

    public void unMarkHighlightPath()
    {
        hightlightPath.SetActive(false);
    }

    private void OnMouseEnter()
    {
        hightlight.SetActive(true);
    }

    private void OnMouseExit()
    {
        hightlight.SetActive(false);
    }

    private void OnMouseDown()
    {

        if (gridInstance.spawnGroundTile)
        {
            int index = Random.Range(0, walkableTileToChoose.Count);
            spriteRenderer.sprite = walkableTileToChoose[index];
            this.tag = "walkable";
        }
        else if (gridInstance.spawnWallTile)
        {
            int index = Random.Range(0, wallTileToChoose.Count);
            spriteRenderer.sprite = wallTileToChoose[index];
            this.tag = "wall";
        }
        if (gridInstance.spawnHero)
        {
            gridInstance.heroTile.isHeroOnTile = false;
            this.isHeroOnTile = true;
            Destroy(gridInstance._hero);
            gridInstance._hero = Instantiate(gridInstance.heroPrefab, new Vector3(x - 0.5f, y + 0.5f), Quaternion.identity);
            gridInstance.heroTile = this;
            gridInstance.spawnHero = false;
        }
        if (gridInstance.spawnTarget)
        {
            gridInstance.targetTile.isTargetOnTile = false;
            this.isTargetOnTile = true;
            Destroy(gridInstance._target);
            gridInstance._target = Instantiate(gridInstance.targetPrefab, new Vector3(x, y), Quaternion.identity);
            gridInstance.targetTile = this;
            gridInstance.spawnTarget = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
