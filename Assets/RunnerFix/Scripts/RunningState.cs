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

        if (_player.timePassed >= _player.speedIncreaseInterval)
        {
            _player.forwardSpeed += _player.speedIncreaseAmount;
            Debug.Log($"New speed: {_player.forwardSpeed}");
            _player.timePassed = 0f;
        }

       _player.transform.position += Vector3.forward * _player.forwardSpeed * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || SwipeManager.DetectSwipeUp())
        {
            _player.SetState(new JumpingState(_player));
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S) || SwipeManager.DetectSwipeDown())
        {
            _player.SetState(new CrouchingState(_player));
<<<<<<< Updated upstream:Assets/RunnerFix/Scripts/RunningState.cs
=======
        }      
    }  
    private void IncreaseSpeed()
    {
        if (_player.timePassed >= _player.speedIncreaseInterval)
        {
            _player.forwardSpeed += _player.speedIncreaseAmount;
            //Debug.Log($"New speed: {_player.forwardSpeed}");
            _player.timePassed = 0f;
>>>>>>> Stashed changes:Assets/RunnerFix/Scripts/Movements/RunningState.cs
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A) || SwipeManager.DetectSwipeLeft())
        {
            if (_player.currentPosition == 0) _player.currentPosition = 1; 
            else if (_player.currentPosition == 2) _player.currentPosition = 0; 
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D) || SwipeManager.DetectSwipeRight())
        {
            if (_player.currentPosition == 0) _player.currentPosition = 2; 
            else if (_player.currentPosition == 1) _player.currentPosition = 0; 
        }

        // Smooth Transition to Lanes
        Vector3 targetPosition = _player.CenterPosition.position;
        if (_player.currentPosition == 1) targetPosition = _player.LeftPosition.position;
        if (_player.currentPosition == 2) targetPosition = _player.RightPosition.position;

        _player.transform.position = Vector3.Lerp(
            _player.transform.position,
            new Vector3(targetPosition.x, _player.transform.position.y, _player.transform.position.z),
            _player.sideSpeed * Time.deltaTime
        );

        
    }  
}
