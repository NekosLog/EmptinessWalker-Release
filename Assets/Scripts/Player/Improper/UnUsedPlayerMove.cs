/* ����� 2024/02/13
*�@����� �j�V�K�L
*�@�ŏI�X�V�� 2024/02/14
*/

using UnityEngine;
using System.Collections;

public class UnUsedPlayerMove : UnUsedMoveBase
{
    // ���s�̃N���X�ϐ�
    private UnUsedPlayerWalk _playerWalk = default;

    // �W�����v�̃N���X�ϐ�
    private UnUsedPlayerJump _playerJump = default;

    // �_�b�V���̃N���X�ϐ�
    private UnUsedPlayerDash _playerDash = default;

    // �U���̃N���X�ϐ�
    private UnUsedPlayerAttack _playerAttack = default;

    // �X�L���̃N���X�ϐ�
    private UnUsedPlayerSkill _playerSkill = default;

    // �񕜂̃N���X�ϐ�
    private UnUsedPlayerHeal _playerHeal = default;
    

    protected override void Awake()
    {
        // �x�[�X�̏����ݒ���s��
        base.Awake();

        // �N���X�̃C���X�^���X��
        _playerWalk = new UnUsedPlayerWalk(transform);
        _playerJump = new UnUsedPlayerJump();
        _playerDash = new UnUsedPlayerDash();
        _playerAttack = new UnUsedPlayerAttack();
        _playerSkill = new UnUsedPlayerSkill();
        _playerHeal = new UnUsedPlayerHeal();
    }


}