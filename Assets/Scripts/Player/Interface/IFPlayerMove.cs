/* 制作日 2023/12/15
*　製作者 ニシガキ
*　最終更新日 2023/12/15
*/
 
public interface IFPlayerMove 
{
    void PlayerWalking(E_InputType inputType);
    void PlayerStoping();
    void PlayerJump();
    void PlayerDash();
    void PlayerAttack();
    void PlayerSkill(E_InputType skillType);
    void PlayerHeal();
    void PlayerHealStart();
    void PlayerHealExit();
}