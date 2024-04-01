/* 制作日
*　製作者
*　最終更新日
*/

using UnityEngine;
using System.Collections;
 
public class AddAttackEvent : MonoBehaviour {
    // 攻撃判定のコライダー
    private Collider _attackCollider = default;

    [SerializeField, Tooltip("攻撃のアニメーション")]
    private AnimationClip _attackAnimation = default;

    [SerializeField, Tooltip("攻撃判定の発生速度")]
    private float _startUpTime = 0.2f;

    [SerializeField, Tooltip("攻撃判定の持続時間")]
    private float _attackActiveTime = 0.1f;

    public void AddEvent(Collider attackCollider)
    {
        // 攻撃判定のコライダーを設定
        _attackCollider = attackCollider;

        // 実行するメソッド名
        string startUpEvent = "StartUp";

        // 攻撃用のAnimationEvent
        AnimationEvent attack1Event = new AnimationEvent();

        // 時間と実行するメソッドを設定
        attack1Event.time = _startUpTime;
        attack1Event.functionName = startUpEvent;

        // イベントを追加
        _attackAnimation.AddEvent(attack1Event);
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