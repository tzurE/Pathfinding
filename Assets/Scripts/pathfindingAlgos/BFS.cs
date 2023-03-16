using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Node
{
    public int x, y, distance;
    public Node parent;

    public Node(int x, int y, int distance, Node parent)
    {
        this.x = x;
        this.y = y;
        this.distance = distance;
        this.parent = parent;
    }
}

class NodeEqualityComparer : IEqualityComparer<Node>
{
    public bool Equals(Node b1, Node b2)
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

    public int GetHashCode(Node bx)
    {
        int hCode = bx.x ^ bx.y;
        return hCode.GetHashCode();
    }
}

public class BFS: MonoBehaviour
{
    private static int distance = 1;
    private static string blockedTile = "wall";

    //public static List<Node> GetShortestPath(Tile startTile, Tile endTile)
    public static IEnumerator GetShortestPath(Tile startTile, Tile endTile)
    {
        GridManager gridManager = GridManager.Instance;
        gridManager.tileDict.Clear();
        gridManager.correctPath.Clear();
        gridManager.heroMoving = false;
        Queue<Node> queue = new Queue<Node>();
        NodeEqualityComparer NodeEqC = new NodeEqualityComparer();
        HashSet<Node> visited = new HashSet<Node>(NodeEqC);
        List<Node> path = new List<Node>();
        List<Tile> tilePath = new List<Tile>();
        startTile.markAsDiscovered();
        yield return new WaitForSeconds(0.2f);

        // add starting node to queue and visited set
        Node startNode = new Node(startTile.x, startTile.y, 0, null);
        queue.Enqueue(startNode);
        visited.Add(startNode);
        bool found = false;

        while (queue.Count > 0 && !found)
        {
            // get next node from queue
            Node currentNode = queue.Dequeue();
            gridManager.gridArray[currentNode.x, currentNode.y].markAsCalculated();
            yield return new WaitForSeconds(0.0001f);

            // check if we've reached the end node
            if (currentNode.x == endTile.x && currentNode.y == endTile.y)
            {
                // backtrack through parents to get path
                while (currentNode != null)
                {
                    path.Add(currentNode);
                    currentNode = currentNode.parent;
                }
                path.Reverse();
                for(int i = 0; i < path.Count; i++)
                {
                    Tile tile = GridManager.Instance.gridArray[path[i].x, path[i].y];
                    tile.markHighlightPath();
                    tilePath.Add(tile);
                    GridManager.Instance.tileDict.Add((tile.x, tile.y), tile);
                }
                found = true;
                GridManager.Instance.CleanGridWithoutPath();
                GridManager.Instance.correctPath = tilePath;
                break;
            }

            // check adjacent nodes
            int[] dx = { -1, 0, 1, 0 };
            int[] dy = { 0, 1, 0, -1 };
            for (int i = 0; i < 4; i++)
            {
                int nextX = currentNode.x + dx[i];
                int nextY = currentNode.y + dy[i];
                //yield return new WaitForSeconds(0.00001f);
                // check if the next node is within bounds and not blocked
                if (nextX >= 0 && nextX < gridManager.width && nextY >= 0 && nextY < gridManager.height && gridManager.gridArray[nextX, nextY].tag != blockedTile)
                {
                    Tile nextTile = gridManager.gridArray[nextX, nextY];
                    Node nextNode = new Node(nextX, nextY, currentNode.distance + distance, currentNode);

                    // check if we've already visited this node
                    if (!visited.Contains(nextNode))
                    {
                        nextTile.markAsDiscovered();
                        nextTile.textOnTile.SetText(nextNode.distance.ToString());
                        queue.Enqueue(nextNode);
                        visited.Add(nextNode);
                    }
                }
            }
        }

        // no path found
        yield return null;

    }
}
