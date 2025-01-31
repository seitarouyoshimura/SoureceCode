using System;
using UnityEngine;

// 空中状態の制御
namespace HA.Yokohama.Beyond
{
    partial class Player
    {
        [Serializable]
        protected class AirState : StateBase<Player>
        {
            public override void OnEnter(Player component, IState<Player> prevState)
            {
                // 空中アニメーション
                component.animator.SetTrigger(airId);
            }

            public override void OnUpdate(Player component)
            {
                // 弾発射
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