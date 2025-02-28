using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesDespawner : MonoBehaviour
{
    [SerializeField] private float _despawnDistance = 10f;
    void Update()
    {
        if(transform.position.z < Camera.main.transform.position.z - _despawnDistance)
        {
            gameObject.SetActive(false);
        }
        
    }
}
