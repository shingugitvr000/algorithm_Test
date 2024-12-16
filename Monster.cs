// Monster.cs
public class Monster : MonoBehaviour
{
    private GridCell currentCell;

    void Start()
    {
        Vector3 pos = transform.position;
        currentCell = DungeonManager.Instance.GetCell(
            Mathf.RoundToInt(pos.x),
            Mathf.RoundToInt(pos.z)
        );
        currentCell.SetObject(gameObject);
    }

    public GridCell CurrentCell => currentCell;
}

// Item.cs
public class Item : MonoBehaviour
{
    private GridCell currentCell;
    public bool isCollected = false;

    void Start()
    {
        Vector3 pos = transform.position;
        currentCell = DungeonManager.Instance.GetCell(
            Mathf.RoundToInt(pos.x),
            Mathf.RoundToInt(pos.z)
        );
        currentCell.SetObject(gameObject);
    }

    public void Collect()
    {
        if (!isCollected)
        {
            isCollected = true;
            currentCell.ClearObject();
            StartCoroutine(CollectEffect());
        }
    }

    private IEnumerator CollectEffect()
    {
        float duration = 0.5f;
        float elapsedTime = 0f;
        Vector3 startScale = transform.localScale;
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + Vector3.up * 2f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            transform.localScale = Vector3.Lerp(startScale, Vector3.zero, t);
            transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

        Destroy(gameObject);
    }

    public GridCell CurrentCell => currentCell;
}