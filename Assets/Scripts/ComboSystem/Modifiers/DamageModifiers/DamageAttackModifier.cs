using BaseProject;

namespace ComboSystem
{
    public class DamageAttackModifier : SingleUseModifier<Damages>
    {
        public DamageAttackModifier(string id, Damages data, ModifierProperties modifierProperties = default) : base(id, data, modifierProperties)
        {
        }

        protected override void Effect()
        {
            Target!.DealDamage(Data);
        }
    }
}