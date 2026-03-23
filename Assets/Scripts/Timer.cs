using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [Header("Timer UI text")]
    [SerializeField] private TMP_Text _timerText;

    private float _elapsedTime;
    private int _lastDisplayedSecond;

    void Start()
    {
        _elapsedTime = 0f;
        _lastDisplayedSecond = -1;

        if (_timerText == null)
        {
            Debug.LogWarning("Timer text not assigned in inspector.");
        }
    }

    void Update()
    {
        _elapsedTime += Time.deltaTime;
        int currentSecond = Mathf.FloorToInt(_elapsedTime);

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
