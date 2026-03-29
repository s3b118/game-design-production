using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    [SerializeField] Enemy[] _enemyPrefabs;

    [Header("Spawn Settings")]
    [SerializeField] float _spawnCooldown = 2f;
    [SerializeField] float _spawnCooldownReductionMult = 0.98f;

    [Header("Ground Tilemap")]
    [SerializeField] Tilemap _groundTiles;

    [Header("Player Reference")]
    [SerializeField] Transform _player;

    [Header("Spawn Restrictions")]
    [SerializeField] float _minPlayerDistance = 2f;

    [Header("Spawn Limits")]
    [SerializeField] int _maxEnemies = 120;

    private float _currentCooldown;
    private int _currentEnemyCount;
    private readonly List<Vector3> _spawnPositions = new();
    private readonly HashSet<Vector3> _occupiedPositions = new();

    private void OnEnable()
    {
        GameManager.OnGameStarted += OnGameStarted;
    }

    private void OnDisable()
    {
        GameManager.OnGameStarted -= OnGameStarted;
    }

    private void OnGameStarted()
    {
        CacheSpawnPositions();
        InvokeRepeating(nameof(HandleGameDifficultyIncrease), 1f, 1f);
    }

    private void Update()
    {
        HandleEnemySpawning();
    }

    private void CacheSpawnPositions()
    {
        _spawnPositions.Clear();

        foreach (Vector3Int position in _groundTiles.cellBounds.allPositionsWithin)
        {
            if (_groundTiles.HasTile(position))
                _spawnPositions.Add(_groundTiles.GetCellCenterWorld(position));
        }
    }

    private void HandleEnemySpawning()
    {
        if (_currentEnemyCount >= _maxEnemies)
            return;

        _currentCooldown -= Time.deltaTime;

        if (_currentCooldown > 0f)
            return;

        _currentCooldown = _spawnCooldown;
        SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        if (!TryGetRandomFreePosition(out Vector3 spawnPos))
            return;

        Enemy enemy = Instantiate(GetRandomEnemyPrefab(), spawnPos, Quaternion.identity);

        _occupiedPositions.Add(spawnPos);
        _currentEnemyCount++;

        var tracker = enemy.gameObject.AddComponent<SpawnTileTracker>();
        tracker.Initialize(spawnPos, _occupiedPositions);

        EntityHealth health = enemy.GetComponent<EntityHealth>();
        if (health != null)
        {
            health.OnDeath += () => _currentEnemyCount--;
        }
    }

    private bool TryGetRandomFreePosition(out Vector3 result)
    {
        List<Vector3> validPositions = new();

        foreach (Vector3 pos in _spawnPositions)
        {
            if (_occupiedPositions.Contains(pos))
                continue;

            if (_player != null && Vector3.Distance(pos, _player.position) < _minPlayerDistance)
                continue;

            validPositions.Add(pos);
        }

        if (validPositions.Count == 0)
        {
            result = default;
            return false;
        }

        result = validPositions[Random.Range(0, validPositions.Count)];
        return true;
    }

    private Enemy GetRandomEnemyPrefab()
    {
        return _enemyPrefabs[Random.Range(0, _enemyPrefabs.Length)];
    }

    private void HandleGameDifficultyIncrease()
    {
        _spawnCooldown *= _spawnCooldownReductionMult;
    }
}