/* 制作日 2024/01/30
*　製作者 ニシガキ
*　最終更新日 2024/01/30
*/

using UnityEngine;
using System.Collections;
 
public class PlayerAttackCollider : MonoBehaviour {
    [SerializeField]
    private int _addSp = 30;

    // ヒットストップ用クラス
    private IFHitStopManager _hitStopManager = default;

    private IFPlayerParameter _playerParameter = default;

    private void Awake()
    {
        // クラスの設定
        _playerParameter = GameObject.FindWithTag("Player").GetComponent<PlayerParameter>();
        _hitStopManager = GameObject.FindWithTag("Manager").GetComponent<HitStopManager>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Enemy")
        {
            _playerParameter.AddPlayerSp(_addSp);
            collision.GetComponent<EnemyParameter>().EnemyDamage(_playerParameter.GetAttack_Damage);
            float hitStopTime = 0.05f;
            _hitStopManager.HitStopEvent(hitStopTime);
        }
    }
}