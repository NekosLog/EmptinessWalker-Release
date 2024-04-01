/* 制作日 2024/02/13
*　製作者 ニシガキ
*　最終更新日 2024/02/14
*/

using UnityEngine;
using System.Collections;

public class UnUsedPlayerMove : UnUsedMoveBase
{
    // 歩行のクラス変数
    private UnUsedPlayerWalk _playerWalk = default;

    // ジャンプのクラス変数
    private UnUsedPlayerJump _playerJump = default;

    // ダッシュのクラス変数
    private UnUsedPlayerDash _playerDash = default;

    // 攻撃のクラス変数
    private UnUsedPlayerAttack _playerAttack = default;

    // スキルのクラス変数
    private UnUsedPlayerSkill _playerSkill = default;

    // 回復のクラス変数
    private UnUsedPlayerHeal _playerHeal = default;
    

    protected override void Awake()
    {
        // ベースの初期設定を行う
        base.Awake();

        // クラスのインスタンス化
        _playerWalk = new UnUsedPlayerWalk(transform);
        _playerJump = new UnUsedPlayerJump();
        _playerDash = new UnUsedPlayerDash();
        _playerAttack = new UnUsedPlayerAttack();
        _playerSkill = new UnUsedPlayerSkill();
        _playerHeal = new UnUsedPlayerHeal();
    }


}