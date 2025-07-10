using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private const string PlayCountKey = "PlayCount";
    private const string HighScoreKey = "HighScore";

    public static GameManager Instance;
    public GameState CurrentState => currentState;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI mainBestScoreText;
    public TextMeshProUGUI mainPlayCountText;
    public TextMeshProUGUI endBestScoreText;
    public TextMeshProUGUI endNewBestScoreText;
    private bool isBestScore = false;

    public int score = 0;
    public int difficultyUnit = 5;
    private int highScore;
    private int playCount;

    private GameState currentState;
    [SerializeField] private GameObject mainUI;
    [SerializeField] private GameObject playUI;
    [SerializeField] private GameObject endUI;
    [SerializeField] private GameObject leftSpikes;
    [SerializeField] private GameObject rightSpikes;

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
        ResetScore();
        UpdateScoreUI();
    }

    private void OnEnable()
    {
        BirdController.OnDirectionChanged += HitWall;
        BirdController.OnDead+= EndPlay;
    }

    private void OnDisable()
    {
        BirdController.OnDirectionChanged -= HitWall;
        BirdController.OnDead -= EndPlay;
    }

    private void Update()
    {
        BackgroundManager.Instance.SetBackground(score, difficultyUnit);
    }

    public void HitWall()
    {
        score += 1;
        UpdateScoreUI();
        AudioManager.Instance.PlayBounce(); // Play bounce sound
        SpikeManager.Instance.ActivateSpike((int)BirdController.Instance.direction, score / difficultyUnit);
        ItemManager.Instance.SpawnItem(BirdController.Instance.direction);
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

        if (mainBestScoreText != null)
        {
            mainBestScoreText.text = "BEST SCORE: " + highScore.ToString();
        }

        if (mainPlayCountText!= null)
        {
            mainPlayCountText.text = "GAMES PLAYED: " + playCount.ToString();
        }
    }

    void SetState(GameState state)
    {
        if (state == GameState.Main)
        {
            mainUI.SetActive(true);

            playUI.SetActive(false);
            leftSpikes.SetActive(false);
            rightSpikes.SetActive(false);
            BirdController.Instance.SetPlayable(false);

            endUI.SetActive(false);
        }
        else if (state == GameState.Play)
        {
            if (currentState == GameState.Main)
            {
                mainUI.SetActive(false);

                playUI.SetActive(true);
                leftSpikes.SetActive(true);
                rightSpikes.SetActive(true);
                BirdController.Instance.SetPlayable(true);
            }
            else // if (currentState == GameState.End)
            {
                endUI.SetActive(false);
                BirdController.Instance.SetPlayable(true);
            }
        }
        else // if (state == GameState.End)
        {
            endUI.SetActive(true);
        }

        currentState = state;
    }

    public void StartPlay()
    {
        ResetScore();
        SetState(GameState.Play);
        IncrementPlayCount();
        SpikeManager.Instance.ActivateSpike(1, 0);
        ItemManager.Instance.SpawnItem(1); // Spawn item at the start

        /*#if UNITY_EDITOR
                Cursor.lockState = CursorLockMode.Confined;
        #endif
        */
    }

    public void ToTitle()
    {
        SetState(GameState.Main);
        BackgroundManager.Instance.SetBackground(0, difficultyUnit);
    }

    public void Retry()
    {
        ResetScore();
        SetState(GameState.Play);
        IncrementPlayCount();
        SpikeManager.Instance.ActivateSpike(1, 0);

/*#if UNITY_EDITOR
        Cursor.lockState = CursorLockMode.Confined;
#endif
*/    }

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

        if (endBestScoreText != null)
        {
            endBestScoreText.text = "BEST SCORE: " + highScore.ToString();
        }

        if (endNewBestScoreText!= null)
        {
            if (isBestScore)
            {
                endBestScoreText.text = "";
                endNewBestScoreText.text = "NEW BEST SCORE!!";
            }
            else
            {
                endNewBestScoreText.text = "";
            }
        }

        AudioManager.Instance.PlayGameEnd(); // Play game end sound

/*#if UNITY_EDITOR
        Cursor.lockState = CursorLockMode.None;
#endif
*/    }

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