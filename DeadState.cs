using System;

// 死亡状態の制御
namespace HA.Yokohama.Beyond
{
    partial class Player
    {
        [Serializable]
        protected class DeadState : StateBase<Player>
        {
            // 何もしない
        }

    }
}