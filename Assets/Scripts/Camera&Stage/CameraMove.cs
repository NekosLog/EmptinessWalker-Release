/* 制作日 2024/01/15
*　製作者 ニシガキ
*　最終更新日 2024/01/15
*/

using UnityEngine;
 
public class CameraMove : MonoBehaviour 
{
    private Transform _player = default;

    private Vector3 _nowPosition = default;
    private Vector3 _cameraOrigin = default;
    private float _stageWidth = default;
    private float _stageHeight = default;

    private Vector2 _distance = default;

    private const float MAX_DISTANCE_X = 3f;
    private const float MAX_DISTANCE_Y = 1.5f;
    private const float ORIGIN_DEFAULT_Z = -10f;
    private const float CAMERA_WIDTH = 17.75f;
    private const float CAMERA_HEIGHT = 0f;

    private void Awake()
    {
        _player = GameObject.FindWithTag("Player").transform;
        _nowPosition = transform.position;
    }

    private void Update()
    {
        PlayerTracking();
    }

    private void PlayerTracking()
    {
        _distance = _player.position - _nowPosition;

        if (Mathf.Abs(_distance.x) > MAX_DISTANCE_X)
        {
            _nowPosition.x += _distance.x - Mathf.Sign(_distance.x) * MAX_DISTANCE_X;
            if (_nowPosition.x < _cameraOrigin.x - _stageWidth)
            {
                _nowPosition.x = _cameraOrigin.x - _stageWidth;
            }
            if (_nowPosition.x > _cameraOrigin.x + _stageWidth)
            {
                _nowPosition.x = _cameraOrigin.x + _stageWidth;
            }
        }

        if (Mathf.Abs(_distance.y) > MAX_DISTANCE_Y)
        {
            _nowPosition.y += _distance.y - Mathf.Sign(_distance.y) * MAX_DISTANCE_Y;
            if (_nowPosition.y < _cameraOrigin.y - _stageHeight)
            {
                _nowPosition.y = _cameraOrigin.y - _stageHeight;
            }
            if (_nowPosition.y > _cameraOrigin.y + _stageHeight)
            {
                _nowPosition.y = _cameraOrigin.y + _stageHeight;
            }
        }

        transform.position = _nowPosition;
    }

    /// <summary>
    /// カメラの範囲をステージに合わせて設定するためのメソッド
    /// </summary>
    /// <param name="origin">カメラ原点</param>
    /// <param name="width">横幅</param>
    /// <param name="height">縦幅</param>
    public void SetStage(StageParameter stageParm)
    {
        // 各値を設定
        _cameraOrigin = stageParm.CameraOrigin;     // カメラ原点
        _cameraOrigin.z = ORIGIN_DEFAULT_Z;         // カメラ距離
        _stageWidth = Mathf.Clamp(stageParm.StageWidth - CAMERA_WIDTH, 0, Mathf.Infinity) / 2;     // 横の範囲　原点からの距離だから半分にする
        print(_stageWidth);
        _stageHeight = stageParm.StageHeight / 2;   // 縦の範囲　原点からの距離だから半分にする

        // 位置情報の更新
        transform.position = _cameraOrigin; // カメラの位置を設定
        _nowPosition = _cameraOrigin;       // 現在位置情報の更新
    }
}