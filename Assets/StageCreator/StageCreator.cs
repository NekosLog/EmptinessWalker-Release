/* 制作日
*　製作者
*　最終更新日
*/

using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Collections;

[Serializable]
public class StageCreator : MonoBehaviour
{
    #region シリアライズ

    [Header("ステージ作成キット  必要なデータを入れてください")]

    [Space]

    [Header("ステージデータ\nStageDatasフォルダにjsonファイルを入れてください")]

    [SerializeField, Tooltip("作成するステージのJSONファイル")]
    private TextAsset _loadFile;

    [Space]

    [Header("ステージオブジェクト\n使用していないオブジェクトは入れなくてもOK")]

    [SerializeField, Tooltip("１番のオブジェクト")]
    private GameObject _stageObject_1;

    [SerializeField, Tooltip("２番のオブジェクト")]
    private GameObject _stageObject_2;

    [SerializeField, Tooltip("３番のオブジェクト")]
    private GameObject _stageObject_3;

    [SerializeField, Tooltip("４番のオブジェクト")]
    private GameObject _stageObject_4;

    [SerializeField, Tooltip("５番のオブジェクト")]
    private GameObject _stageObject_5;

    #endregion

    #region フィールド

    // オブジェクトの設置間隔
    private const float PUT_DISTANCE = 1f;

    #endregion

    [ContextMenu("ステージ作成開始！")]
    public void CreatStage()
    {
        string stringData = _loadFile.text;

        StageData stageData = JsonUtility.FromJson<StageData>(stringData);
        Vector3 startPosition = Vector3.zero;

        startPosition = SetStageSize(stageData.row, stageData.column);

        int row = 0;
        int column = 0;

        while (row < stageData.row)
        {
            int objectNumber = stageData.paletteStats[row * stageData.column + column];

            if (objectNumber != 0)
            {
                Vector3 putPosition = startPosition;
                putPosition.x += column * PUT_DISTANCE;
                putPosition.y += row * -PUT_DISTANCE;

                Instantiate(SelectObject(objectNumber), putPosition, Quaternion.identity, transform);
            }

            column++;

            if (column == stageData.column)
            {
                row++;
                column = 0;
            }
        }

    }

    private Vector3 SetStageSize(float row, float column)
    {
        Vector3 startPositon = transform.position;
        startPositon.x += (column - 1) / 2 * -PUT_DISTANCE;
        startPositon.y += (row - 1) / 2 * PUT_DISTANCE;
        return startPositon;
    }

    private GameObject SelectObject(int objectNumber)
    {
        switch (objectNumber)
        {
            case 1:
                return _stageObject_1;

            case 2:
                return _stageObject_2;

            case 3:
                return _stageObject_3;

            case 4:
                return _stageObject_4;

            case 5:
                return _stageObject_5;

            default:
                Debugger.LogError("生成できない番号");
                return null;
        }
    }
}