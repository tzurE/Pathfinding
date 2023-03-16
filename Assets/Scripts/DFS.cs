using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class DFS : MonoBehaviour
{
    private static int distance = 1;
    private static string blockedTile = "wall";
    private static bool found = false;

    public static IEnumerator FindShortestPath(Tile startTile, Tile endTile)
    {
        int[] row = { -1, 0, 0, 1 };
        int[] col = { 0, -1, 1, 0 };

        GridManager gridManager = GridManager.Instance;
        gridManager.tileDict.Clear();
        gridManager.correctPath.Clear();
        gridManager.heroMoving = false;
        Stack<Node> stack = new Stack<Node>();
        NodeEqualityComparer NodeEqC = new NodeEqualityComparer();
        HashSet<Node> visited = new HashSet<Node>(NodeEqC);
        List<Node> path = new List<Node>();
        List<Tile> tilePath = new List<Tile>();
        startTile.markAsDiscovered();

        Node startNode = new Node(startTile.x, startTile.y, 0, null);
        // Mark the starting cell as visited
        visited.Add(startNode);

        // Create a stack to hold the cells to visit
        // Add the starting cell to the stack
        stack.Push(startNode);

        // Loop while there are still cells to visit
        while (stack.Count > 0 && !found)
        {
            // Pop the next cell from the stack
            Node currentNode = stack.Pop();
            gridManager.gridArray[currentNode.x, currentNode.y].markAsCalculated();
            yield return new WaitForSeconds(0.001f);
            // Check if the current cell is the end cell
            if (currentNode.y == endTile.y && currentNode.x == endTile.x)
            {
                // Return the shortest path to the end cell
                // backtrack through parents to get path
                while (currentNode != null)
                {
                    path.Add(currentNode);
                    currentNode = currentNode.parent;
                }
                path.Reverse();
                for (int i = 0; i < path.Count; i++)
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

            // Loop through the neighboring cells
            for (int i = 0; i < 4; i++)
            {
                int nextY = currentNode.y + row[i];
                int nextX = currentNode.x + col[i];

                Node nextNode = new Node(nextX, nextY, currentNode.distance + distance, currentNode);
                // Check if the neighboring cell is within the bounds of the grid
                if (nextY >= 0 && nextY < gridManager.height && nextX >= 0 && nextX < gridManager.width)
                {
                    Tile nextTile = gridManager.gridArray[nextX, nextY];
                    nextTile.markAsDiscovered();
                    // Check if the neighboring cell is not a wall and has not been visited
                    if (gridManager.gridArray[nextX, nextY].tag != blockedTile && !visited.Contains(nextNode))
                    {
                        nextTile.textOnTile.SetText(nextNode.distance.ToString());
                        // Mark the neighboring cell as visited
                        visited.Add(nextNode);
                        // Add the neighboring cell to the stack with an increased distance
                        stack.Push(nextNode);
                    }
                }
            }
        }

        // If the end cell was not reached, return -1 to indicate no path was found
        yield return -1;
    }
}
