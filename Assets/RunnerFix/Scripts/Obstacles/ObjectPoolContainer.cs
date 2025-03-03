using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolContainer : MonoBehaviour
{
    [SerializeField] private GameObject[] _obstaclePrefabs;
    [SerializeField] private int _poolSize = 10;

    private List<GameObject> _poolList = new List<GameObject>();
    private void Start()
    {
        for (int i = 0; i < _poolSize; i++)
        {
            GameObject obstacleObject = Instantiate(_obstaclePrefabs[Random.Range(0, _obstaclePrefabs.Length)]);
            obstacleObject.SetActive(false);
            _poolList.Add(obstacleObject);
        }
    }
    public GameObject GetPooledObject()
    {
        foreach (GameObject obstacleObject in _poolList)
        {
            if (!obstacleObject.activeInHierarchy)
            {
                return obstacleObject;
            }
        }
        GameObject newObstacleObject = Instantiate(_obstaclePrefabs[Random.Range(0, _obstaclePrefabs.Length)]);
        _poolList.Add(newObstacleObject);
        return newObstacleObject;
    }
}
