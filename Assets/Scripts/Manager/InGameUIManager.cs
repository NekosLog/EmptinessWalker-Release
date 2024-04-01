/* 制作日 2024/02/05
*　製作者 ニシガキ
*　最終更新日 2024/02/05
*/

using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
 
public class InGameUIManager : MonoBehaviour, IFInGameUIManager
{
    [SerializeField, Tooltip("有効状態のHPポインター")]
    private Sprite _validPointer = default;

    [SerializeField, Tooltip("無効状態のHPポインター")]
    private Sprite _invalidPointer = default;

    [SerializeField, Tooltip("HPポインターの左端の位置")]
    private RectTransform _hpPointerStartPosition = default;

    [SerializeField, Tooltip("SPゲージのImage")]
	private Image _playerSPGage = default;

    // プレイヤーの状態を取得するためのインターフェース
    private IFPlayerParameter _playerParameter = default;

    // プレイヤーのHP量を表すポインターたち
    private List<Image> _playerHPPointers = new List<Image>();

    // SPゲージを変化させるためのクラス
    private SPGageGradualChange _spGageGradualChanger = default;

	// プレイヤーの現在のHPの最大値　表示しているポインターの個数
	private int _playerMaxHP = default;

    // プレイヤーのHPの最大値の上限　ポインターの総数
    private int _hpPointerLimit = default;

	// プレイヤーの残りHP　映しているポインターの数
	private int _playerHPValue = default;

	// プレイヤーのSPの現在値　増減の判定に使用
	private float _playerSPRatio = 1;

    // SPの最大値
    private int _playerMaxSP = default;

    // HPポインターの設置間隔
    private const float HP_POINTER_DISTANCE = 100f;

    private void Awake()
    {
        // プレイヤーのパラメータを取得するためのインターフェースを取得する
        _playerParameter = GameObject.FindWithTag("PlayerParent").GetComponent<IFPlayerParameter>();

        // 各パラメータの変化を検知してUIを変更できるようにする
        _playerParameter.MaxPlayerHPProperty.Subscribe(maxHP => { SetMaxHPUI(maxHP); }).AddTo(this);
        _playerParameter.MaxPlayerSPProperty.Subscribe(maxSP => { SetMaxSPUI(maxSP); }).AddTo(this);
        _playerParameter.NowPlayerHPProperty.Subscribe(nowHP => { ChengePlayerHPUI(nowHP); }).AddTo(this);
        _playerParameter.NowPlayerSPProperty.Subscribe(nowSP => { ChengePlayerSPUI(nowSP); }).AddTo(this);
    }

    /// <summary>
    /// 最大HPを設定するメソッド
    /// </summary>
    /// <param name="maxHP">プレイヤーの最大HP</param>
    public void SetMaxHPUI(int maxHP)
    {
        // リストの設定をする時の最初のインデックス
        int startIndex;

        // 初期設定を行っていなければ初期設定を行う
        if (_playerHPPointers.Count == 0)
        {
            // 配列の最初から
            startIndex = 0;

            // ポインターのゲームオブジェクトを全取得する
            GameObject[] pointerObjects = GameObject.FindGameObjectsWithTag("HPPointer");

            // ポインターの総数を設定
            _hpPointerLimit = pointerObjects.Length;

            // ポインターの設置開始位置をVector2に直す
            Vector2 startPosition = _hpPointerStartPosition.position;

            // リストにポインターを設定していく
            for (int i = 0; i < _hpPointerLimit; i++)
            {
                // ポインターのImageを設定
                _playerHPPointers.Add(pointerObjects[i].GetComponent<Image>());

                // ポインターの位置を設定
                _playerHPPointers[i].rectTransform.position = startPosition + (Vector2.right * HP_POINTER_DISTANCE * i);

                // ポインターの表示を隠す
                _playerHPPointers[i].gameObject.SetActive(false);
            }

            // 現在の最大値を設定
            _playerMaxHP = 0;
        }

        // ポインターの総数を超えていないか判定
        if (_hpPointerLimit < maxHP)
        {
            Debug.LogError("ポインターの総数を超えている");
            return;
        }

        // 最大値をすでに超えていた場合
        if (_playerMaxHP > maxHP)
        {
            // 最大値を設定
            startIndex = maxHP;

            // 最大値より大きいポインターを消す
            for (int i = startIndex; i < _playerMaxHP; i++)
            {
                _playerHPPointers[i].gameObject.SetActive(false);
            }

            return;
        }
        // 最大値を超えていない場合
        else
        {
            // 配列の続きから
			startIndex = _playerMaxHP;
        }

        // 最大値を更新
		_playerMaxHP = maxHP;

        // ポインターの表示を増やす
        for (int i = startIndex; i < maxHP; i++)
        {
            _playerHPPointers[i].gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// 残りHPを設定するメソッド
    /// </summary>
    /// <param name="nowHP">プレイヤーの現在のHP</param>
	public void ChengePlayerHPUI(int nowHP)
    {
        // HPが正常値かどうかの判定
        if (nowHP > _playerMaxHP && nowHP < 0)
        {
            // 引数に異常あり
            Debug.LogError("残りHPおかしいかも");
            return;
        }

        // リストに合わせるために１を引く
        int nowHPIndex = nowHP - 1;

        // 増減の判定
        if (_playerHPValue < nowHP)
        {
            // 回復時のUI変更

            // ポインターを有効にしていく
            for (int i = _playerHPValue; i <= nowHPIndex; i++)
            {
                _playerHPPointers[i].sprite = _validPointer;
            }

            // インデックスの位置を設定する
            _playerHPValue = nowHP;
        }
        else if(_playerHPValue > nowHP)
        {
            // ダメージを受けた時のUI変更

            // ポインターを無効にしていく
            for (int i = _playerHPValue -1; i > nowHPIndex; i--)
            {
                _playerHPPointers[i].sprite = _invalidPointer;
            }

            // インデックスの位置を設定する
            _playerHPValue = nowHP;
        }
        else
        {
            // 増減がない場合
            Debug.LogError("ノーダメージ");
            return;
        }
    }

    /// <summary>
    /// 最大SPを設定するメソッド
    /// </summary>
    /// <param name="maxSP">プレイヤーの最大SP</param>
    public void SetMaxSPUI(int maxSP)
    {
        // SPの最大値を設定
        _playerMaxSP = maxSP;
    }

    /// <summary>
    /// 残りSPを設定するメソッド
    /// </summary>
    /// <param name="nowSP">残りSP</param>
	public void ChengePlayerSPUI(float nowSP)
    {
        // もらった値が正しい値の範囲内か判定
        if (nowSP > _playerMaxSP)
        {
            // 上限を超えている
            nowSP = _playerMaxSP;
            Debug.LogError("SP多すぎ");
        }
        else if(nowSP < 0)
        {
            nowSP = 0;
            Debug.LogError("SPがマイナスになってる");
        }

        // SP量を割合に変換
        float nowRatio = nowSP / _playerMaxSP;

        // SPゲージを変化させるクラスがない時はインスタンスする
        if (_spGageGradualChanger == null)
        {
            // クラスのインスタンス
            _spGageGradualChanger = gameObject.AddComponent<SPGageGradualChange>();

            // 必要な値を渡して変化スタート
            _spGageGradualChanger.StartChanging(_playerSPGage, _playerSPRatio, nowRatio);
        }
        // ある時はゲージの残量だけ設定する
        else
        {
            // ゲージの残量を変更
            _spGageGradualChanger.StartChanging(nowRatio);
        }

        // 現在の値を変更
        _playerSPRatio = nowRatio;
    }
}