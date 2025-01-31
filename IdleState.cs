using System;
using UnityEngine;

// 待機状態の制御
namespace HA.Yokohama.Beyond
{
    partial class Player
    {
        [Serializable]
        protected class IdleState : StateBase<Player>
        {
            public override void OnEnter(Player component, IState<Player> prevState)
            {
                // 待機アニメーション
                component.animator.SetTrigger(idleId);

                Effect_Hp.Instance.OnDisplay();
            }
            public override void OnUpdate(Player component)
            {
                // 左スティック入力があればその方向にジャンプする
                if (component.moveInput.sqrMagnitude > 0.01f)
                {
                    component.ChangeState(component.jumpState);
                }
                // Rトリガーが押されたら弾を発射する
                else if (component.IsShotbuttonDown)
                {
                    component.OnBulletShot();
                    component.IsShotbuttonDown = false;
                }
            }

            public override void OnExit(Player component, IState<Player> nextState)
            {
                Effect_Hp.Instance.OffDisplay();

                base.OnExit(component, nextState);
            }
        }

    }
}