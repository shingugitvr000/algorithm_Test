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

// ItemCollector.cs
public class ItemCollector : MonoBehaviour
{
    public float collectionRange = 3f;
    private bool isCollecting = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isCollecting)
        {
            StartCoroutine(CollectItemsInSequence());
        }
    }

    // 구현 필요
    private IEnumerator CollectItemsInSequence()
    {
        if (isCollecting) yield break;
        isCollecting = true;

        yield return null;
        isCollecting = false;
    }
}

// NPC.cs
public class NPC : MonoBehaviour
{
    private GridCell currentCell;
    public float fleeRange = 5f;
    public float moveSpeed = 4f;
    private bool isMoving = false;

    void Start()
    {
        Vector3 pos = transform.position;
        currentCell = DungeonManager.Instance.GetCell(
            Mathf.RoundToInt(pos.x),
            Mathf.RoundToInt(pos.z)
        );
        currentCell.SetObject(gameObject);
        StartCoroutine(CheckForDanger());
    }

    private IEnumerator CheckForDanger()
    {
        while (true)
        {
            Monster nearestMonster = FindNearestMonster();
            if (nearestMonster != null && !isMoving)
            {
                StartCoroutine(FleeFromMonster(nearestMonster));
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    private IEnumerator FleeFromMonster(Monster monster)
    {
        if (isMoving) yield break;
        isMoving = true;

        List<GridCell> escapePath = FindEscapePath(monster);
        
        if (escapePath != null && escapePath.Count > 0)
        {
            foreach (GridCell cell in escapePath)
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
    private List<GridCell> FindEscapePath(Monster monster)
    {
        return new List<GridCell>();
    }

    public GridCell CurrentCell => currentCell;
}