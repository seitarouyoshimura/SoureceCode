using UnityEngine;
// ステートを管理するインターフェイス
public interface IState<T>
{
    /// <summary>
    /// ステート開始時の処理
    /// </summary>
    public void OnEnter(T component, IState<T> prevState);

    /// <summary>
    /// 毎フレーム呼ばれる処理
    /// </summary>
    public void OnUpdate(T component);

    /// <summary>
    /// ステート終了時の処理
    /// </summary>
    public void OnExit(T component, IState<T> nextState);
}

// ステートを管理する抽象クラス
public abstract class StateBase<T> : IState<T>
{
    public virtual void OnEnter(T component, IState<T> prevState)
    {
    }

    public virtual void OnUpdate(T component)
    {
    }

    public virtual void OnExit(T component, IState<T> nextState)
    {
    }
}