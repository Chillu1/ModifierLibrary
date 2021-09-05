using BaseProject;

namespace ComboSystem
{
    public class AoeDamageModifier : SingleUseModifier<IDamageData>
    {
        public AoeDamageModifier(string id, IDamageData data, ModifierProperties modifierProperties = default) : base(id, data, modifierProperties)
        {
        }

        protected override void Effect()
        {
            Target!.DealDamage(Data);
            //TODO Deal the same damage around the target
        }
    }
}