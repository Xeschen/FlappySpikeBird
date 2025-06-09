using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    public int score = 0;
    public Text scoreText; // UI 텍스트 오브젝트 연결

    private GameState currentState;
    [SerializeField] private GameObject mainUI;
    [SerializeField] private GameObject playUI;
    [SerializeField] private GameObject endUI;

    private void Awake()
    {
        // Singleton 패턴
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject); // 씬 전환 시 유지 (선택)
    }

    void Start()
    {
        SetState(GameState.Main);
    }


    public void AddScore(int value)
    {
        score += value;
        UpdateScoreUI();
    }

    public void ResetScore()
    {
        score = 0;
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }
    }

    void SetState(GameState state)
    {
        if (currentState == GameState.End && state == GameState.Play) playUI.SetActive(false);
        currentState = state;

        mainUI.SetActive(currentState == GameState.Main);
        endUI.SetActive(currentState == GameState.End);
        playUI.SetActive(currentState == GameState.Play || currentState == GameState.End);
    }

    public void OnStartButtonClicked()
    {
        SetState(GameState.Play);
    }

    public void OnToTitleButtonClicked()
    {
        SetState(GameState.Main);
    }

    public void OnRetryButtonClicked()
    {
        SetState(GameState.Play);
    }

    public void EndGame()
    {
        SetState(GameState.End);
    }
}

public enum GameState
{
    Main,
    Play,
    End
}