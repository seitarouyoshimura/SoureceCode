using UnityEngine;
using System;

namespace HA.Yokohama.Beyond
{
    // �v���C���[�𐧌䂷��
    public partial class Player : MonoBehaviour
    {
        // �e�X�e�[�g�̃C���X�^���X
        [SerializeField] protected IdleState idleState;
        [SerializeField] protected JumpState jumpState;
        [SerializeField] protected AirState airState;
        [SerializeField] protected DeadState deadState;
        // ���݂̃X�e�[�g
        protected IState<Player> currentState = null;

        // �v���C���[�A�j���[�V����
        static readonly int idleId = Animator.StringToHash("Idle");
        static readonly int jumpId = Animator.StringToHash("Jump");
        static readonly int airId = Animator.StringToHash("Air");
        static readonly int deadId = Animator.StringToHash("Dead");
        // �v���C���[�̃A�j���[�^�[���擾
        [SerializeField] protected Animator animator = null;

        // Player�̕ϐ����Ǘ����Ă���ScriptableObject���擾����
        Player_Data player_Data;
        // ���Ԍv���p�ϐ�
        private float timer = 0;

        [SerializeField] protected Transform groundChecker;         // �ڒn����������Ȃ��`�F�b�J�[
        [SerializeField] protected LayerMask layerMask = default;   // �n�ʃ��C���[
        protected bool isGrounded = false;                          // �ڒn���Ă��邩

        // �v���C���[�̎q�I�u�W�F�N�g���w�肷��
        [SerializeField] protected Transform child;

        // �v���C���[���E�����Ɍ����Ă����true
        protected bool isRight = true;

        // �v���C���[�����˂���e
        [SerializeField] protected GameObject playerBullet = null;
        // �e�̑��x�ɂ������
        [SerializeField] protected int shotPower = 5;

        protected new Rigidbody2D rigidbody;

        private void Start()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            // Player_Data��ǂݍ���
            player_Data = Resources.Load<Player_Data>("Player_Data");
            player_Data.player_hp = player_Data.player_MaxHp;
            ChangeState(airState);
        }

        private void Update()
        {
            currentState.OnUpdate(this);
            OnRotation();
            OnCountDown();
        }

        // �X�e�[�g�̐؂�ւ�����
        private void ChangeState(IState<Player> _nextState)
        {
            if (currentState != null)
            {
                currentState.OnExit(this, _nextState);
            }
            _nextState.OnEnter(this, currentState);
            currentState = _nextState;
        }

        // ��������(�̗�)�̌v��
        private void OnCountDown()
        {
            timer += Time.deltaTime;
            // 1�b���Ƃɑ̗͌���
            if (timer >= 1)
            {
                player_Data.player_hp--;
                timer = 0;
            }
        }

        //�e�̔��˂𐧌䂷��֐�
        private void OnBulletShot()
        {
            var speed = keepMoveInput.normalized * shotPower;
            var position = transform.position;
            var bullet = Instantiate(playerBullet, position, Quaternion.identity);
            var target = speed + (Vector2)bullet.transform.position;
            bullet.transform.LookAt(target);
            bullet.transform.Rotate(new Vector3(90, 0, 0));
            bullet.GetComponent<Rigidbody2D>().AddForce(speed, ForceMode2D.Impulse);
        }

        //�v���C���[���i�s�����Ɍ���
        private void OnRotation()
        {
            // ��������
            var target = Vector2.zero;
            // Player�������Ă���ꍇ
            if (rigidbody.velocity.sqrMagnitude > 0.01f)
            {
                target = rigidbody.velocity + (Vector2)transform.position;
            }
            // Player�������Ă��Ȃ��ꍇ
            else
            {
                target = transform.position;
                target.x += isRight ? 1 : -1;
            }
            child.LookAt(target);
        }

        // �Œ�t���[���ŌĂ΂�鏈��
        private void FixedUpdate()
        {
            // �ڒn����
            var result = Physics2D.OverlapBox(
                groundChecker.position,
                groundChecker.localScale / 2,
                groundChecker.rotation.eulerAngles.z,
                layerMask);

            // �ڒn���Ă���ꍇ
            if (result)
            {
                // ���S���Ă���ꍇ�͔�����
                if (currentState == deadState)
                {
                    return;
                }
                // ���n�����ꍇ
                else if (!isGrounded)
                {
                    isGrounded = true;
                    ChangeState(idleState);
                }
            }
            // �󒆂ɕ����Ă���
            else
            {
                isGrounded = false;

                // ���S���Ă���ꍇ�͔�����
                if (currentState == deadState)
                {
                    return;
                }
                // ���𓥂݊O�����ꍇ
                else if (currentState != airState)
                {
                    ChangeState(airState);
                }
            }
        }
    }
}