/* 制作日 2023/12/12
*　製作者 ニシガキ
*　最終更新日 2023/12/12
*/
/*

using System.Collections;
using UnityEngine;
 
public class PlayerMove : MoveBase, IFPlayerMove, IFLandingEvent
{
    #region フィールド変数

    #region 定数
    // プレイヤーの左右移動速度
    private const float PLAYER_MALK_SPEED = 6f;

    // プレイヤーの跳躍力
    private const float PLAYER_JUMP_POWER = 15f;

    // プレイヤーのダッシュ速度
    private const float PLAYER_DASH_SPEED = 20f;

    // ダッシュの再使用までの時間
    private const float DASH_INTERVAL_TIME = 0.6f;

    // 攻撃の再使用までの時間
    private const float ATTACK_INTERVAL_TIME = 0.25f;

    // 攻撃後の行動可能になるまでの時間　攻撃後の硬直
    private const float ATTACK_STIFFENING = 0.3f;

    // 攻撃判定の持続時間
    private const float ATTACK_LIFE_TIME = 0.06f;

    // スキル１後の再行動可能になるまでの時間　硬直
    private const float SKILL1_STIFFENING = 0.3f;

    // プレイヤーの１回あたりの回復完了までの時間
    private const float HEAL_COMPLETE_TIME = 1f;

    #endregion

    #region シリアライズ
    [SerializeField, Tooltip("プレイヤーの横幅")]
    private float _playerDepth = default;

    [SerializeField, Tooltip("プレイヤーの縦幅")]
    private float _playerHeight = default;

    [SerializeField, Tooltip("接触判定の細かさ")]
    private int _checkValue = default;

    [SerializeField, Tooltip("攻撃の判定コライダー")]
    private GameObject _attackCollider = default;

    [SerializeField, Tooltip("攻撃のコライダーの位置")]
    private Transform _attackColliderPosition = default;

    [SerializeField, Tooltip("スキル１で発射する球")]
    private GameObject _skill1_Bullet = default;
    #endregion

    #region インターフェース
    // PlayerStateのインターフェース
    private IFPlayerState _playerState = default;

    // Fallのインターフェース
    private IFFall _fall = default;

    // PlayerParameterのインターフェース
    private IFPlayerParameter _playerParameter = default;
    #endregion

    // プレイヤーのトランスフォーム
    private Transform _player = default;

    // プレイヤーの歩行の移動加算量
    private Vector2 _walkValue = new Vector2(PLAYER_MALK_SPEED, 0);

    // プレイヤーのダッシュの移動加算量
    private Vector2 _dashValue = new Vector2(PLAYER_DASH_SPEED, 0);

    // ダッシュの継続時間
    private float _dashTime = 0.2f;

    // ダッシュの使用間隔を測る変数
    private float _dashInterval = 0f;

    // ダッシュ開始からの経過時間
    private float _nowDashTime = 0f;

    // プレイヤーが向いている正面方向
    private int _playerForword = 1;

    // 二段ジャンプができるかどうかのフラグ
    private bool _canDoubleJump = true;

    // ジャンプトークン　二段ジャンプを使用したかどうか
    private bool _jumpToken = false;

    // ダッシュ中フラグ　trueでダッシュ中
    private bool _nowDash = false;

    // プレイヤーが回復を開始しているかどうかの確認フラグ
    private bool _isStartHeal = false;

    // 攻撃の使用間隔を測る変数
    private float _attackInterval = default;

    // 攻撃後の再行動可能になるまでの時間を測る変数
    private float _attackStiffening = default;

    // プレイヤーが回復を行っている時間
    private float _playerHealTime = default;

    #endregion

    private void Start()
    {
        // プレイヤーのトランスフォームを取得
        _player = this.transform;

        // _fallに自身が持つFallを代入
        _fall = this.GetComponent<IFFall>();

        // _playerStateに自身が持つPlayerStateを代入
        _playerState = this.GetComponent<PlayerState>();
        // _playerParameterに自身が持つPlayerParameterを代入
        _playerParameter = this.GetComponent<PlayerParameter>();

        // CheckFloorの初期値設定
        CheckFloor.SetObjectSize(_playerDepth, _playerHeight, _checkValue);
        // CheckCeilingの初期設定
        CheckCeiling.SetObjectSize(_playerDepth, _playerHeight, _checkValue);
        // CheckWallの初期値設定
        CheckWall.SetValue(_playerHeight, _playerDepth, _checkValue);
    }

    /// <summary>
    /// 時間の計測と継続処理
    /// </summary>
    private void Update()
    {
        // 時間を測る
        TimeCount();

        // ダッシュを行う
        Dash(_dashTime);

        // 落下処理
        _fall.FallObject(CheckFloor.CheckLanding(), CheckCeiling.CheckHitCeiling());
        
    }

    /// <summary>
    /// 時間の計測を行うメソッド
    /// </summary>
    private void TimeCount()
    {
        // ダッシュのインターバルのカウントダウン
        if (_dashInterval > 0)
        {
            // 時間をカウント
            _dashInterval -= Time.deltaTime;
        }

        // 攻撃のインターバルのカウントダウン
        if (_attackInterval > 0)
        {
            // 時間をカウント
            _attackInterval -= Time.deltaTime;
        }

        // 攻撃後の再行動可能になるまでの時間のカウントダウン
        if (_attackStiffening > 0)
        {
            // 時間をカウント
            _attackStiffening -= Time.deltaTime;
        }
    }

    /// <summary>
    /// プレイヤーの左右移動を行うメソッド
    /// </summary>
    /// <param name="inputType">入力されたキー</param>
    public void PlayerWalking(E_InputType inputType)
    {
        // 移動方向
        int direction;

        // 入力から移動方向を設定
        // 右方向
        if (inputType == E_InputType.Right)
        {
            // プレイヤーの向き
            Quaternion lookVector = Quaternion.Euler(Vector3.zero);

            // プレイヤーの向きを設定
            SetRotate(lookVector);

            // 壁と接触しているかどうか
            if (!CheckWall.CheckHit(E_InputType.Right))
            {
                // 右に移動
                direction = 1;
                _playerForword = 1;
            }
            else
            {
                // 壁と接触中
                direction = 0;
            }
        }
        // 左方向
        else if (inputType == E_InputType.Left)
        {
            // プレイヤーの向き
            Quaternion lookVector = Quaternion.Euler(new Vector3(0, 180, 0));
            
            // プレイヤーの向きを設定
            SetRotate(lookVector);

            // 壁と接触しているかどうか
            if (!CheckWall.CheckHit(E_InputType.Left))
            {
                // 左に移動
                direction = -1;
                _playerForword = -1;
            }
            else
            {
                // 壁と接触中
                direction = 0;
            }
        }
        else
        {
            // エラー入力
            direction = 0;
            Debugger.LogError("移動入力に異常あり:入力が左右以外");
        }

        // 歩ける状態のときのみ
        if (_playerState.GetState(E_PlayerState.Walk))
        {
            // プレイヤーを移動
            _player.position += (Vector3)_walkValue * direction * Time.deltaTime;
        }
    }

    /// <summary>
    /// プレイヤーの角度を設定するメソッド
    /// </summary>
    /// <param name="lookVector"></param>
    private void SetRotate(Quaternion lookVector)
    {
        // ステータスから回転できるか判定
        if (_playerState.GetState(E_PlayerState.Rotate))
        {
            // プレイヤーの向きが違う場合
            if (transform.rotation != lookVector)
            {
                // プレイヤーの向きを代入
                transform.rotation = lookVector;
            }
        }
    }

    /// <summary>
    /// プレイヤーのダッシュを行うメソッド
    /// </summary>
    public void PlayerDash()
    {
        // ダッシュができる かつ インターバル中かどうか
        if (_playerState.GetState(E_PlayerState.Dash) && _dashInterval <= 0)
        {
            // ダッシュ中に設定
            _nowDash = true;

            // 操作入力を無効化
            _playerState.SetAllState(false);

            // 一定時間後に操作入力を有効化
            _playerState.SetAllState(true, _dashTime);

            // ダッシュのインターバル設定
            _dashInterval = DASH_INTERVAL_TIME;
        }
    }

    /// <summary>
    /// ダッシュ中の処理
    /// </summary>
    /// <param name="dashTime">ダッシュの最大継続時間</param>
    private void Dash(float dashTime)
    {
        // ダッシュ中のみ処理
        if (_nowDash)
        {
            // 継続時間のカウント
            _nowDashTime += Time.deltaTime;

            // 壁と衝突していなければ前方へダッシュ
            if (_playerForword == 1 && CheckWall.CheckHit(E_InputType.Right) || _playerForword == -1 && CheckWall.CheckHit(E_InputType.Left))
            {
                // 壁と衝突
            }
            else
            {
                // ダッシュの移動加算
                _player.position += (Vector3)_dashValue * _playerForword * Time.deltaTime;
            }
            // 落下の無効化
            _fall.SetFallValue(0);

            // 時間になったら終了
            if (_nowDashTime > dashTime)
            {
                // ダッシュフラグを消す
                _nowDash = false;

                // 経過時間のリセット
                _nowDashTime = 0f;
            }
        }
    }

    /// <summary>
    /// プレイヤーの落下や上昇を行うメソッド
    /// </summary>
    public void PlayerJump()
    {
        // ジャンプが可能かどうか
        if (_playerState.GetState(E_PlayerState.Jump))
        {
            // 二段ジャンプができるかどうか
            if (_canDoubleJump)
            {
                // ジャンプトークンがある場合
                if (_jumpToken)
                {
                    // 上昇力を設定
                    _fall.SetFallValue(PLAYER_JUMP_POWER);

                    // 空中の場合 ジャンプトークンを消費
                    if (!CheckFloor.CheckLanding())
                    {
                        // ジャンプトークンを消費
                        _jumpToken = false;
                    }
                }

            }
            else
            {
                // 着地しているかどうか
                if (CheckFloor.CheckLanding())
                {
                    // 上昇力を設定
                    _fall.SetFallValue(PLAYER_JUMP_POWER);
                }
            }
        }
    }

    /// <summary>
    /// 着地時の処理
    /// </summary>
    public void LandingEvent()
    {
        // ジャンプトークンの回復
        _jumpToken = true;

        // 攻撃中に着地したら移動不可にする
        if (_attackStiffening > 0)
        {
            // 移動不可に設定
            _playerState.SetState(E_PlayerState.Walk, false);
        }
    }

    /// <summary>
    /// プレイヤーの攻撃を行うメソッド
    /// </summary>
    public void PlayerAttack()
    {
        // 攻撃可能 かつ インターバル中ではない
        if (_playerState.GetState(E_PlayerState.Attack) && _attackInterval <= 0)
        {
            // 再攻撃までの時間を設定
            _attackInterval = ATTACK_INTERVAL_TIME;

            // 攻撃後の再行動可能になるまでの時間を設定
            _attackStiffening = ATTACK_STIFFENING;

            // プレイヤーのステータスを設定　攻撃だけtrue その他はfalse
            _playerState.SetAllState(false) ;
            _playerState.SetState(E_PlayerState.Attack, true);

            // 空中だった場合、移動のみ許可する
            if (!CheckFloor.CheckLanding())
            {
                // 歩行のみtrueに設定
                _playerState.SetState(E_PlayerState.Walk, true);
            }

            // 攻撃硬直が終わったらステータスを戻す
            _playerState.SetAllState(true, ATTACK_STIFFENING);

            // 攻撃を実行
            StartCoroutine(Attack());
        }
    }

    /// <summary>
    /// プレイヤーのスキルを発動するメソッド
    /// </summary>
    /// <param name="skillType">スキルの番号</param>
    public void PlayerSkill(E_InputType skillType)
    {
        // スキルタイプで派生
        switch (skillType)
        {
            // スキル１
            case E_InputType.Skill1:
                Skill1();
                break;

            // スキル２　未実装
            case E_InputType.Skill2:
                // Skill2を実装
                break;
        }

    }

    /// <summary>
    /// スキル１に登録されているスキルを発動するメソッド<br />
    /// いずれはイベントに変えてスキル変更可能にしたい
    /// </summary>
    private void Skill1()
    {
        // スキル１を使用可能のとき
        if (_playerState.GetState(E_PlayerState.Skill1))
        {
            // スキル１の消費SPが足りているとき
            if (_playerParameter.GetPlayerSp() >= _playerParameter.GetSkill1_Cost())
            {
                // プレイヤーの向き
                Quaternion lookVector = default;

                // 右
                int right = 1;

                // 左
                int left = -1;

                // 左右の判定
                if (_playerForword == right)
                {
                    // 右を向いているときの向き
                    lookVector = Quaternion.Euler(Vector3.zero);
                }
                else if (_playerForword == left)
                {
                    // 左を向いているときの向き
                    lookVector = Quaternion.Euler(new Vector3(0, 180, 0));
                }

                // 弾を生成
                Instantiate(_skill1_Bullet, transform.position, lookVector);

                // 発動中は移動不能にして終了したら行動可能にする
                _playerState.SetAllState(false);
                _playerState.SetAllState(true, SKILL1_STIFFENING);

                // 消費SPの分プレイヤーのSPを減らす
                _playerParameter.AddPlayerSp(-_playerParameter.GetSkill1_Cost());
            }
            // SP不足
            else
            {
                Debugger.Log("SP足りない");
            }
        }
    }


    /// <summary>
    /// プレイヤーの回復を行うメソッド
    /// </summary>
    public void PlayerHeal()
    {
        // 回復を開始している場合実行
        if (_isStartHeal)
        {
            // 消費SPが足りなかった場合とHPが満タンの場合はキャンセルする
            if (_playerParameter.GetPlayerSp() < _playerParameter.GetHeal_Cost() || _playerParameter.IsHPMax())
            {
                // 回復終了処理を実行
                PlayerHealExit();
                return;
            }

            // 回復時間を測る
            _playerHealTime += Time.deltaTime;

            // 回復時間が完了時間を超えたら回復実行
            if (_playerHealTime >= HEAL_COMPLETE_TIME)
            {
                // 回復時間をリセット
                _playerHealTime = 0f;

                // プレイヤーのパラメーターを変更する
                _playerParameter.AddPlayerHp(_playerParameter.GetHeal_Value());  // HPを設定
                _playerParameter.AddPlayerSp(-_playerParameter.GetHeal_Cost());   // SPを設定

                // パーティクル再生　素材ができたらここに書く


                Debugger.Log("回復したよ");
            }
        }
    }

    /// <summary>
    /// 回復開始時の処理
    /// </summary>
    public void PlayerHealStart()
    {
        // プレイヤーのステータスと着地判定を確認　回復可能かつ着地中かつHPが最大以外
        if (_playerState.GetState(E_PlayerState.Heal) && CheckFloor.CheckLanding() && !_playerParameter.IsHPMax())
        {
            // 消費SPが足りているか確認
            if (_playerParameter.GetPlayerSp() >= _playerParameter.GetHeal_Cost())
            {
                // プレイヤーのステータスを設定　回復のみTrue　それ以外False
                _playerState.SetAllState(false);
                _playerState.SetState(E_PlayerState.Heal, true);

                // 回復の開始フラグを立てる
                _isStartHeal = true;
            }
            // 消費SPが足りない場合
            else
            {
                Debugger.Log("SPたりないよ");
            }
        }
    }

    /// <summary>
    /// 回復終了時の処理
    /// </summary>
    public void PlayerHealExit()
    {
        // 回復を開始していた場合のみ
        if (_isStartHeal)
        {
            // プレイヤーのステータスを設定　全てTrue
            _playerState.SetAllState(true);

            // プレイヤーの回復時間をリセット
            _playerHealTime = 0f;

            // 回復の開始フラグを取り消す
            _isStartHeal = false;
        }
    }

    /// <summary>
    /// 攻撃判定を生成するコルーチン
    /// </summary>
    private IEnumerator Attack()
    {
        // 攻撃判定のTransformを設定
        _attackCollider.transform.position = _attackColliderPosition.position;
        _attackCollider.transform.rotation = _attackColliderPosition.rotation;

        // 攻撃判定を有効にする
        _attackCollider.SetActive(true);

        // 攻撃判定の持続時間分待つ
        yield return new WaitForSeconds(ATTACK_LIFE_TIME);

        // 攻撃判定を無効にする
        _attackCollider.SetActive(false);
    }
}
*/