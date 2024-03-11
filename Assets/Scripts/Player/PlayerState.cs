/* 制作日 2024/02/18
*　製作者 ニシガキ
*　最終更新日 2024/03/07
*/

using UnityEngine;
using System.Collections; 

public class PlayerState : MonoBehaviour, IFPlayerState
{
    // プレイヤーの行動状態　アニメーション管理に使用
    private E_PlayerState _nowPlayerState = E_PlayerState.Wait;

    // プレイヤーの行動フラグをまとめたStruct
    private struct PlayerStateFlag
    {
        public readonly E_PlayerState _state;
        public bool _flag;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="state">State</param>
        /// <param name="flag">フラグの状態</param>
        public PlayerStateFlag(E_PlayerState state, bool flag)
        {
            _state = state;
            _flag = flag;
        }
    }

    // プレイヤーの行動フラグを管理する配列
    private PlayerStateFlag[] _stateFlagTable = new PlayerStateFlag[9]
    {
        new PlayerStateFlag(E_PlayerState.Walk, false),
        new PlayerStateFlag(E_PlayerState.Jump, false),
        new PlayerStateFlag(E_PlayerState.Dash, false),
        new PlayerStateFlag(E_PlayerState.Crouch, false),
        new PlayerStateFlag(E_PlayerState.Action, false),
        new PlayerStateFlag(E_PlayerState.Attack, false),
        new PlayerStateFlag(E_PlayerState.Heal, false),
        new PlayerStateFlag(E_PlayerState.Skill1, false),
        new PlayerStateFlag(E_PlayerState.Skill2, false),
    };

    private Coroutine _waitingCoroutine = default;

    #region プロパティ

    /// <summary>
    /// PlayerStateを取得するためのプロパティ
    /// </summary>
    /// <param name="state">取得するState</param>
    /// <returns></returns>
    public bool GetState(E_PlayerState state)
    {
        // 取得するStateを選定
        for (int i = 0; i < _stateFlagTable.Length; i++)
        {
            // 一致するStateを探索
            if (state == _stateFlagTable[i]._state)
            {
                // 一致したStateのフラグを返す
                return _stateFlagTable[i]._flag;
            }
        }

        // 例外処理
        Debug.LogError("PlayerStateに異常あり");
        return false;
    }

    /// <summary>
    /// 指定したStateのフラグをオンにする
    /// </summary>
    /// <param name="state">指定するState</param>
    public void OnStateFlag(E_PlayerState state)
    {
        // 取得するStateを選定
        for (int i = 0; i < _stateFlagTable.Length; i++)
        {
            // 一致するStateを探索
            if (state == _stateFlagTable[i]._state)
            {
                // 一致したStateのフラグを返す
                _stateFlagTable[i]._flag = true;
            }
        }
    }

    /// <summary>
    /// 指定したStateのフラグを待機時間後にオンにする
    /// </summary>
    /// <param name="state">指定するState</param>
    /// <param name="delayTime">待機時間</param>
    public Coroutine OnStateFlag(E_PlayerState state, float delayTime)
    {
        // コルーチンに引数を渡して実行開始
        return StartCoroutine(DelaySetOn(state, delayTime));
    }

    /// <summary>
    /// 指定したStateのフラグをオフにする
    /// </summary>
    /// <param name="state">指定するState</param>
    public void OffStateFlag(E_PlayerState state)
    {
        // 取得するStateを選定
        for (int i = 0; i < _stateFlagTable.Length; i++)
        {
            // 一致するStateを探索
            if (state == _stateFlagTable[i]._state)
            {
                // 一致したStateのフラグを返す
                _stateFlagTable[i]._flag = false;
            }
        }
    }

    /// <summary>
    /// 指定したStateのフラグを待機時間後にオフにする
    /// </summary>
    /// <param name="state">指定するState</param>
    /// <param name="delayTime">待機時間</param>
    public Coroutine OffStateFlag(E_PlayerState state, float delayTime)
    {
        // コルーチンに引数を渡して実行開始
        return StartCoroutine(DelaySetOff(state, delayTime));
    }

    #endregion

    /// <summary>
    /// 指定された時間待ってからStateをオンにするコルーチン
    /// </summary>
    /// <param name="state">設定するState</param>
    /// <param name="delayTime">待機時間</param>
    private IEnumerator DelaySetOn(E_PlayerState state, float delayTime)
    {
        // 指定された時間待つ
        yield return new WaitForSeconds(delayTime);

        // Stateをオンにする
        OnStateFlag(state);
    }

    /// <summary>
    /// 指定された時間待ってからStateをオフにするコルーチン
    /// </summary>
    /// <param name="state">設定するState</param>
    /// <param name="delayTime">待機時間</param>
    private IEnumerator DelaySetOff(E_PlayerState state, float delayTime)
    {
        // 指定された時間待つ
        yield return new WaitForSeconds(delayTime);
        
        // Stateをオフにする
        OffStateFlag(state);
    }
}