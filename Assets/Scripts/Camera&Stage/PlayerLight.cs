/* 制作日
*　製作者
*　最終更新日
*/

using UnityEngine;
using System.Collections;
 
public class PlayerLight : MonoBehaviour {

	[SerializeField]
	private Transform _playerTransform = default;

	private void Update()
	{
		Vector3 pos = _playerTransform.position;
		pos.z = -5;
		transform.position = pos;
	}
}