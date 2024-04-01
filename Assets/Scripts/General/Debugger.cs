using System.Diagnostics;
using UnityEngine;

[Tooltip("デバッグ用の文字を出すクラス")] 
public static class Debugger
{
    [Conditional("UNITY_EDITOR"), Tooltip("普通のログを出す")]
	public static void Log(object text)
    {
        UnityEngine.Debug.Log(text);
    }

    [Conditional("UNITY_EDITOR"), Tooltip("エラーログを出す")]
    public static void LogError(object text)
    {
        UnityEngine.Debug.LogError(text);
    }
}