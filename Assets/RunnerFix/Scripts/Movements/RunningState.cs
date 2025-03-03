using Assets.RunnerFix.Scripts;
using UnityEngine;

public class RunningState : PlayerState
{
    // Swipe Detection Variables
    private Vector2 _touchStartPos;
    private bool _swipeDetected;
    public RunningState(PlayerMovement player) : base(player) { }

    public override void Enter()
    {
        _player.isPlaying = true;
        _player.playerAnimator.SetBool("isPlaying", true);
    }

    public override void Update()
    {
        _player.timePassed += Time.deltaTime;
        IncreaseSpeed();

       _player.transform.position += Vector3.forward * _player.forwardSpeed * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || SwipeManager.DetectSwipeUp())
        {
            _player.SetState(new JumpingState(_player));
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S) || SwipeManager.DetectSwipeDown())
        {
            _player.SetState(new CrouchingState(_player));
        }      
    }  
    private void IncreaseSpeed()
    {
        if (_player.timePassed >= _player.speedIncreaseInterval)
        {
            _player.forwardSpeed += _player.speedIncreaseAmount;
            Debug.Log($"New speed: {_player.forwardSpeed}");
            _player.timePassed = 0f;
        }
    }
}
