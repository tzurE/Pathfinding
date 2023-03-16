using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ANode
{
    public int x, y;
    public int gCost; // distance from starting node
    public int hCost; // heuristic distance from end node
    public ANode parent;
    public int fCost { get { return gCost + hCost; } }

    public ANode(int x, int y, int gCost, ANode parent)
    {
        this.x = x;
        this.y = y;
        this.gCost = gCost;
        this.parent = parent;
    }
}

class ANodeEqualityComparer : IEqualityComparer<ANode>
{
    public bool Equals(ANode b1, ANode b2)
    {
        if (b2 == null && b1 == null)
            return true;
        else if (b1 == null || b2 == null)
            return false;
        else if (b1.x == b2.x && b1.y == b2.y)
            return true;
        else
            return false;
    }

    public int GetHashCode(ANode bx)
    {
        int hCode = bx.x ^ bx.y;
        return hCode.GetHashCode();
    }
}
public class AStar : MonoBehaviour
{
    private static List<ANode> openList;
    private static HashSet<ANode> closedList;
    private static ANode[,] astarGrid;
    private static int MOVE_DIAGONAL_COST = 14;
    private static int MOVE_STRAIGHT_COST = 10;
    private static List<Tile> tilePath = new List<Tile>();

    public static IEnumerator FindShortestPath(Tile startTile, Tile endTile)
    {
        int[] dx = { -1, 0, 1, 0 };
        int[] dy = { 0, 1, 0, -1 };

        GridManager gridManager = GridManager.Instance;

        gridManager.tileDict.Clear();
        gridManager.correctPath.Clear();
        gridManager.heroMoving = false;


        astarGrid = new ANode[gridManager.width, gridManager.height];
        ANodeEqualityComparer ANodeEqC = new ANodeEqualityComparer();
        openList = new List<ANode>();
        closedList = new HashSet<ANode>(ANodeEqC);
        List<ANode> path = new List<ANode>();
        bool found = false;
        ANode startNode = new ANode(startTile.x, startTile.y, 0, null);
        ANode endNode = new ANode(endTile.x, endTile.y, int.MaxValue, null);
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        openList.Add(startNode);

        startTile.textOnTile.SetText("0");
        while (openList.Count > 0 && !found)
        {
            ANode currentNode = openList[0];

            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].fCost < currentNode.fCost || openList[i].fCost == currentNode.fCost && openList[i].hCost < currentNode.hCost)
                {
                    currentNode = openList[i];
                }
            }
            openList.Remove(currentNode);
            closedList.Add(currentNode);
            gridManager.gridArray[currentNode.x, currentNode.y].markAsCalculated();
            yield return new WaitForSeconds(0.1f);

            if (currentNode.x == endNode.x && currentNode.y == endNode.y)
            {
                GetPath(startNode, currentNode);
                found = true;
                break;
            }

            // check neighbours
            for (int i = 0; i < 4; i++)
            {
                int nextX = currentNode.x + dx[i];
                int nextY = currentNode.y + dy[i];
                ANode neighborNode = new ANode(nextX, nextY, currentNode.gCost + 1, currentNode);
                if (closedList.Contains(neighborNode)) continue;
                if (nextX >= 0 && nextX < gridManager.width && nextY >= 0 && nextY < gridManager.height && gridManager.gridArray[nextX, nextY].tag != gridManager.blockedTile)
                {
                    int tentativeG = currentNode.gCost + CalculateDistanceCost(currentNode, neighborNode);
                    if (!openList.Contains(neighborNode) || tentativeG < neighborNode.gCost)
                    {
                        neighborNode.gCost = tentativeG;
                        gridManager.gridArray[neighborNode.x, neighborNode.y].textOnTile.SetText((tentativeG/10).ToString());
                        neighborNode.hCost = CalculateDistanceCost(neighborNode, endNode);
                        neighborNode.parent = currentNode;
                        if (!openList.Contains(neighborNode)) openList.Add(neighborNode);
                    }
                }
            }
        }
        
        yield return null;
    }

    private static List<ANode> GetPath(ANode startNode, ANode endNode)
    {
        List<ANode> path = new List<ANode>();
        ANode currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Add(startNode);
        path.Reverse();
        for (int i = 0; i < path.Count; i++)
        {
            Tile tile = GridManager.Instance.gridArray[path[i].x, path[i].y];
            tile.markHighlightPath();
            tilePath.Add(tile);
            GridManager.Instance.tileDict.Add((tile.x, tile.y), tile);
        }
        
        GridManager.Instance.CleanGridWithoutPath();
        GridManager.Instance.correctPath = tilePath;
        return path;
    }

    public static int CalculateDistanceCost(ANode a, ANode b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }
}
