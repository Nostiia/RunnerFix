using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private PlayerMovement _playerComponent;
    [SerializeField] private Text _scorePlace;
    private float _lastScoreZ = 0f; 
    private int _score = 0;
    private float _distanceforScore = 10f;

    private void Start()
    {
        _lastScoreZ = _player.position.z; 
    }

    private void Update()
    {
        if (_playerComponent.isPlaying)
        {
            float distanceTraveled = _player.position.z - _lastScoreZ;

            if (distanceTraveled >= _distanceforScore) 
            {
                _score += 1;
                _lastScoreZ = _player.position.z; 
                Debug.Log("Score: " + _score);
                _scorePlace.text = _score.ToString();
            }
        }
    }
    public int GetScore()
    {
        return _score;
    }
}
