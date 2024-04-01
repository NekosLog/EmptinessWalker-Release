/* 制作日
*　製作者
*　最終更新日
*/

using UnityEngine;
using System.Collections;
 
public class Fall : IFFall
{
    /// <summary>
    /// コンストラクタ　着地時のイベントとTransformをもらう
    /// </summary>
    /// <param name="landingEvent"></param>
    public Fall(IFLandingEvent landingEvent, Transform thisTransform)
    {
        _landingEvent = landingEvent;
        _thisTransform = thisTransform;
    }

    #region 定数
    // 重力の大きさ
    private const float GRAVITY = 35.0f;

    // 終端速度
    private const float TERMINAL = -15f;
    #endregion

    #region 変数
    // オブジェクトの落下量
    private Vector2 _fallValue = default;

    // 着地時のイベントを渡すインターフェース
    private IFLandingEvent _landingEvent = default;

    // 自身のTransform
    private Transform _thisTransform = default;
    #endregion

    /// <summary>
    /// オブジェクトの落下や上昇を行うメソッド
    /// </summary>
    public void FallObject(bool isLanding, bool isHitCeiling)
    {
        // 天井にぶつかったかどうか
        if (_fallValue.y > 0 && isHitCeiling)
        {
            // 速度を０にする
            _fallValue = Vector2.zero;
        }
        // 着地しているかどうか
        else if (_fallValue.y <= 0 && isLanding)
        {
            if (_fallValue.y != 0)
            {
                // 速度を０にする
                _fallValue = Vector2.zero;

                // 着地時のイベントを実行
                _landingEvent.LandingEvent();
            }
        }
        else
        {
            // 終端速度より遅いなら加速
            if (_fallValue.y >= TERMINAL)
            {
                // 重力に応じて加速
                _fallValue.y -= GRAVITY * Time.deltaTime;
            }
        }

        // プレイヤーを移動
        _thisTransform.position += (Vector3)_fallValue * Time.deltaTime;
    }

    /// <summary>
    /// 落下量を設定するメソッド　正数で上昇　負数で下降
    /// </summary>
    /// <param name="setValue">落下量</param>
    public void SetFallValue(float setValue)
    {
        // 値を設定
        _fallValue.y = setValue;
    }
}