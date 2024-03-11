/* 制作日 2024/01/17
*　製作者 ニシガキ
*　最終更新日 2024/01/17
*/

using UnityEngine;
 
public class StageChenger : MonoBehaviour 
{
    private StageDatas _stageDatas = default;

    private CameraMove _cameraMove = default;

    [SerializeField]
    private Transform _spawnPosition = default;

    [SerializeField]
    private E_StageNumber _nextStageNumber = default;
    private void Awake()
    {
        _stageDatas = GameObject.FindWithTag("StageDatas").GetComponent<StageDatasManager>().GetStageDatas();
        _cameraMove = GameObject.FindWithTag("MainCamera").GetComponent<CameraMove>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("当たった");
        if (collision.tag == "Player")
        {
            Teleportation(collision.transform, _spawnPosition.position);
            _cameraMove.SetStage(SearchStage(_nextStageNumber.ToString()));
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
}