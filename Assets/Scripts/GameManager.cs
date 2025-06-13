using UnityEngine;
using UnityEngine.UI;

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
    }

    public void ToTitle()
    {
        SetState(GameState.Main);
    }

    public void Retry()
    {
        SetState(GameState.Play);
        IncrementPlayCount();
    }

    public void ResetData()
    {
        highScore = 0;
        playCount = 0;
        PlayerPrefs.SetInt(HighScoreKey, highScore);
        PlayerPrefs.SetInt(PlayCountKey, playCount);
        UpdateScoreUI();
    }

    public void EndGame()
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