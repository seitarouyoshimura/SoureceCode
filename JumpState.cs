using System;
using UnityEngine;

// ジャンプ移動の制御
namespace HA.Yokohama.Beyond
{
    partial class Player
    {
        [Serializable]
        protected class JumpState : StateBase<Player>
        {
            // ジャンプ力
            [SerializeField] protected Vector2 JumpPower = new Vector2(0, 8);
            public override void OnEnter(Player component, IState<Player> prevState)
            {
                // ジャンプアニメーション
                // component.animator.SetTrigger(jumpId);

                // 飛ぶ前にvelocityを初期化する
                component.rigidbody.velocity = Vector2.zero;
                var power = JumpPower;
                power.x = component.moveInput.x * 2;
                component.rigidbody.AddForce(power, ForceMode2D.Impulse);

                // プレイヤーが左右どちらに飛んだのかを取得
                component.isRight = power.x >= 0;
            }

            public override void OnUpdate(Player component)
            {
                // 対空状態に遷移
                component.ChangeState(component.airState);
            }

        }
    }
}