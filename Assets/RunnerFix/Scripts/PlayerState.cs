using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.RunnerFix.Scripts
{
    public abstract  class PlayerState
    {
        protected PlayerMovement _player;
        public PlayerState(PlayerMovement player)
        {
            _player = player;
        }
        public virtual void Enter() { }
        public virtual void Update() { }
        public virtual void Exit() { }

    }
}
