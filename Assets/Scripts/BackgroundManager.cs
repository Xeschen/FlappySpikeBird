using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BackgroundManager: MonoBehaviour
{
    public static BackgroundManager Instance;
    [SerializeField] private Image image;
    [SerializeField] private Color[] difficultyColors; // 배경 색상 배열

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SetBackground(int difficulty)
    {
        int index = Mathf.Clamp(difficulty, 0, difficultyColors.Length - 1);

        if (image != null)
        {
            image.color = difficultyColors[index];
        }
        else
        {
            Debug.LogWarning("Background Image가 할당되지 않았습니다.");
        }
    }
}