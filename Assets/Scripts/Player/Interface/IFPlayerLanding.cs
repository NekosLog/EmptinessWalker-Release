/* 制作日
*　製作者
*　最終更新日
*/

using UniRx;
 
public interface IFPlayerLanding {
    // 着地判定の変更を伝えるためのプロパティ
    public IReadOnlyReactiveProperty<bool> OnChengeIsLandingProperty { get; }
}