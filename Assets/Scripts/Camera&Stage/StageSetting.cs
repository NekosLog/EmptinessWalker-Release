/* 制作日
*　製作者
*　最終更新日
*/

using UnityEngine;
using System.Collections;
 
public class StageSetting : MonoBehaviour
{
	[SerializeField, Tooltip("最初のステージ名")]
	private E_StageNumber _firstStage = default;

	private void Awake()
    {
        // カメラの移動クラス
        CameraMove CameraMove = GameObject.FindWithTag("MainCamera").GetComponent<CameraMove>();

        // ステージデータを管理するクラス
        StageDatas stageDatas = GameObject.FindWithTag("StageDatas").GetComponent<StageDatasManager>().GetStageDatas();

        // ステージパラメータ
        StageParameter parameter = default;

		// 最初のステージのパラメータを取得
        foreach (StageParameter parm in stageDatas.StageParameterList)
		{
			if (parm.StageNumber == _firstStage.ToString())
			{
				parameter = parm;
			}
		}

		// パラメータを設定
		CameraMove.SetStage(parameter);
	}
}