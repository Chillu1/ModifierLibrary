using BaseProject;

namespace ComboSystem
{
    public class DamageAttackComboModifier : SingleUseComboModifier<IDamageData>
    {
        public DamageAttackComboModifier(string id, IDamageData data, ComboRecipe recipe, ModifierProperties modifierProperties = default) :
            base(id, data, recipe, modifierProperties)
        {
        }

        protected override void Effect()
        {
            Target!.DealDamage(Data);
        }
    }
}