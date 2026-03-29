using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PowerUpsSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] GameObject _healthPowerUpPrefab;
    [SerializeField] GameObject _speedPowerUpPrefab;

    [Header("Spawn Counts")]
    [SerializeField] int _healthPowerUpCount = 5;
    [SerializeField] int _speedPowerUpCount = 5;

    [Header("Ground Tilemap")]
    [SerializeField] Tilemap _groundTiles;

    [Header("Player Reference")]
    [SerializeField] Transform _player;

    [Header("Spawn Restrictions")]
    [SerializeField] float _minPlayerDistance = 2f;

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
        SpawnPowerUps(_healthPowerUpPrefab, _healthPowerUpCount);
        SpawnPowerUps(_speedPowerUpPrefab, _speedPowerUpCount);
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

    private void SpawnPowerUps(GameObject prefab, int count)
    {
        if (prefab == null)
        {
            Debug.LogWarning($"[PowerUpsSpawner] Prefab is null — skipping {count} spawns.");
            return;
        }

        for (int i = 0; i < count; i++)
        {
            if (!TryGetRandomFreePosition(out Vector3 spawnPos))
            {
                Debug.LogWarning($"[PowerUpsSpawner] No free position found. Skipping remaining spawns for {prefab.name}.");
                break;
            }

            GameObject instance = Instantiate(prefab, spawnPos, Quaternion.identity);

            _occupiedPositions.Add(spawnPos);

            var tracker = instance.AddComponent<SpawnTileTracker>();
            tracker.Initialize(spawnPos, _occupiedPositions);
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
}