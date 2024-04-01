/* 制作日 2024/02/14
*　製作者 ニシガキ
*　最終更新日 2024/02/14
*/

using UnityEngine;
 
public class CheckCeiling
{
    /// <summary>
    /// コンストラクタ<br />
    /// 自身のオブジェクトを代入する
    /// </summary>
    /// <param name="thisTransform">代入するTransform</param>
    public CheckCeiling(Transform thisTransform)
    {
        // 自身のTransformを取得
        _thisTransform = thisTransform;
    }

    #region フィールド変数

    // オブジェクトの太さ　着地判定の幅を決めるのに使用　横幅の半分
    private float _objectDepth = default;

    // オブジェクトの縦幅　頭判定の出始めを決めるのに使用
    private float _objectUpper = default;

    // 接触判定の細かさ　上端と下端の間に何本の判定をとるか
    private int _checkValue = default;

    // 判定の間隔　上下端と本数によって変動
    private float _checkDistance = default;

    // 処理を行うオブジェクトのトランスフォーム
    private Transform _thisTransform = default;
    #endregion

    /// <summary>
    /// オブジェクトが着地しているかどうかを判定するbool
    /// </summary>
    public bool CheckHitCeiling()
    {
        // レイの長さ
        float rayLength = 0.2f;

        // ステージのレイヤーマスク
        LayerMask groundLayer = 1 << 6;

        // レイの生成と判定
        for (int i = 0; i < _checkValue; i++)
        {
            // オブジェクトと接触しているか判定
            if(Physics.Raycast(_thisTransform.position + _objectUpper * Vector3.up + (-_objectDepth + _checkDistance * i) * Vector3.right, Vector3.up, rayLength, groundLayer))
            {
                // ぶつかっている場合
                return true;
            }
        }
        // ぶつかっていない場合
        return false;
    }

    /// <summary>
    /// オブジェクトのサイズを設定するメソッド
    /// </summary>
    /// <param name="objectDepth">オブジェクトの横幅</param>
    /// <param name="objectHeight">オブジェクトの下幅</param>
    /// <param name="checkValue">判定の本数</param>
    public void SetObjectSize(float objectDepth, float objectHeight, int checkValue)
    {
        // 判定の細かさの最低値
        int checkMinimunValue = 2;

        // 各値を設定
        _objectDepth = objectDepth / 2; // 横幅の半分
        _objectUpper = objectHeight / 2; // 縦幅の半分

        // 判定の細かさの最低値を下回っていないか
        if (checkValue >= checkMinimunValue)
        {
            // 上回っている
            _checkValue = checkValue;
        }
        else
        {
            // 下回っている
            _checkValue = checkMinimunValue;
        }

        // 値から判定の間隔を設定　間隔を求めるため上下幅÷(本数ー１)
        _checkDistance = objectHeight / (_checkValue - 1);
    }
}