using System;
using UnityEngine;

// �ҋ@��Ԃ̐���
namespace HA.Yokohama.Beyond
{
    partial class Player
    {
        [Serializable]
        protected class IdleState : StateBase<Player>
        {
            public override void OnEnter(Player component, IState<Player> prevState)
            {
                // �ҋ@�A�j���[�V����
                component.animator.SetTrigger(idleId);

                Effect_Hp.Instance.OnDisplay();
            }
            public override void OnUpdate(Player component)
            {
                // ���X�e�B�b�N���͂�����΂��̕����ɃW�����v����
                if (component.moveInput.sqrMagnitude > 0.01f)
                {
                    component.ChangeState(component.jumpState);
                }
                // R�g���K�[�������ꂽ��e�𔭎˂���
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