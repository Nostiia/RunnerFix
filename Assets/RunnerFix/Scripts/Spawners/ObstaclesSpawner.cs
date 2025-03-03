using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesSpawner : MonoBehaviour
{
    [SerializeField] private ObjectPoolContainer _oblectPool;
    [SerializeField] private Transform _player;

    [SerializeField] private float _spawnInterval = 7f;

    [SerializeField] private float _mindistanceAheadPlayer = 50f;
    [SerializeField] private float _maxdistanceAheadPlayer = 70f;

    private float _nextSpawningTime = 0f;

    [SerializeField] private float[] _lanes = new float[] { -3f, 0f, 3f }; 
    private int _lastSpawnedLane = 0;
    private bool _gameIsStrated = false;

    private float _secondWithoutObstacles = 5f;
     private void StartSpawning()
    {
        _gameIsStrated = true;
    }

    private void Update()
    {
        PlayerMovement _playerController = _player.GetComponent<PlayerMovement>();
        if (_playerController.isPlaying)
        {
            Invoke(nameof(StartSpawning), _secondWithoutObstacles);
        }
        if (_gameIsStrated && Time.time >= _nextSpawningTime)
        {
            SpawnObstacles();
            AdjustSpawnRace();
        }
    }
    private void SpawnObstacles()
    {
        GameObject _obstacle = _oblectPool.GetPooledObject();
        if (_obstacle != null)
        {
            int _laneIndex;
            do
            {
                _laneIndex = Random.Range(0, _lanes.Length);
            } while (_laneIndex == _lastSpawnedLane);
            _lastSpawnedLane = _laneIndex;
            float _spawnZ = _player.position.z + Random.Range(_mindistanceAheadPlayer, _maxdistanceAheadPlayer);
            _obstacle.transform.position = new Vector3(_lanes[_laneIndex], 0, _spawnZ);
            _obstacle.SetActive(true);
        }
        _nextSpawningTime = Time.time + _spawnInterval;
    }
    private void AdjustSpawnRace()
    {
        float _playerSpeed = _player.GetComponent<PlayerMovement>().forwardSpeed;
        _spawnInterval = Mathf.Max(0.5f, 2f - (_playerSpeed / 20f));
    }
}
