using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpikeManager : MonoBehaviour
{
    public static SpikeManager Instance;

    public int maxSpikes = 5; // 최대 활성화 스파이크 수

    [Header("Spike Parents")]
    public Transform leftSpikeParent;
    public Transform rightSpikeParent;

    private List<GameObject> leftSpikes = new List<GameObject>();
    private List<GameObject> rightSpikes = new List<GameObject>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // 하위 오브젝트 수집
        foreach (Transform child in leftSpikeParent)
        {
            leftSpikes.Add(child.gameObject);
            child.gameObject.SetActive(false);
        }

        foreach (Transform child in rightSpikeParent)
        {
            rightSpikes.Add(child.gameObject);
            child.gameObject.SetActive(false);
        }
    }

    public void ActivateSpike(int direction, int difficulty)
    {
        var count = Mathf.Min(difficulty, maxSpikes); // 현재 스코어에 따라 활성화할 스파이크 수

        if (direction > 0)
        {
            ActivateRandomSpike(rightSpikes, count);
            DeactivateAll(leftSpikes);
        }
        else
        {
            ActivateRandomSpike(leftSpikes, count);
            DeactivateAll(rightSpikes);
        }
    }

    void ActivateRandomSpike(List<GameObject> spikeList, int count)
    {
        if (spikeList.Count == 0) return;

        // 모든 비활성화
        foreach (var spike in spikeList)
        {
            spike.SetActive(false);
        }

        foreach (var spike in spikeList.OrderBy(u => Random.value).Take(Mathf.Min(count, spikeList.Count)))
        {
            spike.SetActive(true); // 랜덤으로 선택된 스파이크 활성화
        }
    }

    void DeactivateAll(List<GameObject> spikeList)
    {
        foreach (var spike in spikeList)
        {
            spike.SetActive(false);
        }
    }
}