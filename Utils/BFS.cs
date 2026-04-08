using Microsoft.Extensions.Logging;
using RogueConsole.World;

static class BFS
{
    static ILogger logger { get; set; }

    private class QuePair
    {
        public int first,
            second;

        public QuePair(int first, int second)
        {
            this.first = first;
            this.second = second;
        }
    }

    static readonly int ROW = 16;
    static readonly int COL = 16;

    // Direction vectors

    static int[] dRow = { -1, 0, 1, 0 };
    static int[] dCol = { 0, 1, 0, -1 };

    // Function to check if a cell
    // is be visited or not
    static bool isValid(TileMap[,] Rooms, bool[,] vis, int row, int col)
    {
        // If cell lies out of bounds
        if (row < 0 || col < 0 || row >= ROW || col >= COL)
            return false;

        if (Rooms[row, col] == null)
        {
            return false;
        }

        // If cell is already visited
        if (vis[row, col])
            return false;

        // Otherwise
        return true;
    }

    // Function to perform the BFS traversal
    static (int x, int y) Search(TileMap[,] grid, bool[,] vis, int row, int col)
    {
        // Stores indices of the matrix cells
        Queue<QuePair> q = new Queue<QuePair>();

        // Mark the starting cell as visited
        // and push it into the queue
        q.Enqueue(new QuePair(row, col));
        vis[row, col] = true;

        var lastRoom = (row, col);
        // Iterate while the queue
        // is not empty
        while (q.Count != 0)
        {
            QuePair cell = q.Peek();
            int x = cell.first;
            int y = cell.second;
            logger.LogInformation(" {grid}", grid[x, y]);
            q.Dequeue();
            // Go to the adjacent cells
            for (int i = 0; i < 4; i++)
            {
                int adjx = x + dRow[i];
                int adjy = y + dCol[i];
                if (isValid(grid, vis, adjx, adjy))
                {
                    q.Enqueue(new QuePair(adjx, adjy));
                    vis[adjx, adjy] = true;
                    lastRoom = (adjx, adjy);
                }
            }
        }
        return lastRoom;
    }

    // Driver Code
    public static (int x, int y) Execute(TileMap[,] Rooms, ILogger _logger)
    {
        logger = _logger;

        // Declare the visited array
        bool[,] vis = new bool[ROW, COL];
        return Search(Rooms, vis, 8, 8);
    }
}

