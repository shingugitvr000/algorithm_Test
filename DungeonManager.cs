// DungeonManager.cs
public class DungeonManager : MonoBehaviour
{
    public static DungeonManager Instance { get; private set; }

    [Header("Grid Settings")]
    public int width = 12;
    public int height = 12;

    [Header("Prefabs")]
    public GameObject floorPrefab;
    public GameObject wallPrefab;
    public GameObject trapPrefab;
    public GameObject playerPrefab;
    public GameObject monsterPrefab;
    public GameObject itemPrefab;
    public GameObject npcPrefab;

    private GridCell[,] grid;

    void Awake()
    {
        if (Instance == null) Instance = this;
        grid = new GridCell[width, height];
        InitializeGrid();
    }

    public GridCell GetCell(int x, int y)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
            return grid[x, y];
        return null;
    }

    public List<GridCell> GetNeighbors(GridCell cell)
    {
        List<GridCell> neighbors = new List<GridCell>();
        int[] dx = { 0, 1, 0, -1 };
        int[] dy = { 1, 0, -1, 0 };

        for (int i = 0; i < 4; i++)
        {
            int newX = cell.x + dx[i];
            int newY = cell.y + dy[i];
            GridCell neighbor = GetCell(newX, newY);

            if (neighbor != null && neighbor.isWalkable)
            {
                neighbors.Add(neighbor);
            }
        }

        return neighbors;
    }
}
