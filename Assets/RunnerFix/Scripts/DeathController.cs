using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.RunnerFix.Scripts
{
    public class DeathController : MonoBehaviour
    {
        [SerializeField] private ScoreManager _scoreManager;
        [SerializeField] private LoseScreenUIManager _loseScreenUIManager;
        private ObstacleInteractor _interactor;
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<ObstacleInteractor>(out _interactor))
            {
                Debug.Log($"Lose Score: {_scoreManager.GetScore()}");
                Time.timeScale = 0f;
                _loseScreenUIManager.ActivateLoseScreen();
            }
        }
        public ObstacleInteractor GetObstacle()
        {
            return _interactor;
        }
    }
}
