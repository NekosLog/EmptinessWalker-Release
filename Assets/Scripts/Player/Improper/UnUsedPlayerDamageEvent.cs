///* 制作日 2024/02/08
//*　製作者 ニシガキ
//*　最終更新日 2024/02/08
//*/

//using UnityEngine;
 
//public class UnUsedPlayerDamageEvent : MonoBehaviour {
//    // ダメージを与えるプレイヤーのパラメータを設定
//    private IFPlayerParameter _playerParameter = default;

//    // ヒットストップ用クラス
//    private IFHitStopManager _hitStopManager = default;

//    // 無敵時間のカウント変数
//    private float _invincibleTimer = default;

//    // 設定する無敵時間の長さ
//    private const float SET_TIME_STANDARD = 1f;

//    /// <summary>
//    /// 初期設定
//    /// </summary>
//    private void Awake()
//    {
//        // クラスを設定
//        _playerParameter = gameObject.GetComponent<UnUsedPlayerParameter>();
//        _hitStopManager = GameObject.FindWithTag("Manager").gameObject.GetComponent<HitStopManager>();
//    }

//    /// <summary>
//    /// 無敵時間のカウントダウン
//    /// </summary>
//    private void Update()
//    {
//        _invincibleTimer -= Time.deltaTime;
//    }

//    /// <summary>
//    /// 攻撃判定に接触している時に実行
//    /// </summary>
//    /// <param name="collision">接触しているコライダー</param>
//    private void OnTriggerStay2D(Collider2D collision)
//    {
//        // 無敵時間外　かつ　攻撃判定と接触している
//        if (_invincibleTimer <= 0 && collision.gameObject.tag == "DamageCollider")
//        {
//            // ヒットストップする時間
//            float hitStopTime = 0.2f;

//            // ヒットストップを発生させる
//            _hitStopManager.HitStopEvent(hitStopTime);

//            // 攻撃判定が持つダメージ量分プレイヤーのHPを減らす
//            _playerParameter.AddPlayerHp(-collision.gameObject.GetComponent<DamageParameter>().GetDamage());

//            // 無敵時間を設定する
//            _invincibleTimer = SET_TIME_STANDARD;
//        }
//    }
//}