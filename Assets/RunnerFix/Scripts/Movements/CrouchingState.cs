using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.RunnerFix.Scripts
{
    public class CrouchingState: PlayerState
    {
        public CrouchingState(PlayerMovement player) : base(player) { }
        public override void Enter()
        {
            _player.isCrunch = true;
            _player.playerAnimator.SetBool("isCrounch", true);
            _player.playerCollider.height = _player.expectedRollHeight;
            _player.playerCollider.center = new Vector3(_player.playerCollider.center.x, _player.expectedRollHeight / 2, _player.playerCollider.center.z);
            _player.StartCoroutine(ResetCrunch());
        }
        public override void Update()
        {
            _player.transform.position += Vector3.forward * _player.forwardSpeed * Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || SwipeManager.DetectSwipeUp())
            {
                _player.SetState(new JumpingState(_player));
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A) || SwipeManager.DetectSwipeLeft())
            {
                _player.TurnLeft();
            }

            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D) || SwipeManager.DetectSwipeRight())
            {
                _player.TurnRight();
            }
        }
        private IEnumerator ResetCrunch()
        {
            yield return new WaitForSeconds(1.167f);
            _player.isCrunch = false;
            _player.playerAnimator.SetBool("isCrounch", _player.isCrunch);
            _player.playerCollider.height = _player.normalHeightCollider;
            _player.playerCollider.center = new Vector3(_player.playerCollider.center.x, _player.normalHeightCollider / 2, _player.playerCollider.center.z);
            _player.SetState(new RunningState(_player));
        }
    }
}
