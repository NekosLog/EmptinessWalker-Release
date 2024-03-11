/* 制作日
*　製作者
*　最終更新日
*/

using UnityEngine;
using System.Collections;
 
public class HitStopManager : MonoBehaviour ,IFHitStopManager
{

    // ヒットストップの時間を計るタイマー
    private float _stopTimer = default;
    
    // ヒットストップ中かどうか　trueでストップ中
    private bool _nowStop = default;

    private void Update()
    {
        // 停止時間のカウントと停止解除
        if (_stopTimer > 0)
        {
            // 時間のカウント
            _stopTimer -= Time.unscaledDeltaTime;
        }
        else if (_nowStop)
        {
            // 通常時のTimeScale
            float standardTimeScale = 1f;

            // TimeScaleをもとに戻す
            Time.timeScale = standardTimeScale;

            // 停止フラグを消す
            _nowStop = false;
        }
    }

    /// <summary>
    /// ヒットストップを行うメソッド
    /// </summary>
    /// <param name="stopTime">停止時間</param>
    public void HitStopEvent(float stopTime)
    {
        // 停止中のTimeScale
        float stopTimeScale = 0f;

        // 時間停止
        Time.timeScale = stopTimeScale;

        // 停止時間を設定
        _stopTimer = stopTime;

        // 停止フラグを立てる
        _nowStop = true;
    }
}