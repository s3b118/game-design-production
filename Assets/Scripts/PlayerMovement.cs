using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 5f;
    [SerializeField] SpriteRenderer _characterBody;
    [SerializeField] Animator _animator;

    private float _baseMoveSpeed;
    private Rigidbody2D _rb;
    private Vector2 _movement;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _baseMoveSpeed = _moveSpeed;
    }

    void Update()
    {
        _movement.x = Input.GetAxisRaw("Horizontal");
        _movement.y = Input.GetAxisRaw("Vertical");
        _movement = _movement.normalized;

        _animator.SetBool("isWalking", _movement.magnitude > 0f);

        if (_movement.x != 0f)
            _characterBody.flipX = _movement.x < 0f;
    }

    void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + _movement * _moveSpeed * Time.fixedDeltaTime);
    }

    public void ActivateSpeedBoost(float multiplier, float duration)
    {
        StopAllCoroutines();
        StartCoroutine(SpeedBoostRoutine(multiplier, duration));
    }

    private IEnumerator SpeedBoostRoutine(float multiplier, float duration)
    {
        _moveSpeed = _baseMoveSpeed * multiplier;
        yield return new WaitForSeconds(duration);
        _moveSpeed = _baseMoveSpeed;
    }
}