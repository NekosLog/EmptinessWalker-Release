/* 制作日 2024/01/30
*　製作者 ニシガキ
*　最終更新日 2024/03/13
*/

using UnityEngine;
using UniRx;

public class PlayerParameter : MonoBehaviour, IFPlayerParameter
{
    #region 変数一覧
    //プレイヤーのHPの初期値
    private int _startPlayerHP = 5;

    //プレイヤーのHPの初期最大値
    private int _startMaxHP = 5;

    // プレイヤーのSPの初期値
    private int _startPlayerSP = 100;

    // プレイヤーのSPの初期最大値
    private int _startMaxSP = 100;

    // プレイヤーの通常攻撃の与ダメージ
    private int _attack_Damage = 2;

    // スキル１の消費SP
    private int _skill1_Cost = 30;

    // スキル１の与ダメージ
    private int _skill1_Damage = 5;

    // 回復の消費SP
    private int _heal_Cost = 50;

    // 回復するHPの量
    private int _heal_Value = 1;
    #endregion

    #region プロパティ一覧
    // 現在のプレイヤーのHPを保持するプロパティ
    private ReactiveProperty<int> _nowPlayerHPProperty = new ReactiveProperty<int>();

    // 現在のプレイヤーのSPを保持するプロパティ
    private ReactiveProperty<int> _nowPlayerSPProperty = new ReactiveProperty<int>();

    // 現在のプレイヤーのHPの最大値を保持するプロパティ
    private ReactiveProperty<int> _maxPlayerHPProperty = new ReactiveProperty<int>();

    // 現在のプレイヤーのSTの最大値を保持するプロパティ
    private ReactiveProperty<int> _maxPlayerSPProperty = new ReactiveProperty<int>();

    /// <summary>
    /// 現在のプレイヤーのHPを管理するプロパティ
    /// </summary>
    public IReadOnlyReactiveProperty<int> NowPlayerHPProperty
    {
        // 現在のプレイヤーのHPを渡す
        get { return _nowPlayerHPProperty; }
    }

    /// <summary>
    /// 現在のプレイヤーのSPを管理するプロパティ
    /// </summary>
    public IReadOnlyReactiveProperty<int> NowPlayerSPProperty
    {
        // 現在のプレイヤーのSPを渡す
        get { return _nowPlayerSPProperty; }
    }


    /// <summary>
    /// 現在のプレイヤーのHPの最大値を管理するプロパティ
    /// </summary>
    public IReadOnlyReactiveProperty<int> MaxPlayerHPProperty
    {
        // 現在のプレイヤーのHPの最大値を渡す
        get { return _maxPlayerHPProperty; }
    }

    /// <summary>
    /// 現在のプレイヤーのSPの最大値を管理するプロパティ
    /// </summary>
    public IReadOnlyReactiveProperty<int> MaxPlayerSPProperty
    {
        // 現在のプレイヤーのSPの最大値を渡す
        get { return _maxPlayerSPProperty; }
    }

    /// <summary>
    /// 現在のプレイヤーのHPを渡すためのプロパティ
    /// </summary>
    public int GetPlayerHp
    {
        // 現在のプレイヤーのHPを渡す
        get { return _nowPlayerHPProperty.Value; }
    }

    /// <summary>
    /// 現在のプレイヤーのHPが上限値と同じかどうか
    /// </summary>
    public bool IsHPMax
    {
        // 現在のHPが上限値以上ならTrue、上限値未満ならFalseを返す
        get { return _nowPlayerHPProperty.Value >= _maxPlayerHPProperty.Value; }
    }

    /// <summary>
    /// 現在のプレイヤーのSPを渡すためのプロパティ
    /// </summary>
    public int GetPlayerSp
    {
        // 現在のプレイヤーのSPを渡す
        get { return _nowPlayerSPProperty.Value; }
    }

    /// <summary>
    /// 現在のプレイヤーのSPが上限値と同じかどうか
    /// </summary>
    public bool IsSPMax
    {
        // 現在のSPが上限値以上ならTrue、上限値未満ならFalseを返す
        get { return _nowPlayerSPProperty.Value >= _maxPlayerSPProperty.Value; }
    }

    /// <summary>
    /// プレイヤーの攻撃で与えられるダメージ量を取得するためのプロパティ
    /// </summary>
    public int GetAttack_Damage
    {
        // プレイヤーの通常攻撃のダメージ量を渡す
        get { return _attack_Damage; }
    }

    /// <summary>
    /// プレイヤーのスキル１で消費するSPの量を取得するためのプロパティ
    /// </summary>
    public int GetSkill1_Cost
    {
        // プレイヤーのスキル１で消費するSPの量を渡す
        get { return _skill1_Cost; }
    }

    /// <summary>
    /// プレイヤーのスキル１で与えられるダメージ量を取得するためのプロパティ
    /// </summary>
    public int GetSkill1_Damage
    {
        // プレイヤーのスキル１のダメージ量を渡す
        get { return _skill1_Damage; }
    }

    /// <summary>
    /// プレイヤーの回復で消費するSPの量を取得するためのプロパティ
    /// </summary>
    public int GetHeal_Cost
    {
        // プレイヤーの回復で消費するSPの量を渡す
        get { return _heal_Cost; }
    }

    /// <summary>
    /// プレイヤーの回復で増えるHPの量を取得するためのプロパティ
    /// </summary>
    public int GetHeal_Value
    {
        // プレイヤーの回復で増えるHPの量を渡す
        get { return _heal_Value; }
    }
    #endregion

    #region メソッド一覧
    private void Awake()
    {
        // HPの初期値を設定
        _maxPlayerHPProperty.Value = _startMaxHP;
        _nowPlayerHPProperty.Value = _startPlayerHP;

        // SPの初期値を設定
        _maxPlayerSPProperty.Value = _startMaxSP;
        _nowPlayerSPProperty.Value = _startPlayerSP;
    }

    /// <summary>
    /// 現在のプレイヤーのHPに加算するメソッド
    /// </summary>
    /// <param name="value">加算する値</param>
    public void AddPlayerHp(int value)
    {
        // 値を加算
        int setValue = _nowPlayerHPProperty.Value + value;

        // 上限を超えないように制限
        setValue = Mathf.Clamp(setValue, 0, _maxPlayerHPProperty.Value);

        // 値を代入
        _nowPlayerHPProperty.Value = setValue;
    }

    /// <summary>
    /// 現在のプレイヤーのSPに加算するメソッド
    /// </summary>
    /// <param name="value">加算する値</param>
    public void AddPlayerSp(int value)
    {
        // 値を加算
        int setValue = _nowPlayerSPProperty.Value + value;

        // 上限を超えないように制限
        setValue = Mathf.Clamp(setValue, 0, _maxPlayerSPProperty.Value);

        // 値を代入
        _nowPlayerSPProperty.Value = setValue;
    }
    #endregion
}