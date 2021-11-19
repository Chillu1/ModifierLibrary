using BaseProject;

namespace ComboSystemComposition
{
    public interface IBeing
    {
        BaseBeing BaseBeing { get; }
        string Id { get; }
        void DealDamage(DamageData[] data);
        void AddModifier(Modifier modifier, AddModifierParameters parameters);
    }
}