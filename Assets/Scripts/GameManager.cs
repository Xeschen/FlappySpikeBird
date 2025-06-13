using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class GameManager : MonoBehaviour
{
    private const string PlayCountKey = "PlayCount";
    private const string HighScoreKey = "HighScore";

    public static GameManager Instance;
    public GameState CurrentState => currentState;

    public Text scoreText;
    public Text highScoreText;
    public Text playCountText;
    public Text endHighScoreText;
    public Text endBestScoreText;
    private bool isBestScore = false;

    public int score = 0;
    public int difficultyUnit = 5;
    private int highScore;
    private int playCount;

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
    }

    void Start()
    {
        SetState(GameState.Main);
        LoadGameData();
        UpdateScoreUI();
    }

    private void OnEnable()
    {
        BirdController.OnDirectionChanged += AddScore;
        BirdController.OnDead+= EndPlay;
    }

    private void OnDisable()
    {
        BirdController.OnDirectionChanged -= AddScore;
        BirdController.OnDead -= EndPlay;
    }

    private void Update()
    {
        BackgroundManager.Instance.SetBackground(score, difficultyUnit);
    }

    public void AddScore()
    {
        score += 1;
        UpdateScoreUI();
        AudioManager.Instance.PlayBounce(); // Play bounce sound
        SpikeManager.Instance.ActivateSpike((int)BirdController.Instance.direction, score / difficultyUnit);
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

        if (highScoreText != null)
        {
            highScoreText.text = "BEST SCORE: " + highScore.ToString();
        }

        if (playCountText!= null)
        {
            playCountText.text = "GAMES PLAYED: " + playCount.ToString();
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

    public void StartPlay()
    {
        SetState(GameState.Play);
        IncrementPlayCount();
        SpikeManager.Instance.ActivateSpike(1, 0);

#if UNITY_EDITOR
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
#endif
    }

    public void ToTitle()
    {
        SetState(GameState.Main);
        BackgroundManager.Instance.SetBackground(0, difficultyUnit);
    }

    public void Retry()
    {
        SetState(GameState.Play);
        IncrementPlayCount();
        SpikeManager.Instance.ActivateSpike(1, 0);

#if UNITY_EDITOR
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
#endif
    }

    public void ResetData()
    {
        highScore = 0;
        playCount = 0;
        PlayerPrefs.SetInt(HighScoreKey, highScore);
        PlayerPrefs.SetInt(PlayCountKey, playCount);
        UpdateScoreUI();
    }

    public void EndPlay()
    {
        SetState(GameState.End);
        
        isBestScore = highScore < score;
        highScore = Mathf.Max(highScore, score);
        PlayerPrefs.SetInt(HighScoreKey, highScore);

        if (endHighScoreText != null)
        {
            endHighScoreText.text = "BEST SCORE: " + highScore.ToString();
        }

        if (endBestScoreText!= null)
        {
            if (isBestScore)
            {
                endHighScoreText.text = "";
                endBestScoreText.text = "NEW BEST SCORE!!";
            }
            else
            {
                endBestScoreText.text = "";
            }
        }

        AudioManager.Instance.PlayGameEnd(); // Play game end sound

#if UNITY_EDITOR
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
#endif
    }

    private void IncrementPlayCount()
    {
        playCount += 1;
        PlayerPrefs.SetInt(PlayCountKey, playCount);
    }

    private void LoadGameData()
    {
        highScore = PlayerPrefs.GetInt(HighScoreKey, 0);
        playCount = PlayerPrefs.GetInt(PlayCountKey, 0);
    }
}

public enum GameState
{
    Main,
    Play,
    End
}