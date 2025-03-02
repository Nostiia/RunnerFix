using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.RunnerFix.Scripts
{
    public class JumpingState: PlayerState
    {
        public JumpingState(PlayerMovement player) : base(player) { }
        public override void Enter()
        {
            _player.isJump = true;
            _player.playerAnimator.SetBool("isJump", true);
            _player.playerRigidBody.AddForce(Vector3.up * _player.jumpForce, ForceMode.Impulse);
            _player.StartCoroutine(ResetJump());
        }
        public override void Update()
        {
            _player.transform.position += Vector3.forward * _player.forwardSpeed * Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S) || SwipeManager.DetectSwipeDown())
            {
                _player.SetState(new CrouchingState(_player));
            }
        }
        private IEnumerator ResetJump()
        {
            yield return new WaitForSeconds(1.5f);
            _player.isJump = false;
            _player.playerAnimator.SetBool("isJump", false);
            _player.SetState(new RunningState(_player));
        }
    }
    
}
