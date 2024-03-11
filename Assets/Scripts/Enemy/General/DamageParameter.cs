/* 制作日
*　製作者
*　最終更新日
*/

using UnityEngine;
using System.Collections;
 
public class DamageParameter : MonoBehaviour {

    [SerializeField, Tooltip("与えるダメージ量")]
    private int _damage = default;

    /// <summary>
    /// 攻撃判定が当たった時に与えるダメージ量を取得するメソッド
    /// </summary>
    /// <returns></returns>
	public int GetDamage()
    {
        return _damage;
    }
}