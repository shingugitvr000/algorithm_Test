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

    private Monster FindNearestMonster()
    {
        Monster nearestMonster = null;
        float nearestDistance = fleeRange;

        foreach (Monster monster in FindObjectsOfType<Monster>())
        {
            float distance = Vector3.Distance(transform.position, monster.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestMonster = monster;
            }
        }

        return nearestMonster;
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

    private float CalculateDanger(GridCell cell, Monster monster)
    {
        float danger = cell.movementCost;
        
        float distanceToMonster = Vector3.Distance(
            new Vector3(cell.x, 0, cell.y),
            monster.transform.position
        );
        danger += Mathf.Max(0, fleeRange - distanceToMonster) * 2;

        if (cell.cellType == CellType.Trap)
            danger *= 1.5f;

        return danger;
    }

    public GridCell CurrentCell => currentCell;
}