using BaseProject;

namespace ComboSystem
{
    public class DamageAttackModifier : SingleUseModifier<DamageData[]>
    {
        public DamageAttackModifier(string id, DamageData[] data, ModifierProperties modifierProperties = default) : base(id, data, modifierProperties)
        {
        }

        protected override void Effect()
        {
            Target!.DealDamage(Data);
        }
    }
}