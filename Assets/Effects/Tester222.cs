/* 制作日
*　製作者
*　最終更新日
*/

using UnityEngine;
using System.Collections;
using UniRx;

public class Tester222 : PhysicsBase, IFPlayerLanding
{
    IFPlayerState _playerState = default;

    // 着地しているかどうかの判定　checkFloorからキャッシュしておく
    private ReactiveProperty<bool> _isLanding = new ReactiveProperty<bool>();

    bool isWait = true;
    bool isjump = true;
    bool isAttack = true;
    bool isDash = true;
    bool isSkill = true;
    bool isHeal = true;
    bool isFall = false;

    // 着地判定の変更を伝えるためのプロパティ
    public IReadOnlyReactiveProperty<bool> OnChengeIsLandingProperty
    {
        get { return _isLanding; }
    }

    private void Start()
    {
        // _playerStateに自身が持つPlayerStateを代入
        _playerState = this.GetComponent<PlayerState>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            isFall = !isFall;
        }

        if (isFall)
        {
            _isLanding.Value = false;
        }
        else
        {
            _isLanding.Value = true;
        }

        float Holizontal = Input.GetAxisRaw("Horizontal");
        float Jump = Input.GetAxisRaw("Jump");
        float Attack = Input.GetAxisRaw("Attack");
        float Dash = Input.GetAxisRaw("Dash");
        float Skill = Input.GetAxisRaw("Skill1");
        float Heal = Input.GetAxisRaw("Heal");

        if (Holizontal > 0)
        {
            transform.rotation = Quaternion.Euler(Vector3.zero);
            _playerState.OnStateFlag(E_PlayerState.Walk);
            isWait = false;
        }
        else if (Holizontal < 0)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0,180,0));
            _playerState.OnStateFlag(E_PlayerState.Walk);
            isWait = false;
        }
        else if(!isWait)
        {
            _playerState.OffStateFlag(E_PlayerState.Walk);
        }

        if (Jump != 0 && isjump)
        {
            _playerState.OnStateFlag(E_PlayerState.Jump);
            isjump = false;
        }
        else if (Jump == 0)
        {
            isjump = true;
        }

        if (Attack != 0 && isAttack)
        {
            _playerState.OnStateFlag(E_PlayerState.Attack);
            isAttack = false;
        }
        else if (Attack == 0)
        {
            isAttack = true;
        }

        if (Skill != 0 && isSkill)
        {
            _playerState.OnStateFlag(E_PlayerState.Skill1);
            isSkill = false;
        }
        else if (Skill == 0)
        {
            isSkill = true;
        }


        if (Heal != 0)
        {
            _playerState.OnStateFlag(E_PlayerState.Heal);
        }
        else if (Heal == 0)
        {
            _playerState.OffStateFlag(E_PlayerState.Heal);
        }

        if (Dash != 0 && isDash)
        {
            _playerState.OnStateFlag(E_PlayerState.Dash);
            isDash = false;
        }
        else if (Dash == 0)
        {
            isDash = true;
        }
    }
}