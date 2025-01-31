using UnityEngine;
// �X�e�[�g���Ǘ�����C���^�[�t�F�C�X
public interface IState<T>
{
    /// <summary>
    /// �X�e�[�g�J�n���̏���
    /// </summary>
    public void OnEnter(T component, IState<T> prevState);

    /// <summary>
    /// ���t���[���Ă΂�鏈��
    /// </summary>
    public void OnUpdate(T component);

    /// <summary>
    /// �X�e�[�g�I�����̏���
    /// </summary>
    public void OnExit(T component, IState<T> nextState);
}

// �X�e�[�g���Ǘ����钊�ۃN���X
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