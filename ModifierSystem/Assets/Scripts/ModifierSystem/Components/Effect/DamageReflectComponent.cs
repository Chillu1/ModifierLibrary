using System.Linq;
using BaseProject;

namespace ModifierSystem
{
    public class DamageReflectComponent : EffectComponent
    {
        private double Percentage { get; }

        public DamageReflectComponent(double percentage, ConditionCheckData conditionCheckData = null) : base(conditionCheckData)
        {
            Percentage = percentage;
        }

        protected override void ActualEffect(BaseBeing receiver, BaseBeing acter)
        {
            var reflectDamage = receiver.Stats.Damage.ToDamageData().ToArray();
            foreach (var data in reflectDamage)
                data.BaseDamage *= Percentage;
            receiver.DealDamage(reflectDamage, acter, AttackType.Reflect);
        }
    }
}