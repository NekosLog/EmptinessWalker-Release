// ----------------------------------------------------------------------------
// 作成者 ニシガキ
// 作成開始日 2024/01/15
// 特筆事項 ステージ移動等で使用するステージデータを管理するクラスです。
//          それぞれの値はToolCipを参照してください。
// ----------------------------------------------------------------------------
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "StageDatas", menuName = "ScriptableObjects/CreateStageDatas")]
public class StageDatas : ScriptableObject
{
    public List<StageParameter> StageParameterList = new List<StageParameter>();
}

[System.Serializable] 
public class StageParameter
{
    public string StageNumber = "ステージ番号";

    public Vector2 CameraOrigin = default;

    public float StageWidth = default;

    public float StageHeight = default;
}