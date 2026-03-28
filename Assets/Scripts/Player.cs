using UnityEngine;

public class Player : MonoBehaviour
{
    public EntityHealth Health { get; private set; }
    public PlayerController Movement { get; private set; }

    private void Awake()
    {
        Health = GetComponent<EntityHealth>();
        Movement = GetComponent<PlayerController>();
    }

    private void OnEnable()
    {
        Health.OnDeath += HandleDeath;
    }

    private void OnDisable()
    {
        Health.OnDeath -= HandleDeath;
    }

    private void HandleDeath()
    {
        GameManager.Instance.GameOver();
    }
}