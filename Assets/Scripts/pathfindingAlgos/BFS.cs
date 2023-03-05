using System.Collections;
using System.Collections.Generic;

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

public class BFS
{
    private int distance = 1;

    public static List<Node> GetShortestPath(Tile startTile, Tile endTile)
    {
        Queue<Node> queue = new Queue<Node>();
        HashSet<Node> visited = new HashSet<Node>();

        // add starting node to queue and visited set
        Node startNode = new Node(startTile.x, startTile.y, 0, null);
        queue.Enqueue(startNode);
        visited.Add(startNode);

        while (queue.Count > 0)
        {
            // get next node from queue
            Node currentNode = queue.Dequeue();

            // check if we've reached the end node
            if (currentNode.x == endTile.x && currentNode.y == endTile.y)
            {
                // backtrack through parents to get path
                List<Node> path = new List<Node>();
                while (currentNode != null)
                {
                    path.Add(currentNode);
                    currentNode = currentNode.parent;
                }
                path.Reverse();
                return path;
            }

            // check adjacent nodes
            int[] dx = { -1, 0, 1, 0 };
            int[] dy = { 0, 1, 0, -1 };
            for (int i = 0; i < 4; i++)
            {
                int nextX = currentNode.x + dx[i];
                int nextY = currentNode.y + dy[i];

                // check if the next node is within bounds and not blocked
                if (nextX >= 0 && nextX < rows && nextY >= 0 && nextY < cols && grid[nextX, nextY] == 0)
                {
                    Node nextNode = new Node(nextX, nextY, currentNode.distance + 1, currentNode);

                    // check if we've already visited this node
                    if (!visited.Contains(nextNode))
                    {
                        queue.Enqueue(nextNode);
                        visited.Add(nextNode);
                    }
                }
            }
        }

        // no path found
        return null;

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
