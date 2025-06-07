using System.Collections.Generic;
using UnityEngine;

public class SpikeManager : MonoBehaviour
{
    [Header("Spike Parents")]
    public Transform leftSpikeParent;
    public Transform rightSpikeParent;

    private List<GameObject> leftSpikes = new List<GameObject>();
    private List<GameObject> rightSpikes = new List<GameObject>();

    private void Awake()
    {
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

    private void OnEnable()
    {
        BirdController.OnDirectionChanged += HandleDirectionChanged;
    }

    private void OnDisable()
    {
        BirdController.OnDirectionChanged -= HandleDirectionChanged;
    }

    void HandleDirectionChanged(float direction)
    {
        if (direction > 0)
        {
            ActivateRandomSpike(rightSpikes);
            DeactivateAll(leftSpikes);
        }
        else
        {
            ActivateRandomSpike(leftSpikes);
            DeactivateAll(rightSpikes);
        }
    }

    void ActivateRandomSpike(List<GameObject> spikeList)
    {
        if (spikeList.Count == 0) return;

        // 모든 비활성화
        foreach (var spike in spikeList)
        {
            spike.SetActive(false);
        }

        // 랜덤 하나만 활성화
        int index = Random.Range(0, spikeList.Count);
        spikeList[index].SetActive(true);
    }

    void DeactivateAll(List<GameObject> spikeList)
    {
        foreach (var spike in spikeList)
        {
            spike.SetActive(false);
        }
    }
}