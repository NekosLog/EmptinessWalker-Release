using UnityEngine;
using UniRx;
 
public class PlayerAnimationManager : MonoBehaviour, IFPlayerActiveAnimation
{
    #region 変数一覧
    [SerializeField, Tooltip("プレイヤーの状態を管理するクラス")]
    private IFPlayerPassiveState _playerState = default;

    [SerializeField, Tooltip("プレイヤーのAnimator")]
    private Animator _playerAnimator = default;

    #region アニメーターステート一覧
    private string _waitFlag = "IsWait";
    private string _walkFlag = "IsWalk";
    private string _fallFlag = "IsFall";
    private string _healFlag = "IsHeal";

    private string _jumpTrigger = "DoJump";
    private string _airJumpTrigger = "DoAirJump";
    private string _attack1Trigger = "DoAttack1";
    private string _attack2Trigger = "DoAttack2";
    private string _DashTrigger = "DoDash";
    private string _skill1Trigger = "DoSkill1";
    private string _skill2Trigger = "DoSkill2";
    #endregion

    // 死亡アニメーションのAnimationの名前
    private const string DEAD_ANIMATION = "Dead";

    // ノックバックアニメーションのAnimationの名前
    private const string KNOCK_BACK_ANIMATION = "KnockBack";
    #endregion

    #region メソッド一覧
    private void Awake()
    {
        // _playerStateに自身が持つPlayerStateを代入
        _playerState = this.GetComponent<PlayerState>();

        // PassiveStateを購読してPassiveAnimationを変化させる
        _playerState.OnChengePassiveStateProperty.Subscribe(
            state => { ChengePassiveAnimation(state); }).AddTo(this);
    }

    /// <summary>
    /// 恒常的なアニメーションの状態を変更するメソッド
    /// </summary>
    /// <param name="nowState">現在の状態</param>
	private void ChengePassiveAnimation(E_PlayerState nowState)
    {
        // 待機
        if (nowState == E_PlayerState.Wait)
        {
            // アニメーションフラグを立てる
            _playerAnimator.SetBool(_waitFlag, true);
        }
        else
        {
            // アニメーションフラグを消す
            _playerAnimator.SetBool(_waitFlag, false);
        }

        // 歩行
        if (nowState == E_PlayerState.Walk)
        {
            // アニメーションフラグを立てる
            _playerAnimator.SetBool(_walkFlag, true);
        }
        else
        {
            // アニメーションフラグを消す
            _playerAnimator.SetBool(_walkFlag, false);
        }

        // 落下
        if (nowState == E_PlayerState.Fall)
        {
            // アニメーションフラグを立てる
            _playerAnimator.SetBool(_fallFlag, true);
        }
        else
        {
            // アニメーションフラグを消す
            _playerAnimator.SetBool(_fallFlag, false);
        }

        // 回復
        if (nowState == E_PlayerState.Heal)
        {
            // アニメーションフラグを立てる
            _playerAnimator.SetBool(_healFlag, true);
        }
        else
        {
            // アニメーションフラグを消す
            _playerAnimator.SetBool(_healFlag, false);
        }
    }

    /// <summary>
    /// 瞬間的なアニメーションを行うメソッド
    /// </summary>
    /// <param name="animation">行うアニメーション</param>
	public void ExecutionActiveAnimation(E_PlayerAnimation animation)
    {
        // 受け取ったアニメーションを行う
        switch (animation)
        {
            // ジャンプ
            case E_PlayerAnimation.Jump:
                _playerAnimator.SetTrigger(_jumpTrigger);
                break;

            // 空中ジャンプ
            case E_PlayerAnimation.AirJump:
                _playerAnimator.SetTrigger(_airJumpTrigger);
                break;

            // 攻撃１段目
            case E_PlayerAnimation.Attack1:
                _playerAnimator.SetTrigger(_attack1Trigger);
                break;

            // 攻撃２段目
            case E_PlayerAnimation.Attack2:
                _playerAnimator.SetTrigger(_attack2Trigger);
                break;

            // ダッシュ
            case E_PlayerAnimation.Dash:
                _playerAnimator.SetTrigger(_DashTrigger);
                break;

            // スキル１
            case E_PlayerAnimation.Skill1:
                _playerAnimator.SetTrigger(_skill1Trigger);
                break;

            // スキル２
            case E_PlayerAnimation.Skill2:
                _playerAnimator.SetTrigger(_skill2Trigger);
                break;
        }
    }

    /// <summary>
    /// 割り込みアニメーションを行うメソッド
    /// </summary>
    /// <param name="animation">行うアニメーション</param>
	public void CompelAnimation(E_PlayerAnimation animation)
    {
        // 受け取ったアニメーションを行う
        switch (animation)
        {
            // ノックバック
            case E_PlayerAnimation.KnockBack:
                _playerAnimator.Play(DEAD_ANIMATION);
                break;

            // 死亡
            case E_PlayerAnimation.Dead:
                _playerAnimator.Play(KNOCK_BACK_ANIMATION);
                break;
        }
    }
    #endregion
}