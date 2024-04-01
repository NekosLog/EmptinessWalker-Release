/* 制作日
*　製作者
*　最終更新日
*/

using UnityEngine;
using System.Collections;
 
public class SkillBulletMove : MonoBehaviour
{
    // ヒットストップ用クラス
    private IFHitStopManager _hitStopManager = default;

    // プレイヤーのパラメータクラス
    private IFPlayerParameter _playerParameter = default;

    [SerializeField, Tooltip("弾速")]
    private float _bulletSpeed = 10f;

    private float _lifeTime = default;

    private const float LIFE_TIME_LIMITE = 2f;

    private Renderer _myRenderer = default;

    private void Awake()
    {
        // クラスの設定
        _playerParameter = GameObject.FindWithTag("Player").GetComponent<PlayerParameter>();
        _hitStopManager = GameObject.FindWithTag("Manager").GetComponent<HitStopManager>();

        // 自分のレンダラーを取得
        _myRenderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        transform.Translate(Vector3.right * _bulletSpeed * Time.deltaTime);

        _lifeTime += Time.deltaTime;

        if (!_myRenderer.isVisible)
        {
            Destroy(gameObject);
        }

        if (_lifeTime > LIFE_TIME_LIMITE)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Enemy":
                collision.GetComponent<EnemyParameter>().EnemyDamage(_playerParameter.GetSkill1_Damage);
                float hitStopTime = 0.15f;
                _hitStopManager.HitStopEvent(hitStopTime);
                Destroy(gameObject);
                break;

            default:
                break;
        }
    }

}