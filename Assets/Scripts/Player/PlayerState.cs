/* 制作日 2024/02/18
*　製作者 ニシガキ
*　最終更新日 2024/03/13
*/

using UnityEngine;
using System.Collections;
using UniRx;

public class PlayerState : MonoBehaviour, IFPlayerState, IFPlayerPassiveState
{
    #region 変数一覧
    [SerializeField, Tooltip("プレイヤーの着地状態を判定するインターフェース")]
    private IFPlayerLanding _playerLanding = default;

    [SerializeField, Tooltip("プレイヤーの行動アニメーションの実行用インターフェース")]
    private IFPlayerActiveAnimation _playerActiveAnimation = default;

    /// <summary>
    /// プレイヤーの行動フラグを示す構造体
    /// </summary>
    private struct PlayerStateFlag
    {
        // フラグの種類
        public readonly E_PlayerState state;

        // フラグの状態
        public bool flag;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="state">State</param>
        /// <param name="flag">フラグの状態</param>
        public PlayerStateFlag(E_PlayerState state, bool flag)
        {
            this.state = state;
            this.flag = flag;
        }
    }

    // プレイヤーの行動フラグを管理する配列
    private PlayerStateFlag[] _stateFlagTable = new PlayerStateFlag[10]
    {
        new PlayerStateFlag(E_PlayerState.Wait, true),
        new PlayerStateFlag(E_PlayerState.Walk, false),
        new PlayerStateFlag(E_PlayerState.Fall, false),
        new PlayerStateFlag(E_PlayerState.Jump, false),
        new PlayerStateFlag(E_PlayerState.Dash, false),
        new PlayerStateFlag(E_PlayerState.Action, false),
        new PlayerStateFlag(E_PlayerState.Attack, false),
        new PlayerStateFlag(E_PlayerState.Heal, false),
        new PlayerStateFlag(E_PlayerState.Skill1, false),
        new PlayerStateFlag(E_PlayerState.Skill2, false),
    };

    // 何段目の攻撃かのカウント
    private int _attackCount = 0;

    // 攻撃の段数　現在は２段
    private const int ATTACK_MAX_COUNT = 2;
    #endregion

    #region プロパティ
    // プレイヤーの行動状態を保持するプロパティ　アニメーション管理に使用
    private ReactiveProperty<E_PlayerState> _nowPasseiveState = new ReactiveProperty<E_PlayerState>();

    // 攻撃を行えるかどうかの判定
    private ReactiveProperty<bool> _attackPossibility = new ReactiveProperty<bool>();

    // 攻撃以外のアクティブアニメーションを行えるかどうかの判定
    private ReactiveProperty<bool> _activeActionPossibility = new ReactiveProperty<bool>();

    /// <summary>
    /// プレイヤーの現在の状態を管理するプロパティ<br />
    /// アニメーションで使用
    /// </summary>
    public IReadOnlyReactiveProperty<E_PlayerState> OnChengePassiveStateProperty
    {
        get { return _nowPasseiveState; }
    }

    /// <summary>
    /// 攻撃を行えるかどうかの判定を管理するプロパティ<br />
    /// PlayerMoveで使用
    /// </summary>
    public IReadOnlyReactiveProperty<bool> AttackAssessmentProperty
    {
        get { return _attackPossibility; }
    }

    /// <summary>
    /// 攻撃以外のアクティブアニメーションを行えるかどうかの判定を管理するプロパティ<br />
    /// PlayerMoveで使用
    /// </summary>
    public IReadOnlyReactiveProperty<bool> ActiveActionAssessmentProperty
    {
        get { return _activeActionPossibility; }
    }

    /// <summary>
    /// PlayerStateを取得するためのプロパティ
    /// </summary>
    /// <param name="state">取得するState</param>
    /// <returns></returns>
    public bool GetState(E_PlayerState state)
    {
        // 取得するStateを選定
        for (int i = 0; i < _stateFlagTable.Length; i++)
        {
            // 一致するStateを探索
            if (state == _stateFlagTable[i].state)
            {
                // 一致したStateのフラグを返す
                return _stateFlagTable[i].flag;
            }
        }

        // 例外処理
        Debug.LogError("PlayerStateに異常あり");
        return false;
    }
    #endregion

