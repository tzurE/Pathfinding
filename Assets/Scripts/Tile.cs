using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private List<Sprite> groundTileToChoose;
    [SerializeField] private List<Sprite> walkableTileToChoose;
    [SerializeField] private List<Sprite> wallTileToChoose;
    [SerializeField] private GameObject hightlight;
    [SerializeField] private Color discoverColor, calculatedColor, originalColor;

    public bool isHeroOnTile = false;
    public bool isTargetOnTile = false;
    public int x, y;

    void Start()
    {
        int index = Random.Range(0, groundTileToChoose.Count);
        spriteRenderer.sprite = groundTileToChoose[index];
        this.tag = "grass";
        originalColor = spriteRenderer.color;
    }

    public void markAsDiscovered()
    {
        spriteRenderer.color = discoverColor;
    }

    public void markNotDiscovered()
    {
        spriteRenderer.color = originalColor;
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
        if (GridManager.Instance.spawnGroundTile)
        {
            int index = Random.Range(0, walkableTileToChoose.Count);
            spriteRenderer.sprite = walkableTileToChoose[index];
            this.tag = "walkable";
        }
        else if (GridManager.Instance.spawnWallTile)
        {
            int index = Random.Range(0, wallTileToChoose.Count);
            spriteRenderer.sprite = wallTileToChoose[index];
            this.tag = "wall";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
