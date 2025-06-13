using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void OnScreenTouch()
    {
        switch (GameManager.Instance.CurrentState)
        {
            case GameState.Main:
                GameManager.Instance.StartPlay();
                break;

            case GameState.Play:
                BirdController.Instance?.Jump();
                break;
        }
    }

    public void OnButtonToTitleTouch()
    {
        GameManager.Instance.ToTitle();
    }

    public void OnButtonRetryTouch()
    {
        GameManager.Instance.Retry();
    }

    public void OnButtonResetTouch()
    {
        GameManager.Instance.ResetData();
    }

}