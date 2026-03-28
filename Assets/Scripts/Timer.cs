using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [Header("Timer UI text")]
    [SerializeField] private TMP_Text _timerText;

    private const float StartTime = 120f;
    private float _remainingTime;
    private int _lastDisplayedSecond;
    private bool _gameOverCalled;

    void Start()
    {
        _remainingTime = StartTime;
        _lastDisplayedSecond = -1;
        _gameOverCalled = false;

        if (_timerText == null)
        {
            Debug.LogWarning("Timer text not assigned in inspector.");
        }

        UpdateTimerUI(Mathf.CeilToInt(_remainingTime));
    }

    void Update()
    {
        if (_gameOverCalled) return;

        _remainingTime -= Time.deltaTime;

        if (_remainingTime <= 0f)
        {
            _remainingTime = 0f;
            UpdateTimerUI(0);

            _gameOverCalled = true;
            GameManager.Instance.GameOver();
            return;
        }

        int currentSecond = Mathf.CeilToInt(_remainingTime);

        if (currentSecond != _lastDisplayedSecond)
        {
            _lastDisplayedSecond = currentSecond;
            UpdateTimerUI(currentSecond);
        }
    }

    void UpdateTimerUI(int totalSeconds)
    {
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;

        _timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}