/* 制作日 2023/12/20
*　製作者 ニシガキ
*　最終更新日 2023/12/20
*/
 
using UnityEngine;
using UniRx;

/// <summary>
/// PlayerStateのインターフェース
/// </summary>
public interface IFPlayerState {
    public bool GetState(E_PlayerState state);
    public void OnStateFlag(E_PlayerState state);
    public Coroutine OnStateFlag(E_PlayerState state, float delayTime);
    public void OffStateFlag(E_PlayerState state);
    public Coroutine OffStateFlag(E_PlayerState state, float delayTime);
    public void DelayCancel(Coroutine cancelCoroutine);
    public IReadOnlyReactiveProperty<bool> AttackAssessmentProperty { get; }
    public IReadOnlyReactiveProperty<bool> ActiveActionAssessmentProperty { get; }
}