using Assets.RunnerFix.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

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

    [field: SerializeField] public Transform CenterPosition { get; private set; }
    [field: SerializeField] public Transform LeftPosition { get; private set; }
    [field: SerializeField] public Transform RightPosition { get; private set; }

    [field: SerializeField] public Animator playerAnimator { get; private set; }
    [field: SerializeField] public Rigidbody playerRigidBody { get; private set; }
    [field: SerializeField] public CapsuleCollider playerCollider { get; private set; }

    [field: SerializeField] public float timePassed = 0f;
    [field: SerializeField] public float expectedRollHeight { get; private set; }
    [field: SerializeField] public float normalHeightCollider { get; private set; }

    private PlayerState _currentState;
    [field: SerializeField] public int currentPosition { get; private set; }



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
        
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A) || SwipeManager.DetectSwipeLeft())
        {
            TurnLeft();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D) || SwipeManager.DetectSwipeRight())
        {
            TurnRight();
        }

        // Smooth Transition to Lanes
        Vector3 targetPosition = CenterPosition.position;
        if (currentPosition == 1) targetPosition = LeftPosition.position;
        if (currentPosition == 2) targetPosition = RightPosition.position;

        transform.position = Vector3.Lerp(
        transform.position,
        new Vector3(targetPosition.x, transform.position.y, transform.position.z),
            sideSpeed * Time.deltaTime
        );
        _currentState?.Update();
    }
    public void TurnRight()
    {
        if (currentPosition == 0) currentPosition = 2;
        else if (currentPosition == 1) currentPosition = 0;
    }
    public void TurnLeft()
    {
        if (currentPosition == 0) currentPosition = 1;
        else if (currentPosition == 2) currentPosition = 0;
    }

    public void SetState(PlayerState _newState)
    {
        _currentState?.Exit();
        _currentState = _newState;
        _currentState.Enter();
    }

}
