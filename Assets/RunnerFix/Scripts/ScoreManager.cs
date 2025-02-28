using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Transform Player;
    public PlayerMovement PlayerComponent;
    public Text ScorePlace;
    private float _lastScoreZ = 0f; 
    private int _score = 0;
    private float _distanceforScore = 10f;

    void Start()
    {
        _lastScoreZ = Player.position.z; 
    }

    void Update()
    {
        if (PlayerComponent.isPlaying)
        {
            float distanceTraveled = Player.position.z - _lastScoreZ;

            if (distanceTraveled >= _distanceforScore) 
            {
                _score += 1;
                _lastScoreZ = Player.position.z; 
                Debug.Log("Score: " + _score);
                ScorePlace.text = _score.ToString();
            }
        }
    }
    public int GetScore()
    {
        return _score;
    }
}
