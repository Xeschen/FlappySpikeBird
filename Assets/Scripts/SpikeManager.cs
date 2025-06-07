using UnityEngine;

public class SpikeManager : MonoBehaviour
{
    [Header("Spike GameObjects")]
    public GameObject leftSpike;
    public GameObject rightSpike;

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
            // 오른쪽으로 이동 → 오른쪽 가시 활성화, 왼쪽 비활성화
            rightSpike.SetActive(true);
            leftSpike.SetActive(false);
        }
        else
        {
            // 왼쪽으로 이동 → 왼쪽 가시 활성화, 오른쪽 비활성화
            leftSpike.SetActive(true);
            rightSpike.SetActive(false);
        }
    }
}