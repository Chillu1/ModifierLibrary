using BaseProject;

namespace ComboSystem
{
    public class AoeDamageComboModifier : SingleUseComboModifier<DamageData[]>
    {
        public AoeDamageComboModifier(string id, DamageData[] data, ComboRecipe recipe, ModifierProperties modifierProperties = default) : base(
            id, data, recipe, modifierProperties)
        {
        }

        protected override void Effect()
        {
            Target!.DealDamage(Data);
            //TODO Deal the same damage around the target
        }
    }
}