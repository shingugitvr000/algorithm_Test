public class GridCell : MonoBehaviour
{
    public int x, y;                   
    public CellType cellType;          
    public bool isWalkable = true;     
    public float movementCost = 1f;    
    public GameObject occupyingObject;  

    private MeshRenderer meshRenderer;
    private Color originalColor;

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        originalColor = meshRenderer.material.color;
    }

    public void Initialize(int xPos, int yPos, CellType type)
    {
        x = xPos;
        y = yPos;
        cellType = type;

        switch (type)
        {
            case CellType.Wall:
                isWalkable = false;
                movementCost = float.MaxValue;
                break;
            case CellType.Trap:
                movementCost = 2f;
                break;
        }
    }

    public void SetHighlight(Color color)
    {
        meshRenderer.material.color = color;
    }

    public void ResetHighlight()
    {
        meshRenderer.material.color = originalColor;
    }

    public bool IsOccupied()
    {
        return occupyingObject != null;
    }

    public void SetObject(GameObject obj)
    {
        occupyingObject = obj;
    }

    public void ClearObject()
    {
        occupyingObject = null;
    }
}