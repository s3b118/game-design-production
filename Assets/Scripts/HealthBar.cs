using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Image _hpBarFill;
    [SerializeField] EntityHealth _playerHealth;

    private void HandleHealthChanged(float currentHealth, float maxHealth)
    {
        _hpBarFill.fillAmount = currentHealth / maxHealth;
    }

    void OnEnable()
    {
        _playerHealth.OnHealthChanged += HandleHealthChanged;
    }

    void OnDisable()
    {
        _playerHealth.OnHealthChanged -= HandleHealthChanged;
    }
}