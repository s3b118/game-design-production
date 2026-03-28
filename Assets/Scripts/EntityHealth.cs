using System;
using UnityEngine;

public class EntityHealth : MonoBehaviour
{
    [SerializeField] float _maxHealth = 200f;
    [SerializeField] float _startingHealth = 100f;
    [SerializeField] float _healthRegen = 0f;

    private float _currentHealth;

    public Action OnDeath;
    public Action<float, float> OnHealthChanged;

    void Awake()
    {
        _currentHealth = Mathf.Clamp(_startingHealth, 0f, _maxHealth);
    }

    void Start()
    {
        OnHealthChanged?.Invoke(_currentHealth, _maxHealth);
    }

    public void LoseHealth(float amount)
    {
        _currentHealth = Mathf.Clamp(_currentHealth - amount, 0f, _maxHealth);
        OnHealthChanged?.Invoke(_currentHealth, _maxHealth);

        if (_currentHealth <= 0f)
            Death();
    }

    public void GainHealth(float amount)
    {
        _currentHealth = Mathf.Clamp(_currentHealth + amount, 0f, _maxHealth);
        OnHealthChanged?.Invoke(_currentHealth, _maxHealth);
    }

    void HandleHealthRegen()
    {
        GainHealth(_maxHealth * _healthRegen);
    }

    public void Death()
    {
        OnDeath?.Invoke();
    }
}