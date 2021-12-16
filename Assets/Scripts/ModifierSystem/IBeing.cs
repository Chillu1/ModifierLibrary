using BaseProject;

namespace ModifierSystem
{
    public interface IBeing
    {
        BaseBeing BaseBeing { get; }
        DamageData[] DealDamage(DamageData[] data);
        void AddModifier(IModifier modifier, AddModifierParameters parameters);
    }
}