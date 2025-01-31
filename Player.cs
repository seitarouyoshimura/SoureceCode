using UnityEngine;
using System;

namespace HA.Yokohama.Beyond
{
    // プレイヤーを制御する
    public partial class Player : MonoBehaviour
    {
        // 各ステートのインスタンス
        [SerializeField] protected IdleState idleState;
        [SerializeField] protected JumpState jumpState;
        [SerializeField] protected AirState airState;
        [SerializeField] protected DeadState deadState;
        // 現在のステート
        protected IState<Player> currentState = null;

        // プレイヤーアニメーション
        static readonly int idleId = Animator.StringToHash("Idle");
        static readonly int jumpId = Animator.StringToHash("Jump");
        static readonly int airId = Animator.StringToHash("Air");
        static readonly int deadId = Animator.StringToHash("Dead");
        // プレイヤーのアニメーターを取得
        [SerializeField] protected Animator animator = null;

        // Playerの変数を管理しているScriptableObjectを取得する
        Player_Data player_Data;
        // 時間計測用変数
        private float timer = 0;

        [SerializeField] protected Transform groundChecker;         // 接地判定をおこなうチェッカー
        [SerializeField] protected LayerMask layerMask = default;   // 地面レイヤー
        protected bool isGrounded = false;                          // 接地しているか

        // プレイヤーの子オブジェクトを指定する
        [SerializeField] protected Transform child;

        // プレイヤーが右方向に向いていればtrue
        protected bool isRight = true;

        // プレイヤーが発射する弾
        [SerializeField] protected GameObject playerBullet = null;
        // 弾の速度にかける力
        [SerializeField] protected int shotPower = 5;

        protected new Rigidbody2D rigidbody;

        private void Start()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            // Player_Dataを読み込み
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

        // ステートの切り替え処理
        private void ChangeState(IState<Player> _nextState)
        {
            if (currentState != null)
            {
                currentState.OnExit(this, _nextState);
            }
            _nextState.OnEnter(this, currentState);
            currentState = _nextState;
        }

        // 制限時間(体力)の計測
        private void OnCountDown()
        {
            timer += Time.deltaTime;
            // 1秒ごとに体力減少
            if (timer >= 1)
            {
                player_Data.player_hp--;
                timer = 0;
            }
        }

        //弾の発射を制御する関数
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

        //プレイヤーが進行方向に向く
        private void OnRotation()
        {
            // 向く方向
            var target = Vector2.zero;
            // Playerが動いている場合
            if (rigidbody.velocity.sqrMagnitude > 0.01f)
            {
                target = rigidbody.velocity + (Vector2)transform.position;
            }
            // Playerが動いていない場合
            else
            {
                target = transform.position;
                target.x += isRight ? 1 : -1;
            }
            child.LookAt(target);
        }

        // 固定フレームで呼ばれる処理
        private void FixedUpdate()
        {
            // 接地判定
            var result = Physics2D.OverlapBox(
                groundChecker.position,
                groundChecker.localScale / 2,
                groundChecker.rotation.eulerAngles.z,
                layerMask);

            // 接地している場合
            if (result)
            {
                // 死亡している場合は抜ける
                if (currentState == deadState)
                {
                    return;
                }
                // 着地した場合
                else if (!isGrounded)
                {
                    isGrounded = true;
                    ChangeState(idleState);
                }
            }
            // 空中に浮いている
            else
            {
                isGrounded = false;

                // 死亡している場合は抜ける
                if (currentState == deadState)
                {
                    return;
                }
                // 足を踏み外した場合
                else if (currentState != airState)
                {
                    ChangeState(airState);
                }
            }
        }
    }
}