using BaseProject;

namespace ComboSystem
{
    public class DamageAttackModifier : SingleUseModifier<IDamageData>
    {
        public DamageAttackModifier(string id, IDamageData data, ModifierProperties modifierProperties = default) : base(id, data, modifierProperties)
        {
        }

        protected override void Effect()
        {
            Target!.DealDamage(Data);
        }
    }
}