    #region メソッド一覧
    private void Start()
    {
        // 必要なインターフェースを取得する
        _playerLanding = GetComponent<IFPlayerLanding>();
        _playerActiveAnimation = GetComponent<IFPlayerActiveAnimation>();

        // プレイヤーの着地状態を取得できるようにする
        _playerLanding.OnChengeIsLandingProperty.Subscribe
            (isLanding => { ChengeFallFlag(isLanding); }).AddTo(this);

        // 今の状態を更新
        OnChengePassiveState();

        // 各行動の可否を更新
        CheckMovePossibility();
    }

    /// <summary>
    /// 指定したStateのフラグをオンにするメソッド
    /// </summary>
    /// <param name="state">指定するState</param>
    public void OnStateFlag(E_PlayerState state)
    {
        // Stateに応じたActiveAnimationが無いか確認する
        CheckActiveAnimation(state);

        // 取得するStateを選定
        for (int i = 0; i < _stateFlagTable.Length; i++)
        {
            // 一致するStateを探索
            if (state == _stateFlagTable[i].state)
            {
                // 一致したStateのフラグを変更
                _stateFlagTable[i].flag = true;

                // 今の状態を更新
                OnChengePassiveState();

                // 各行動の可否を更新
                CheckMovePossibility();
            }
        }
    }

    /// <summary>
    /// 指定したStateのフラグを待機時間後にオンにするメソッド
    /// </summary>
    /// <param name="state">指定するState</param>
    /// <param name="delayTime">待機時間</param>
    /// <return>予約したコルーチン</return>
    public Coroutine OnStateFlag(E_PlayerState state, float delayTime)
    {
        // コルーチンに引数を渡して実行開始
        return StartCoroutine(DelaySetOn(state, delayTime));
    }

    /// <summary>
    /// 指定したStateのフラグをオフにするメソッド
    /// </summary>
    /// <param name="state">指定するState</param>
    public void OffStateFlag(E_PlayerState state)
    {
        // 取得するStateを選定
        for (int i = 0; i < _stateFlagTable.Length; i++)
        {
            // 一致するStateを探索
            if (state == _stateFlagTable[i].state)
            {
                // 一致したStateのフラグを変更
                _stateFlagTable[i].flag = false;

                // 今の状態を更新
                OnChengePassiveState();

                // 各行動の可否を更新
                CheckMovePossibility();
            }
        }
    }

    /// <summary>
    /// 指定したStateのフラグを待機時間後にオフにするメソッド
    /// </summary>
    /// <param name="state">指定するState</param>
    /// <param name="delayTime">待機時間</param>
    /// <return>予約したコルーチン</return>
    public Coroutine OffStateFlag(E_PlayerState state, float delayTime)
    {
        // コルーチンに引数を渡して実行開始
        return StartCoroutine(DelaySetOff(state, delayTime));
    }

    /// <summary>
    /// 予約していたコルーチンを停止するためのメソッド
    /// </summary>
    /// <param name="cancelCoroutine"></param>
    public void DelayCancel(Coroutine cancelCoroutine)
    {
        StopCoroutine(cancelCoroutine);
    }

    /// <summary>
    /// プレイヤーの着地状態を変更するメソッド
    /// </summary>
    /// <param name="isLanding">着地しているかどうかの判定</param>
    private void ChengeFallFlag(bool isLanding)
    {
        // 着地状態を変更
        _stateFlagTable[(int)E_PlayerState.Fall].flag = !isLanding;

        // 今の状態を更新
        OnChengePassiveState();
    }

    /// <summary>
    /// フラグが変更されたときに現在の状態に変更がないか判定するメソッド
    /// </summary>
    /// <returns>今の状態</returns>
    private E_PlayerState OnChengePassiveState()
    {
        // 返す値
        E_PlayerState returnState = default;

        // 回復中かどうか
        if (_stateFlagTable[(int)E_PlayerState.Heal].flag == true)
        {
            // 回復モーションに移行
            returnState = E_PlayerState.Heal;
        }

        // 落下中かどうか
        else if (_stateFlagTable[(int)E_PlayerState.Fall].flag == true)
        {
            // 落下モーションに移行
            returnState = E_PlayerState.Fall;
        }

        // 歩行中かどうか
        else if (_stateFlagTable[(int)E_PlayerState.Walk].flag == true)
        {
            // 歩行モーションに移行
            returnState = E_PlayerState.Walk;
        }

        // その他
        else
        {
            // 待機モーションに移行
            returnState = E_PlayerState.Wait;
        }

        // 今の状態を更新
        _nowPasseiveState.Value = returnState;

        // 今の状態を返す
        return returnState;
    }

