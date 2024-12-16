// PlayerController.cs
public class PlayerController : MonoBehaviour
{
    private GridCell currentCell;
    private Monster targetMonster;
    private bool isMoving = false;
    public float moveSpeed = 5f;

    void Start()
    {
        Vector3 pos = transform.position;
        currentCell = DungeonManager.Instance.GetCell(
            Mathf.RoundToInt(pos.x),
            Mathf.RoundToInt(pos.z)
        );
        currentCell.SetObject(gameObject);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isMoving)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Monster monster = hit.collider.GetComponent<Monster>();
                if (monster != null)
                {
                    Debug.Log("Monster clicked!");
                    targetMonster = monster;
                    StartCoroutine(MoveToTarget(monster.CurrentCell));
                }
            }
        }
    }

    private IEnumerator MoveToTarget(GridCell targetCell)
    {
        if (isMoving || targetCell == null) yield break;
        isMoving = true;

        List<GridCell> path = FindPathToTarget(targetCell);
        
        if (path.Count > 0)
        {
            foreach (GridCell cell in path)
            {
                Vector3 targetPosition = new Vector3(cell.x, transform.position.y, cell.y);
                
                while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
                {
                    transform.position = Vector3.MoveTowards(
                        transform.position,
                        targetPosition,
                        moveSpeed * Time.deltaTime
                    );
                    yield return null;
                }

                currentCell.ClearObject();
                currentCell = cell;
                currentCell.SetObject(gameObject);
            }
        }

        isMoving = false;
    }

    // 구현 필요
    private List<GridCell> FindPathToTarget(GridCell targetCell)
    {
        return new List<GridCell>();
    }
}