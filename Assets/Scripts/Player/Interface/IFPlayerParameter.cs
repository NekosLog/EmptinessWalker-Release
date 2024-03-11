public interface IFPlayerParameter{
    int GetPlayerHp();
    int GetPlayerSp();
    bool IsHPMax();
    bool IsSPMax();
    void AddPlayerHp(int value);
    void AddPlayerSp(int value);
    int GetAttack_Damage();
    int GetSkill1_Cost();
    int GetSkill1_Damage();
    int GetHeal_Cost();
    int GetHeal_Value();
}