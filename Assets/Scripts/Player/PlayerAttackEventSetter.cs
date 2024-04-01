/* 制作日
*　製作者
*　最終更新日
*/

using UnityEngine;
using System.Collections;
 
public class PlayerAttackEventSetter : MonoBehaviour 
{
    [SerializeField, Tooltip("攻撃判定のコライダー")]
    private Collider _attackCollider = default;

    [SerializeField, Tooltip("１段目の攻撃のアニメーション")]
    private AnimationClip _attack1Animation = default;

    [SerializeField, Tooltip("２段目の攻撃のアニメーション")]
    private AnimationClip _attack2Animation = default;

    [SerializeField, Tooltip("攻撃判定の発生速度")]
    private float _startUpTime = 0.25f;

    [SerializeField, Tooltip("攻撃判定の持続時間")]
    private float _attackActiveTime = 0.1f;


    private void Awake()
    {
        // イベントを追加する
        AddEvent(_attack1Animation);
        AddEvent(_attack2Animation);

        // 使い終わったら不要な参照を解除
        _attack1Animation = null;
        _attack2Animation = null;
    }

    public void AddEvent(AnimationClip animation)
    {
        // 実行するメソッド名
        string startUpEvent = "StartUp";

        // 攻撃用のAnimationEvent
        AnimationEvent attackEvent = new AnimationEvent();

        // 時間と実行するメソッドを設定
        attackEvent.time = _startUpTime;
        attackEvent.functionName = startUpEvent;

        // イベントを追加
        animation.AddEvent(attackEvent);
    }

    public void StartUp()
    {
        _attackCollider.enabled = true;

        StartCoroutine("EndAttack");
    }

    private IEnumerator EndAttack()
    {
        yield return new WaitForSeconds(_attackActiveTime);

        _attackCollider.enabled = false;
    }
}