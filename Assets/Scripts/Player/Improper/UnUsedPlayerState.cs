/* 制作日 2023/12/20
*　製作者 ニシガキ
*　最終更新日 2023/12/20
*/

using UnityEngine;
using System.Collections; 

public class UnUsedPlayerState : MonoBehaviour
{
    #region State一覧
    private bool _canWalk = true;
    private bool _canJump = true;
    private bool _canDash = true;
    private bool _canCrouch = true;
    private bool _canAction = true;
    private bool _canAttack = true;
    private bool _canHeal = true;
    private bool _canSkill1 = true;
    private bool _canSkill2 = true;
    private bool _canRotate = true;
    #endregion

    private Coroutine _waitingCoroutine = default;

    /// <summary>
    /// PlayerStateを取得するためのプロパティ
    /// </summary>
    /// <param name="state">取得するState</param>
    /// <returns></returns>
    public bool GetState(E_PlayerState state)
    {
        // 取得するStateを選定
        switch (state)
        {
            case E_PlayerState.Walk:
                return _canWalk;

            case E_PlayerState.Jump:
                return _canJump;

            case E_PlayerState.Dash:
                return _canDash;

            case E_PlayerState.Action:
                return _canAction;

            case E_PlayerState.Attack:
                return _canAttack;

            case E_PlayerState.Heal:
                return _canHeal;

            case E_PlayerState.Skill1:
                return _canSkill1;

            case E_PlayerState.Skill2:
                return _canSkill2;
        }
        // 例外処理
        Debug.LogError("PlayerStateに異常あり");
        return false;
    }

    /// <summary>
    /// PlayerStateの値を設定するプロパティ
    /// </summary>
    /// <param name="state">設定するState</param>
    /// <param name="value">設定する値</param>
    public void OnState(E_PlayerState state, bool value)
    {
        // 設定するStateを選定
        switch (state)
        {
            case E_PlayerState.Walk:
                _canWalk = value;
                return;

            case E_PlayerState.Jump:
                _canJump = value;
                return;

            case E_PlayerState.Dash:
                _canDash = value;
                return;

            case E_PlayerState.Action:
                _canAction = value;
                return;

            case E_PlayerState.Attack:
                _canAttack = value;
                return;

            case E_PlayerState.Heal:
                _canHeal = value;
                return;

            case E_PlayerState.Skill1:
                _canSkill1 = value;
                return;

            case E_PlayerState.Skill2:
                _canSkill2 = value;
                return;
        }
        // 例外処理
        Debug.LogError("PlayerStateに異常あり");
    }

    /// <summary>
    /// <para>PlayerStateの値を設定するプロパティ</para>
    /// <para>指定した時間待ってから実行</para>
    /// </summary>
    /// <param name="state">設定するState</param>
    /// <param name="value">設定する値</param>
    /// <param name="delayTime">処理を待つ時間</param>
    public void SetState(E_PlayerState state, bool value, float delayTime)
    {
        if (_waitingCoroutine != null)
        {
            StopCoroutine(_waitingCoroutine);
        }
        _waitingCoroutine = StartCoroutine(DelaySet(state, value, delayTime));
    }

    /// <summary>
    /// 指定した時間待ってから実装するためのコルーチン
    /// </summary>
    /// <param name="value">設定する値</param>
    /// <param name="delayTime">処理を待つ時間</param>
    private IEnumerator DelaySet(E_PlayerState state, bool value, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        OnState(state, value);
    }

    /// <summary>
    /// すべてのStateに値を設定するプロパティ
    /// </summary>
    /// <param name="value">設定する値</param>
    public void SetAllState(bool value)
    {
        _canWalk = value;
        _canJump = value;
        _canDash = value;
        _canCrouch = value;
        _canAction = value;
        _canAttack = value;
        _canHeal = value;
        _canSkill1 = value;
        _canSkill2 = value;
        _canRotate = value;
    }

    /// <summary>
    /// <para>すべてのStateに値を設定するプロパティ</para>
    /// <para>指定した時間待ってから実行</para>
    /// </summary>
    /// <param name="value">設定する値</param>
    /// <param name="delayTime">処理を待つ時間</param>
    public void SetAllState(bool value, float delayTime)
    {
        if (_waitingCoroutine != null)
        {
            StopCoroutine(_waitingCoroutine);
        }
        _waitingCoroutine = StartCoroutine(DelaySetAll(value, delayTime));
    }

    /// <summary>
    /// 指定した時間待ってから実装するためのコルーチン
    /// </summary>
    /// <param name="value">設定する値</param>
    /// <param name="delayTime">処理を待つ時間</param>
    private IEnumerator DelaySetAll(bool value, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        SetAllState(value);
        if (_waitingCoroutine != null)
        {
            _waitingCoroutine = null;
        }
    }
}