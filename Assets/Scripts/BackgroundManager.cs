using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BackgroundManager: MonoBehaviour
{
    public static BackgroundManager Instance;
    [SerializeField] private Image InnerBackground;
    [SerializeField] private Image OuterBackground;
    [SerializeField] private Transform spikesParent;
    [SerializeField] private Transform wallsParent;
    [SerializeField] private Color[] innerColors; // 배경 색상 배열
    [SerializeField] private Color[] outerColors; // 배경 색상 배열

    private SpriteRenderer[] spikeRenderers;
    private SpriteRenderer[] wallRenderers;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        spikeRenderers = spikesParent.GetComponentsInChildren<SpriteRenderer>();
        wallRenderers = wallsParent.GetComponentsInChildren<SpriteRenderer>();
    }

    public void SetBackground(int score, int difficultyUnit)
    {
        int index = (score / difficultyUnit) % innerColors.Length;

        if (OuterBackground != null)
        {
            InnerBackground.color = innerColors[index];
            OuterBackground.color = outerColors[index];
            foreach (var spike in spikeRenderers) spike.color = outerColors[index];
            foreach (var wall in wallRenderers) wall.color = outerColors[index];
        }
        else
        {
            Debug.LogWarning("Background Image가 할당되지 않았습니다.");
        }
    }
}