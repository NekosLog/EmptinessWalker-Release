/* 制作日 2024/02/07
*　製作者 ニシガキ
*　最終更新日 2024/02/07
*/

using UnityEngine;
using UnityEngine.UI;
 
public class SPGageGradualChange : MonoBehaviour
{
    // SPゲージのImage
    private Image _spGageImage = default;

    // 経過時間
    private float _changeTimer = default;

    // SPゲージの変化量
    private float _changeValue = default;

    // SPゲージの変化開始時の割合
    private float _startRatio = default;

    // 変化開始のフラグ
    private bool _isStart = false;

    // 終了時刻
    private const float END_TIME = 0.15f;


    /// <summary>
    /// SPゲージを増減させるための初期設定メソッド　インスタンス時はこっち
    /// </summary>
    /// <param name="spGageImage">変化させるSPゲージ</param>
    /// <param name="startRatio">最初の割合</param>
    /// <param name="endRatio">終了時の割合</param>
    public void StartChanging(Image spGageImage, float startRatio, float endRatio)
    {
        // Imageの代入
        _spGageImage = spGageImage;

        // 開始時の割合を設定
        _startRatio = startRatio;

        // 変化量を設定
        _changeValue = endRatio - _startRatio;

        // 経過時間の初期化
        _changeTimer = default;

        // 開始フラグを立てる
        _isStart = true;
    }

    /// <summary>
    /// SPゲージを増減させるための初期設定メソッド　途中で変更するときはこっち
    /// </summary>
    /// <param name="endRatio">終了時の割合</param>
    public void StartChanging(float endRatio)
    {
        // 開始時の割合を設定　変化中の現在値を代入
        _startRatio = _startRatio + (_changeValue * _changeTimer / END_TIME);

        // 変化量を設定
        _changeValue = endRatio - _startRatio;

        // 経過時間の初期化
        _changeTimer = default;

        print("途中から");
    }

    private void Update()
    {
        // 開始フラグが立ったら
        if (_isStart)
        {
            // 経過時間をカウント
            _changeTimer += Time.deltaTime;

            // 終了時刻になったら自分で消える
            if (_changeTimer >= END_TIME)
            {
                // 終了時の割合に設定
                _spGageImage.fillAmount = _startRatio + _changeValue;

                // このクラスを消す
                Destroy(this);
            }

            // 現在の割合を算出
            float nowRatio = _startRatio + (_changeValue * _changeTimer / END_TIME);

            // SPゲージの割合を設定
            _spGageImage.fillAmount = nowRatio;
        }
    }
}