/* 制作日
*　製作者
*　最終更新日
*/

using UnityEngine;
using System.Collections;

public class UnUsedTestScript : MonoBehaviour
{

	private void Awake()
	{
		Quaternion a = new Quaternion(0, 0, 0, 0f);
		Quaternion b = new Quaternion(0, 0, 0, 0.5f);
		Quaternion c = new Quaternion(0, 0, 0, 1f);

		a = Quaternion.Euler(new Vector3(360f, 0f, 0f));
		b = Quaternion.Euler(new Vector3(360f, 180f, 0f));
		c = Quaternion.Euler(new Vector3(360f, 0f, 180f));


		print($"a = {a} , b = {b} , c = {c}");
		//print($"a = {a.eulerAngles} , b = {b.eulerAngles} , c = {c.eulerAngles}");
	}
}