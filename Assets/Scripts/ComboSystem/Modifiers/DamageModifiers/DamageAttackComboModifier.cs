using BaseProject;

namespace ComboSystem
{
    public class DamageAttackComboModifier : SingleUseComboModifier<Damages>
    {
        public DamageAttackComboModifier(string id, Damages data, ComboRecipe recipe, ModifierProperties modifierProperties = default) :
            base(id, data, recipe, modifierProperties)
        {
        }

        protected override void Effect()
        {
            Target!.DealDamage(Data);
        }
    }
}