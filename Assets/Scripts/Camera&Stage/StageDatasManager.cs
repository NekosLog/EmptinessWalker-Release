/* 制作日
*　製作者
*　最終更新日
*/

using UnityEngine;
using System.Collections;
 
public class StageDatasManager : MonoBehaviour {
    [SerializeField]
    private StageDatas StageDatas = default;

    public StageDatas GetStageDatas()
    {
        return this.StageDatas;
    }
}