using BaseProject;

namespace ComboSystem
{
    public class DamageAttackComboModifier : SingleUseComboModifier<DamageData[]>
    {
        public DamageAttackComboModifier(string id, DamageData[] data, ComboRecipe recipe, ModifierProperties modifierProperties = default) :
            base(id, data, recipe, modifierProperties)
        {
        }

        protected override void Effect()
        {
            Target!.DealDamage(Data);
        }
    }
}