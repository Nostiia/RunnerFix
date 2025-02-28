using Assets.RunnerFix.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleInteractor : MonoBehaviour
{
    public void DestroyInteractorObstacle()
    {
        gameObject.SetActive(false);
    }
}
