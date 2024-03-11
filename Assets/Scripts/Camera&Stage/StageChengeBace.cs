/* 制作日 2024/01/17
*　製作者 ニシガキ
*　最終更新日 2024/01/17
*/

using UnityEngine;
 
public class StageChengeBace : MonoBehaviour 
{
    private Collider _playerCollider = default;

    private void Awake()
    {
        _playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider>();
    }
}