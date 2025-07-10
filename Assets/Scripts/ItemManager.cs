using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;

    public GameObject itemPrefab;
    public float minY = -3.8f;
    public float maxY = 3.8f;

    private float[] spawnXPositions = new float[] { -2.7f, 2.7f }; // 왼쪽, 오른쪽 위치
    private GameObject pooledItem = null;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        pooledItem = Instantiate(itemPrefab);
        pooledItem.SetActive(false);
    }

    public void SpawnItem(float direction)
    {
        if (pooledItem.activeSelf)
        {
            return;
        }

        float x = direction < 0 ? spawnXPositions[0] : spawnXPositions[1];
        float y = Random.Range(minY, maxY);
        Vector3 spawnPos = new Vector3(x, y, 0);
        pooledItem.transform.position = spawnPos;
        pooledItem.SetActive(true);
    }

    public void ClearItem()
    {
        if (pooledItem != null)
        {
            pooledItem.SetActive(false);
        }
    }
}
