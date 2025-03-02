using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform _player;
    private float _cameraPositionZ = -1.45f;

    private void LateUpdate()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, _player.transform.position.z + _cameraPositionZ);
    }
}
