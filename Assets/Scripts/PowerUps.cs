using UnityEngine;

public class PowerUps : MonoBehaviour
{
    public enum PowerUpType
    {
        HealthBoost,
        SpeedBoost,
    }

    [SerializeField] PowerUpType powerUpType;

    [Header("Health Boost Settings")]
    [SerializeField] float healthBoostAmount = 100f;

    [Header("Speed Boost Settings")]
    [SerializeField] float speedMultiplier = 2f;
    [SerializeField] float speedBoostDuration = 20f;

    [Header("Audio")]
    [SerializeField] AudioClip _pickupSFX;
    [SerializeField][Range(0f, 1f)] float _pickupVolume = 1f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent<Player>(out var player)) return;

        switch (powerUpType)
        {
            case PowerUpType.HealthBoost:
                ApplyHealthBoost(player);
                break;

            case PowerUpType.SpeedBoost:
                ApplySpeedBoost(player);
                break;
        }

        PlayPickupSFX();
        Destroy(gameObject);
    }

    private void ApplyHealthBoost(Player player)
    {
        player.Health.GainHealth(healthBoostAmount);
    }

    private void ApplySpeedBoost(Player player)
    {
        player.Movement.ActivateSpeedBoost(speedMultiplier, speedBoostDuration);
    }

    private void PlayPickupSFX()
    {
        if (_pickupSFX == null || AudioManager.Instance == null) return;

        AudioManager.Instance.PlayAudio(_pickupSFX, AudioManager.SoundType.SFX, _pickupVolume, false);
    }
}