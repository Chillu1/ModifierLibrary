using System.Linq;
using BaseProject;

namespace ModifierSystem
{
    public sealed class DamageReflectComponent : EffectComponent
    {
        private double Percentage { get; }

        private Properties EffectProperties { get; }

        public DamageReflectComponent(Properties effectProperties, IBaseEffectProperties baseProperties = null) : base(baseProperties)
        {
            EffectProperties = effectProperties;
            
            Info = $"Damage Reflect: {EffectProperties.Percentage*100d}%\n";
        }
        
        public DamageReflectComponent(double percentage, ConditionCheckData conditionCheckData = null) : base(conditionCheckData)
        {
            Percentage = percentage;

            Info = $"Damage Reflect: {percentage*100d}%\n";
        }

        protected override void Effect(BaseProject.Unit receiver, BaseProject.Unit acter)
        {
            var reflectDamage = receiver.Stats.Damage.ToDamageData().ToArray();
            foreach (var data in reflectDamage)
                data.BaseDamage *= EffectProperties.Percentage;
            receiver.DealDamage(reflectDamage, acter, AttackType.Reflect);
        }

        public struct Properties : IEffectProperties
        {
            public double Percentage { get; }

            public Properties(double percentage)
            {
                Percentage = percentage;
            }
        }
    }
}