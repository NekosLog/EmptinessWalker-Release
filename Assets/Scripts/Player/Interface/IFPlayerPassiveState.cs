using UniRx;

public interface IFPlayerPassiveState
{
    IReadOnlyReactiveProperty<E_PlayerState> OnChengePassiveStateProperty { get; }
}