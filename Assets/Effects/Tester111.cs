/* 制作日
*　製作者
*　最終更新日
*/

using UnityEngine;
using System.Collections;
 
public class Tester111 : PhysicsBase {

    private void Update()
    {
        CheckCeiling.CheckHitCeiling();
        CheckFloor.CheckHitFloor();
        CheckWall.CheckHitWall(E_InputType.Right);
        CheckWall.CheckHitWall(E_InputType.Left);
    }
}