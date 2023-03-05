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

    public static List<Tile> GetShortestPath(Tile startTile, Tile endTile)
    {
        Queue<Node> queue = new Queue<Node>();
        HashSet<Node> visited = new HashSet<Node>();

        // add starting node to queue and visited set
        Node startNode = new Node(startTile.x, startTile.y, 0, null);
        queue.Enqueue(startNode);
        visited.Add(startNode);

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
