using BaseProject;

namespace ComboSystem
{
    public class AoeDamageModifier : SingleUseModifier<Damages>
    {
        public AoeDamageModifier(string id, Damages data, ModifierProperties modifierProperties = default) : base(id, data, modifierProperties)
        {
        }

        protected override void Effect()
        {
            Target!.DealDamage(Data);
            //TODO Deal the same damage around the target
        }
    }
}