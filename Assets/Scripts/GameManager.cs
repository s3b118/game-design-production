using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] MainMenuManager _mainMenuManager;
    [SerializeField] InGameUIManager _inGameUIManager;

    public static GameManager Instance { get; private set; }
    public static event Action OnGameStarted;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;

        Time.timeScale = 0f;
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        _inGameUIManager.ShowInGameUI();
        OnGameStarted?.Invoke();
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(0);
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        _inGameUIManager.ShowGameOverPanel();
    }
}