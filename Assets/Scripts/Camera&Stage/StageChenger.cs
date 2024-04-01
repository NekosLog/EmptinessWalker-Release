/* 制作日 2024/01/17
*　製作者 ニシガキ
*　最終更新日 2024/01/17
*/

using System.Collections;
using UnityEngine;
 
public class StageChenger : MonoBehaviour 
{
    // ステージデータをまとめたスクリプタブルオブジェクト
    private StageDatas _stageDatas = default;

    // カメラ移動を管理するクラス
    private CameraMove _cameraMove = default;

    // プレイヤーのTransform
    private Transform _playerTransform = default;

    [SerializeField, Tooltip("ワープする先の座標")]
    private Transform _spawnPosition = default;

    [SerializeField, Tooltip("ワープ前のステージ番号")]
    private E_StageNumber _myStageNumber = default;

    [SerializeField, Tooltip("ワープ先のステージ番号")]
    private E_StageNumber _nextStageNumber = default;

    [SerializeField, Tooltip("フェードアウト用のキャンバスアニメーション")]
    private Animator _fadeAnimator = default;

    private void Awake()
    {
        _playerTransform = GameObject.FindWithTag("PlayerParent").transform;
        _stageDatas = GameObject.FindWithTag("StageDatas").GetComponent<StageDatasManager>().GetStageDatas();
        _cameraMove = GameObject.FindWithTag("MainCamera").GetComponent<CameraMove>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        print("当たった");
        if (collision.tag == "Player")
        {
            StartCoroutine("ChengeStage");
        }
    }

    private StageParameter SearchStage(string stageNumber)
    {
        foreach (StageParameter parm in _stageDatas.StageParameterList)
        {
            if (parm.StageNumber == stageNumber)
            {
                return parm;
            }
        }
        Debug.LogError("ステージが見つかりませんでした。");
        return null;
    }

    private void Teleportation(Transform target, Vector2 position)
    {
        target.position = position;
    }

    private IEnumerator ChengeStage()
    {
        TimeManager.StopTime();
        _fadeAnimator.SetBool("FadeFlag", true);
        yield return new WaitForSeconds(0.5f);
        _fadeAnimator.SetBool("FadeFlag", false);
        Teleportation(_playerTransform, _spawnPosition.position);
        _cameraMove.SetStage(SearchStage(_nextStageNumber.ToString()));
        TimeManager.StartTime();
    }
}