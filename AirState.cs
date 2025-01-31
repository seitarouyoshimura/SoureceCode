using System;
using UnityEngine;

// �󒆏�Ԃ̐���
namespace HA.Yokohama.Beyond
{
    partial class Player
    {
        [Serializable]
        protected class AirState : StateBase<Player>
        {
            public override void OnEnter(Player component, IState<Player> prevState)
            {
                // �󒆃A�j���[�V����
                component.animator.SetTrigger(airId);
            }

            public override void OnUpdate(Player component)
            {
                // �e����
                if (component.IsShotbuttonDown)
                {
                    component.OnBulletShot();
                    component.IsShotbuttonDown = false;
                }
            }

            public override void OnExit(Player component, IState<Player> nextState)
            {
                var directionX = component.rigidbody.velocity.x;
                component.isRight = (directionX >= 0);

                base.OnExit(component, nextState);
            }
        }
    }
}