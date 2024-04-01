/* 制作日
*　製作者
*　最終更新日
*/

using UnityEngine;
using System.Collections;
 
public class EnemyParameter : MonoBehaviour {
    [SerializeField, Tooltip("敵のHP")]
    private int _enemyHP = default;

    private void Update()
    {
        if (_enemyHP <= 0)
        {
            // 死亡時
            Destroy(gameObject);
        }
    }

    public void EnemyDamage(int damage)
    {
        _enemyHP -= damage;
    }
}