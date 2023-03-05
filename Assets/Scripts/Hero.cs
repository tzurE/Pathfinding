using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    private GridManager gridManager;
    private Tile currentTile;

    int[] dx = { -1, 0, 1, 0 };
    int[] dy = { 0, 1, 0, -1 };


    // Start is called before the first frame update
    void Start()
    {
        gridManager = GridManager.Instance;
        currentTile = gridManager.gridArray[4, 11];
        StartCoroutine(LookAtChildTiles());
    }

    IEnumerator LookAtChildTiles()
    {
        for (int i = 0; i < 4; i++)
        {
            int nextX = currentTile.x + dx[i];
            int nextY = currentTile.y + dy[i];
            gridManager.gridArray[nextX, nextY].markAsDiscovered();
            yield return new WaitForSeconds(1f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
