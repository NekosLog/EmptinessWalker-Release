/* 制作日 2024/02/14
*　製作者 ニシガキ
*　最終更新日 2024/02/14
*/

using UnityEngine;
 
public class UnUsedMoveBase : MonoBehaviour
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

    /// <summary>
    /// 移動を行う
    /// </summary>
    private void Update()
    {
        // 移動量の分だけ移動させる
        transform.position += _moveValue * Time.deltaTime;
    }

    #region SetMoveValueメソッド

    /// <summary>
    /// 移動量の設定メソッド　指定した値を直接代入する
    /// </summary>
    /// <param name="x">Xの値</param>
    protected void SetMoveValue_X(float x)
    {
        // 値を代入
        _moveValue.x = x;
    }

    /// <summary>
    /// 移動量の設定メソッド　指定した値を直接代入する
    /// </summary>
    /// <param name="y">Yの値</param>
    protected void SetMoveValue_Y(float y)
    {
        // 値を代入
        _moveValue.y = y;
    }

    /// <summary>
    /// 移動量の設定メソッド　指定した値を直接代入する
    /// </summary>
    /// <param name="x">Xの値</param>
    /// <param name="y">Yの値</param>
    protected void SetMoveValue(float x, float y)
    {
        // それぞれの値を代入
        _moveValue.x = x;
        _moveValue.y = y;
    }

    /// <summary>
    /// 移動量の設定メソッド　指定した値を直接代入する
    /// </summary>
    /// <param name="value">代入する値</param>
    protected void SetMoveValue(Vector2 value)
    {
        // それぞれの値を代入
        _moveValue.x = value.x;
        _moveValue.y = value.y;
    }

    /// <summary>
    /// 移動量の設定メソッド　指定した値を直接代入する
    /// </summary>
    /// <param name="value">代入する値</param>
    protected void SetMoveValue(Vector3 value)
    {
        // それぞれの値を代入
        _moveValue.x = value.x;
        _moveValue.y = value.y;
    }

    #endregion

    #region AddMoveValueメソッド

    /// <summary>
    /// 移動量の設定メソッド　指定した値を加算する
    /// </summary>
    /// <param name="x">Xの値</param>
    /// <param name="y">Yの値</param>
    protected void AddMoveValue(float x, float y)
    {
        // それぞれの値を加算
        _moveValue.x += x;
        _moveValue.y += y;
    }

    /// <summary>
    /// 移動量の設定メソッド　指定した値を加算する
    /// </summary>
    /// <param name="value">加算する値</param>
    protected void AddMoveValue(Vector2 value)
    {
        // それぞれの値を加算
        _moveValue.x += value.x;
        _moveValue.y += value.y;
    }

    /// <summary>
    /// 移動量の設定メソッド　指定した値を加算する
    /// </summary>
    /// <param name="value">加算する値</param>
    protected void AddMoveValue(Vector3 value)
    {
        // それぞれの値を加算
        _moveValue.x += value.x;
        _moveValue.y += value.y;
    }

    #endregion
}