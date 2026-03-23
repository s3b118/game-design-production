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

    float _currentCooldown;

    [Header("Ground Tilemap")]
    [SerializeField] Tilemap _groundTiles;

    [Header("Player Reference")]
    [SerializeField] Transform _player;

    [Header("Spawn Restrictions")]
    [SerializeField] float _minPlayerDistance = 2f;

    [Header("Spawn Limits")]
    [SerializeField] int _maxEnemies = 120;
    int _currentEnemyCount = 0;

    List<Vector3> _spawnPositions = new();
    HashSet<Vector3> _occupiedPositions = new();

    void Start()
    {
        SetEnemySpawnPositions();
        InvokeRepeating(nameof(HandleGameDifficultyIncrease), 1f, 1f);
    }

    void SetEnemySpawnPositions()
    {
        foreach (Vector3Int position in _groundTiles.cellBounds.allPositionsWithin)
        {
            if (_groundTiles.HasTile(position))
            {
                _spawnPositions.Add(_groundTiles.GetCellCenterWorld(position));
            }
        }
    }

    void HandleEnemySpawning()
    {
        if (_currentEnemyCount >= _maxEnemies)
            return;

        _currentCooldown -= Time.deltaTime;

        if (_currentCooldown > 0f)
            return;

        _currentCooldown = _spawnCooldown;
        SpawnEnemyToFreeLocation();
    }

    bool TryGetRandomFreePosition(out Vector3 result)
    {
        List<Vector3> valid = new();

        foreach (var pos in _spawnPositions)
        {
            if (_occupiedPositions.Contains(pos))
                continue;

            if (_player != null && Vector3.Distance(pos, _player.position) < _minPlayerDistance)
                continue;

            valid.Add(pos);
        }

        if (valid.Count == 0)
        {
            result = default;
            return false;
        }

        result = valid[Random.Range(0, valid.Count)];
        return true;
    }

    void SpawnEnemyToFreeLocation()
    {
        if (!TryGetRandomFreePosition(out Vector3 spawnPos))
            return;

        Enemy enemyPrefab = GetRandomEnemyPrefab();
        Enemy enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

        _occupiedPositions.Add(spawnPos);
        _currentEnemyCount++;

        var tracker = enemy.gameObject.AddComponent<SpawnTileTracker>();
        tracker.Initialize(spawnPos, _occupiedPositions);

        EntityHealth health = enemy.GetComponent<EntityHealth>();
        if (health != null)
        {
            health.OnDeath += () =>
            {
                _currentEnemyCount--;
                _occupiedPositions.Remove(spawnPos);
            };
        }
    }

    Enemy GetRandomEnemyPrefab()
    {
        return _enemyPrefabs[Random.Range(0, _enemyPrefabs.Length)];
    }

    void HandleGameDifficultyIncrease()
    {
        _spawnCooldown *= _spawnCooldownReductionMult;
    }

    void Update()
    {
        HandleEnemySpawning();
    }
}
