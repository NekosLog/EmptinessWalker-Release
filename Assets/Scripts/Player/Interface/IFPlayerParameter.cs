using UniRx;

public interface IFPlayerParameter{
    void AddPlayerHp(int value);
    void AddPlayerSp(int value);
    int GetPlayerHp { get; }
    int GetPlayerSp { get; }
    bool IsHPMax { get; }
    bool IsSPMax { get; }
    int GetAttack_Damage { get; }
    int GetSkill1_Cost { get; }
    int GetSkill1_Damage { get; }
    int GetHeal_Cost { get; }
    int GetHeal_Value { get; }
    IReadOnlyReactiveProperty<int> NowPlayerHPProperty { get; }
    IReadOnlyReactiveProperty<int> NowPlayerSPProperty { get; }
    IReadOnlyReactiveProperty<int> MaxPlayerHPProperty { get; }
    IReadOnlyReactiveProperty<int> MaxPlayerSPProperty { get; }
}