    /// <summary>
    /// ActiveAnimationの種類を判断してアニメーションを実行するメソッド
    /// </summary>
    /// <param name="state">実行する行動の種類</param>
    private void CheckActiveAnimation(E_PlayerState state)
    {
        // 実行するアニメーション
        E_PlayerAnimation animation = default;

        // 種類を判断
        switch (state)
        {
            // ジャンプ
            case E_PlayerState.Jump:
                // 地上ジャンプか空中ジャンプか判定
                if (_stateFlagTable[(int)E_PlayerState.Fall].flag == false)
                {
                    // 地上ジャンプ
                    animation = E_PlayerAnimation.Jump;
                }
                else
                {
                    // 空中ジャンプ
                    animation = E_PlayerAnimation.AirJump;
                }
                break;

            // 攻撃
            case E_PlayerState.Attack:
                // 何回目の攻撃か判定
                animation = AttackCountor();
                break;

            // スキル１
            case E_PlayerState.Skill1:
                animation = E_PlayerAnimation.Skill1;
                break;

            // ダッシュ
            case E_PlayerState.Dash:
                animation = E_PlayerAnimation.Dash;
                break;

            // その他
            default:
                // ActiveAnimation以外の場合何もしない
                return;
        }

        // アニメーションを実行
        _playerActiveAnimation.ExecutionActiveAnimation(animation);
    }

    /// <summary>
    /// 何段目の攻撃アニメーションか判定するメソッド
    /// </summary>
    /// <returns>攻撃アニメーション</returns>
    private E_PlayerAnimation AttackCountor()
    {
        // 返すアニメーション
        E_PlayerAnimation returnAnimation = default;

        // 何回目の攻撃か判定
        switch (_attackCount)
        {
            // １段目
            case 0:
                returnAnimation = E_PlayerAnimation.Attack1;
                break;

            // ２段目
            case 1:
                returnAnimation = E_PlayerAnimation.Attack2;
                break;

            // 例外処理
            default:
                Debug.LogError("アタックカウントが異常");
                break;
        }

        // カウントを増やす
        _attackCount++;

        // カウントが上限を超えていないか
        if (_attackCount >= ATTACK_MAX_COUNT)
        {
            // カウントをリセットする
            AttackCountReflesh();
        }

        // アニメーションを返す
        return returnAnimation;
    }

    /// <summary>
    /// 攻撃回数のカウントをリセットするメソッド
    /// </summary>
    private void AttackCountReflesh()
    {
        // カウントを初期値にリセットする
        _attackCount = 0;
    }

    /// <summary>
    /// 各行動が可能かどうかの判定を更新するメソッド
    /// </summary>
    private void CheckMovePossibility()
    {
        /*  フラグの状態から各行動を行える状態か判断する
         *  攻撃を行える状態
         *  →　［ ダッシュ・回復・スキル１・スキル２ を行っていないとき］
         *  
         *  攻撃以外の行動を行える状態
         *  →　［ 攻撃を行える状態　かつ　 攻撃　を行っていないとき］
         */

        // 攻撃を行えるかどうか判定する
        _attackPossibility.Value =
           (_stateFlagTable[(int)E_PlayerState.Dash].flag ||
            _stateFlagTable[(int)E_PlayerState.Heal].flag ||
            _stateFlagTable[(int)E_PlayerState.Skill1].flag ||
            _stateFlagTable[(int)E_PlayerState.Skill2].flag)
             == false;

        // 攻撃以外のアクティブアニメーションを行えるかどうか判定する
        _activeActionPossibility.Value =
           (_attackPossibility.Value &&
            _stateFlagTable[(int)E_PlayerState.Attack].flag == false);
    }
    #endregion

    #region コルーチン一覧
    /// <summary>
    /// 指定された時間待ってからStateをオンにするコルーチン
    /// </summary>
    /// <param name="state">設定するState</param>
    /// <param name="delayTime">待機時間</param>
    private IEnumerator DelaySetOn(E_PlayerState state, float delayTime)
    {
        // 指定された時間待つ
        yield return new WaitForSeconds(delayTime);

        // Stateをオンにする
        OnStateFlag(state);
    }

    /// <summary>
    /// 指定された時間待ってからStateをオフにするコルーチン
    /// </summary>
    /// <param name="state">設定するState</param>
    /// <param name="delayTime">待機時間</param>
    private IEnumerator DelaySetOff(E_PlayerState state, float delayTime)
    {
        // 指定された時間待つ
        yield return new WaitForSeconds(delayTime);

        // Stateをオフにする
        OffStateFlag(state);
    }
    #endregion
}