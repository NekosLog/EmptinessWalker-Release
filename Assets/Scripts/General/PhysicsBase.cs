/* 制作日 2024/02/14
*　製作者 ニシガキ
*　最終更新日 2024/02/14
*/

using UnityEngine;
 
public class PhysicsBase : MonoBehaviour
{
    /// <summary>
    /// 壁判定
    /// </summary>
    protected CheckWall CheckWall { get; private set; }

    /// <summary>
    /// 床判定
    /// </summary>
    protected CheckFloor CheckFloor { get; private set; }

    /// <summary>
    /// 天井判定
    /// </summary>
    protected CheckCeiling CheckCeiling { get; private set; }

    // 移動量
    private Vector3 _moveValue = default;


    /// <summary>
    /// 初期設定　クラスのインスタンス
    /// </summary>
    protected virtual void Awake()
    {
        // それぞれのクラスをインスタンス
        CheckFloor = new CheckFloor(this.transform);
        CheckWall = new CheckWall(this.transform);
        CheckCeiling = new CheckCeiling(this.transform);
    }
}