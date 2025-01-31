using System;
using UnityEngine;

// �W�����v�ړ��̐���
namespace HA.Yokohama.Beyond
{
    partial class Player
    {
        [Serializable]
        protected class JumpState : StateBase<Player>
        {
            // �W�����v��
            [SerializeField] protected Vector2 JumpPower = new Vector2(0, 8);
            public override void OnEnter(Player component, IState<Player> prevState)
            {
                // �W�����v�A�j���[�V����
                // component.animator.SetTrigger(jumpId);

                // ��ԑO��velocity������������
                component.rigidbody.velocity = Vector2.zero;
                var power = JumpPower;
                power.x = component.moveInput.x * 2;
                component.rigidbody.AddForce(power, ForceMode2D.Impulse);

                // �v���C���[�����E�ǂ���ɔ�񂾂̂����擾
                component.isRight = power.x >= 0;
            }

            public override void OnUpdate(Player component)
            {
                // �΋��ԂɑJ��
                component.ChangeState(component.airState);
            }

        }
    }
}