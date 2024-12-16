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