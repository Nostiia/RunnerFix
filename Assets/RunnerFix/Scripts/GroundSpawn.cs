using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSpawn : MonoBehaviour
{
    //TODO: add [SerializeField] private instead of public
    [SerializeField] private GameObject _ground;
    [SerializeField] private Transform _spawnPosition;
    [SerializeField] private Transform _player;

    [SerializeField] private float _timeBetween;
    [SerializeField] private float _lengthOfGround = 100f;

    private float _positionPause = 90f;
    private float _lastPlayerZ;
    void Start()
    {
        _lastPlayerZ = _player.position.z - _positionPause;
        SpawnGround();
    }

    void Update()
    {
        if(_player.position.z > _lastPlayerZ + _lengthOfGround)
        {
            SpawnGround();
            _lastPlayerZ = _player.position.z;
        }
    }
    public void SpawnGround()
    {
        Instantiate(_ground, _spawnPosition.position, Quaternion.identity);
        _spawnPosition.position = new Vector3(_spawnPosition.position.x, _spawnPosition.position.y, _spawnPosition.position.z + _lengthOfGround);
    }
}
