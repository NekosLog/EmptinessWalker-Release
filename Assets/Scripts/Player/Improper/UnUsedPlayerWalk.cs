/* 制作日 2024/02/14
*　製作者 ニシガキ
*　最終更新日 2024/02/14
*/

using UnityEngine;
using System.Collections;
 
public class UnUsedPlayerWalk
{
    /// <summary>
    /// コンストラクタ　自身のTransformを取得
    /// </summary>
    /// <param name="transform">自身のTransform</param>
    public UnUsedPlayerWalk(Transform transform)
    {
        _playerTransform = transform;

        _lookingRotation = _playerTransform.rotation.eulerAngles;
    }

    // 自身のTransform
    private Transform _playerTransform = default;

    // 向いている方向
    private Vector3 _lookingRotation = default;

    // 歩行速度
    private float _walkSpeed = default;

    /// <summary>
    /// 歩行処理
    /// </summary>
    public float Walk(E_InputType inputType)
    {
        // 向き
        Vector3 forwardRotation = default;

        // 進行方向
        float direction = default;

        switch (inputType)
        {
            // 右方向
            case E_InputType.Right:

                break;

            // 左方向
            case E_InputType.Left:

                break;

            // その他
            default:
                Debugger.LogError("PlayerWalkに異常あり：InputTypeが左右以外");
                return 0;
        }

        if (_lookingRotation != forwardRotation)
        {
            _playerTransform.rotation = Quaternion.Euler(forwardRotation);
            _lookingRotation = forwardRotation;
        }

        return direction * _walkSpeed;
    }

}