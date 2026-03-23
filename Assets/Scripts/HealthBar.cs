using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Image _hpBarFill;
    [SerializeField] EntityHealth _playerHealth;

    public void OnHealthChanged(float currentHealth, float maxHealth)
    {
        _hpBarFill.fillAmount = currentHealth / maxHealth;
    }

    void OnEnable()
    {
        _playerHealth.OnHealthChanged += OnHealthChanged;
    }

    void OnDisable()
    {
        _playerHealth.OnHealthChanged -= OnHealthChanged;
    }
}