using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.RunnerFix.Scripts
{
    public class IdleState: PlayerState
    {
        public IdleState(PlayerMovement player) : base(player) { }

        public override void Enter()
        {

            _player.playerAnimator.SetBool("isPlaying", false);
        }

        public override void Update()
        {
            if (_player.isPlaying)
            {
                _player.SetState(new RunningState(_player));
            }
        }
    }
}
