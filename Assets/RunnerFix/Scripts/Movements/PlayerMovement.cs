using Assets.RunnerFix.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//state machine for movement,
public class PlayerMovement : MonoBehaviour
{
    internal float forwardSpeed;
    internal float sideSpeed = 3f;
    internal float jumpForce = 7f;
    internal float speedIncreaseInterval = 5f;
    internal float speedIncreaseAmount = 1f;
    internal float initialSpeed = 3f;
    internal bool isPlaying = false;
    internal bool isJump = false;
    internal bool isCrunch = false;

    public Transform CenterPosition, LeftPosition, RightPosition;

    internal Animator playerAnimator;
    internal Rigidbody playerRigidBody;
    internal CapsuleCollider playerCollider;

    internal float timePassed = 0f;
    internal float expectedRollHeight;
    internal float normalHeightCollider;

    private PlayerState _currentState;
    internal int currentPosition;



    private void Start()
    {
        playerAnimator = GetComponent<Animator>();
        playerRigidBody = GetComponent<Rigidbody>();
        playerCollider = GetComponent<CapsuleCollider>();
        normalHeightCollider = playerCollider.height;
        expectedRollHeight = normalHeightCollider / 2;
        forwardSpeed = initialSpeed;

        SetState(new IdleState(this));
    }

    private void Update()
    {
        _currentState?.Update();
    }

    public void SetState(PlayerState _newState)
    {
        _currentState?.Exit();
        _currentState = _newState;
        _currentState.Enter();
    }

}
