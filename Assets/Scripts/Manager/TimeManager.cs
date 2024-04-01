/* 制作日
*　製作者
*　最終更新日
*/

using UnityEngine;
using System.Collections;

static public class TimeManager{
    // コルーチンを使うためにMonoBehaviourを生成する
    static private MonoBehaviour coroutineHandler = new MonoBehaviour();

    // タイムスケール　時間の進み
    static private float _timeScale = default;

    // 遅延時間　遅らせて処理を行うときに使用
    static private float _delayTime = default;

    // 経過時間を伝えるプロパティ　１秒で１
    static public float deltaTime { get { return Time.deltaTime * _timeScale; } }

    // 時間の進みを伝えるプロパティ
    static public float timeScale { get { return _timeScale * _timeScale; } }

    // 時間を止める
    public static void StopTime()
    {
        _timeScale = 0;
    }

    // 時間を進める
    public static void StartTime()
    {
        _timeScale = 1;
    }

    // 時間を止めて、遅らせて時間を進める
    public static void StopAndDelayStartTime(float delayTime)
    {
        StopTime();
        _delayTime = delayTime;
        coroutineHandler.StartCoroutine("DelayStartCoroutine");
    }

    // 遅らせて時間を進めるコルーチン用関数
    private static IEnumerator DelayStartCoroutine()
    {
        yield return new WaitForSeconds(_delayTime);
        StartTime();
    }
}